﻿using Assimp;

using GlSharp.Materials.Textures;
using GlSharp.Mesh;
using GlSharp.Shaders;
using GlSharp.Types;


namespace GlSharp.Models;
public static class ModelLoader
{
    private static readonly Dictionary<int, List<MeshBase>> modelList = [];

    public static List<MeshBase> LoadModel(string modelName, IProgram shader)
    {
        string ModelFilePath = Path.Combine(Environment.CurrentDirectory, "Assets", "Models", modelName, $"{modelName}.obj");

        // First, check if an model was already loaded
        int modelHash = modelName.GetHashCode() + shader.GetHashCode();
        if (modelList.TryGetValue(modelHash, out List<MeshBase>? model) && model is not null)
            return model;

        // cache not available, load!
        List<MeshBase> meshes = [];

        using AssimpContext context = new();

#warning this can throw errors, deal with them... somehow
        Assimp.Scene scene = context.ImportFile(ModelFilePath, PostProcessSteps.Triangulate);

        processNode(scene.RootNode, scene, meshes, shader, modelName);

        modelList.Add(modelHash, meshes);

        return meshes;
    }

    public static void ClearLoadedModels() => modelList.Clear();

    private static void processNode(Node node, Assimp.Scene scene, List<MeshBase> meshes, IProgram shader, string modelName)
    {
        for (int i = 0; i < node.MeshCount; i++)
        {
            MeshBase? mesh = processMesh(scene.Meshes[node.MeshIndices[i]], scene, shader, modelName);
            if (mesh is not null)
                meshes.Add(mesh);
        }

        foreach (Node child in node.Children)
        {
            processNode(child, scene, meshes, shader, modelName);
        }
    }

    private static MeshBase? processMesh(Assimp.Mesh mesh, Assimp.Scene scene, IProgram shader, string modelName)
    {
        if (!mesh.HasNormals || !mesh.HasVertices || !mesh.HasTextureCoords(0) || mesh.MaterialIndex <= 0)
        {
            Console.Error.WriteLine($"ERROR::ASSIMP::Mesh is missing some important data!");
            Console.Error.WriteLine($"    (Vertices: {mesh.HasVertices})");
            Console.Error.WriteLine($"    (Normals: {mesh.HasNormals})");
            Console.Error.WriteLine($"    (Texture UV: {mesh.HasTextureCoords(0)})");
            Console.Error.WriteLine($"    (Material: {mesh.MaterialIndex})");
            return null;
        }

        List<Vertex.Data> vertices = [];
        for (int i = 0; i < mesh.VertexCount; i++)
        {
            vertices.Add(new()
            {
                Position = new(
                    (float)mesh.Vertices[i].X,
                    (float)mesh.Vertices[i].Y,
                    (float)mesh.Vertices[i].Z),
                Normal = new(
                    (float)mesh.Normals[i].X,
                    (float)mesh.Normals[i].Y,
                    (float)mesh.Normals[i].Z),
                TexCoords = new(
                    (float)mesh.TextureCoordinateChannels[0][i].X,
                    (float)mesh.TextureCoordinateChannels[0][i].Y)
            });
        }

        List<int> indices = [.. mesh.GetIndices()];

        List<Texture.Data> textures = [];
        if (mesh.MaterialIndex >= 0)
        {
            Material material = scene.Materials[mesh.MaterialIndex];

            if (material.HasTextureDiffuse)
            {
                textures.Add(new()
                {
                    Id = TextureLoader.Load(
                        Path.Combine(modelName, material.TextureDiffuse.FilePath.Replace("textures\\", ""))),
                    Type = Texture.Type.Diffuse
                });
            }
            else
            {
                textures.Add(new() { Id = 0, Type = Texture.Type.Diffuse });
            }

            if (material.HasTextureSpecular)
            {
                textures.Add(new()
                {
                    Id = TextureLoader.Load(
                        Path.Combine(modelName, material.TextureSpecular.FilePath.Replace("textures\\", ""))),
                    Type = Texture.Type.Specular
                });
            }
            else
            {
                textures.Add(new() { Id = 0, Type = Texture.Type.Specular });
            }
        }

        return new(vertices, indices, textures, shader);
    }
}
