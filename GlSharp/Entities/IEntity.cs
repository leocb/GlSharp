namespace GlSharp.Entities;

public interface IEntity : IDisposable {
    void Draw(float time);
    void Update(float time);
}
