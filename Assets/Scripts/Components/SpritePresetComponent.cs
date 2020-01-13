using System;
using Unity.Entities;

[Serializable]
public struct SpritePresetComponent : ISharedComponentData, IEquatable<SpritePresetComponent> {
    public SpriteAnimPreset value;

    public bool Equals(SpritePresetComponent other) {
        return other.value == value;
    }

    public override int GetHashCode() {
        return (null == value) ? 0 : value.GetHashCode();
    }
}