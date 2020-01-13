using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class SpriteProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public SpriteAnimPreset preset = null;

    private bool SettingTransformComponents(Entity entity, EntityManager dstManager) {
        var sprite = GetComponent<SpriteRenderer>()?.sprite;
        if (null == sprite) {
            Debug.LogError("Set sprite renderer in sprite!!!!!");
            dstManager.DestroyEntity(entity);
            return false;
        }

        dstManager.RemoveComponent<LocalToWorld>(entity);

        var spriteScale = new float3(sprite.rect.width, sprite.rect.height, 1.0f) / sprite.pixelsPerUnit;
        spriteScale *= GetComponent<Transform>().localScale;
        spriteScale.z = 1.0f;
        dstManager.AddComponentData(entity, new NonUniformScale() {
            Value = spriteScale
        });
        dstManager.AddComponentData(entity, new SpriteTransformComponent());

        return true;
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (false == SettingTransformComponents(entity, dstManager)) {
            return;
        }

        dstManager.AddComponentData(entity, new SpriteAnimStateComponent() {
            hash = preset.datas.First().Key
        });
        dstManager.AddSharedComponentData(entity, new SpritePresetComponent() {
            value = preset
        });
    }
}