using GlSharp.Entities;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace GlSharp.Behavior;
internal class RandomRotationBehavior:BehaviorBase {

    private Vector3 rotationVector;
    public RandomRotationBehavior() {
        rotationVector = new Vector3(
            Random.Shared.NextSingle(),
            Random.Shared.NextSingle(),
            Random.Shared.NextSingle());
    }
    public override void Update(IEntity entity, float time) {
        entity.Rotation = Quaternion.FromAxisAngle(rotationVector,(float)Engine.Time.Elapsed.TotalSeconds);
    }
}
