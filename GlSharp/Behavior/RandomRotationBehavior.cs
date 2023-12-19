using GlSharp.Entities;

using OpenTK.Mathematics;

namespace GlSharp.Behavior;
internal class RandomRotationBehavior : BehaviorBase
{

    private Vector3 rotationVector;
    public RandomRotationBehavior()
    {
        rotationVector = new Vector3(
            Random.Shared.NextSingle() - 0.5f,
            Random.Shared.NextSingle() - 0.5f,
            Random.Shared.NextSingle() - 0.5f);
    }
    public override void Update(IEntity entity, float time)
    {
        entity.Rotation = Quaternion.FromAxisAngle(rotationVector, (float)Engine.Time.Elapsed.TotalSeconds);
    }
}
