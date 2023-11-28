using GlSharp.Materials;
using GlSharp.Materials.Textures;

using OpenTK.Mathematics;

namespace GlSharp.Models;

public class PlaneModel : BaseModel {

    public override float[] Vertices => new float[] {
      // positions        // colors          // Texture coordinates
      0.5f,  0.5f, 0.0f,  1.0f, 1.0f, 1.0f,  1.0f, 1.0f,  // top right
      0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 1.0f,  1.0f, 0.0f,  // bottom right
     -0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,  // bottom left
     -0.5f,  0.5f, 0.0f,  0.0f, 1.0f, 1.0f,  0.0f, 1.0f   // top left
    };

    public override uint[] Indices => new uint[] {
        0, 1, 3,   // first triangle
        2, 3, 1,   // second triangle
    };

    public PlaneModel() : base() {

        // Material
        Material = new MaterialBase(
            new int[] {
                TextureLoader.Load("container.jpg"),
                TextureLoader.Load("awesomeface.png")
            }, "basic.vert", "basic.frag");
    }
}
