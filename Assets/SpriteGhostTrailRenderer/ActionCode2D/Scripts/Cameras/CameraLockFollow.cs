using UnityEngine;

namespace ActionCode2D.Cameras
{
    [DisallowMultipleComponent]
    public sealed class CameraLockFollow : MonoBehaviour
    {
        public Transform target;
        public Vector2 offset = Vector2.up * 4f;

        private void LateUpdate()
        {
            UpdatePosition();
        }

        private void OnValidate()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (target)
            {
                Vector3 pos = transform.position;
                pos.x = offset.x + target.position.x;
                pos.y = offset.y + target.position.y;
                transform.position = pos;
            }
        }
    }
}