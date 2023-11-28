using GlSharp.Materials.Textures;

namespace GlSharp.Materials;
internal class AwesomeCrateMaterial : MaterialBase {
    public AwesomeCrateMaterial()
        : base(
            new int[] {
                TextureLoader.Load("container.jpg"),
                TextureLoader.Load("awesomeface.png")
            }, "basic.vert", "basic.frag") { }
}
