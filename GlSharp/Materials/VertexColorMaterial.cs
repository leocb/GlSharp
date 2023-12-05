namespace GlSharp.Materials;
public class VertexColorMaterial : MaterialBase {
    public VertexColorMaterial()
        : base(Array.Empty<int>(), "Basic.vert", "basicVertColor.frag") { }
}