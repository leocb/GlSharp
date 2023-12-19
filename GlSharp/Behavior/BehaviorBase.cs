using GlSharp.Entities;

namespace GlSharp.Behavior;
public abstract class BehaviorBase : IBehavior
{

    public abstract void Update(IEntity entity, float time);
}
