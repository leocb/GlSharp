using GlSharp.Entities;

namespace GlSharp.Behavior;
public interface IBehavior {
    void Update(IEntity entity, float time);
}