using System;
using Unity.Entities;

[Serializable]
[WriteGroup(typeof(EyesightComponent))]
public struct SomthingComponent : IComponentData {
    public int type;
    public float thinkingTime;
}