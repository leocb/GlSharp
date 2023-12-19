using GlSharp.Materials.Textures;
using GlSharp.Tools;

namespace GlSharp.Materials;
internal class AwesomeCrateMaterial : MaterialBase
{
    public AwesomeCrateMaterial()
        : base("Basic.vert", "BasicTexture.frag", new int[] {
                TextureLoader.Load("awesomeface.png"),
                TextureLoader.Load("container.jpg")
            })
    {

        GlTools.TsGlCall(() =>
        {
            Program.Use();
            for (int i = 0; i < textureHandles?.Length; i++)
            {
                Program.SetInt($"texture{i}", i);
            }
        });
    }
}
