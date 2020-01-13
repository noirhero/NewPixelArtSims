using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(SpriteTransformSystem))]
public class SpriteRenderSystem : ComponentSystem {
    private static readonly int _mainTex = Shader.PropertyToID("_MainTex");

    private Mesh _mesh = null;
    private Material _material = null;

    protected override void OnUpdate() {
        if (null == _mesh) {
            Entities.ForEach((SpriteMeshComponent mesh) => {
                _mesh = mesh.mesh;
                _material = mesh.material;
            });
        }

        var propertyBlock = new MaterialPropertyBlock();
        Entities.ForEach((SpritePresetComponent preset, ref SpriteAnimStateComponent state, ref SpriteTransformComponent transform) => {
            propertyBlock.SetTexture(_mainTex, preset.value.GetFrame(state.hash, state.accumTime));
            Graphics.DrawMesh(_mesh, transform.Value, _material, 0, Camera.main, 0, propertyBlock);
        });
    }
}