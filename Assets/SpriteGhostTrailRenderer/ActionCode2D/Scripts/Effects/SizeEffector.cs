using UnityEngine;

namespace ActionCode2D.Effects
{
    [DisallowMultipleComponent]
    public sealed class SizeEffector : MonoBehaviour
    {
        [Range(0.1f, 5f)] public float speed = 1.2f;
        private Vector3 _defaultScale;

        private const float MAX_SIZE = 4f;
        private const float MIN_SIZE = 0.1f;

        private void Start()
        {
            _defaultScale = transform.localScale;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.E)) IncreaseSize();
            else if (Input.GetKey(KeyCode.Q)) DecreaseSize();
            else if (Input.GetKeyDown(KeyCode.R)) ResetSize();
        }

        public void IncreaseSize()
        {
            if (transform.localScale.x > MAX_SIZE)
            {
                transform.localScale = Vector3.one * MAX_SIZE;
                return;
            }
            transform.localScale += Vector3.one * speed * Time.deltaTime;
        }

        public void DecreaseSize()
        {
            if(transform.localScale.x < MIN_SIZE)
            {
                transform.localScale = Vector3.one * MIN_SIZE;
                return;
            }
            transform.localScale -= Vector3.one * speed * Time.deltaTime;
        }

        public void ResetSize()
        {
            transform.localScale = _defaultScale;
        }
    }
}