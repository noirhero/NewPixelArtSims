using System;
using Unity.Entities;

[Serializable]
[WriteGroup(typeof(EyesightComponent))]
public struct BoundComponent : IComponentData {
    public float posX;
    public float sizeX;
}