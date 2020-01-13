using System;
using Unity.Entities;

[Serializable]
[WriteGroup(typeof(VelocityComponent))]
public struct WallComponent : IComponentData {
}