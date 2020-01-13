using System;
using Unity.Entities;
using Unity.Transforms;

[Serializable]
[GenerateAuthoringComponent]
[WriteGroup(typeof(Translation))]
public struct PropertyComponent : IComponentData {
    public float strength;
    public float agility;
    public float mentality;
    public float intelligence;
    public float search;
    public float eyesight;
}