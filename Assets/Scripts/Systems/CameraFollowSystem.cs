using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class CameraFollowSystem : ComponentSystem {
    private Transform _cameraTransform = null;

    protected override void OnUpdate() {
        if (null == _cameraTransform) {
            _cameraTransform = Camera.main?.transform;
            if (null == _cameraTransform) {
                return;
            }
        }

        var deltaTime = Time.DeltaTime;
        Entities
            .WithAll<CameraFollowerComponent>()
            .ForEach((ref Translation pos) => {
                var cameraPos = _cameraTransform.position;
                var at = (pos.Value.x - cameraPos.x) * deltaTime;
                _cameraTransform.position = new Vector3(cameraPos.x + at, cameraPos.y, cameraPos.z);
            });
    }
}