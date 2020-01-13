using Unity.Entities;
using Unity.Transforms;

[GenerateAuthoringComponent]
[WriteGroup(typeof(Translation))]
public struct VelocityComponent : IComponentData {
    public float value;
    public float dir;
}