using System;
using UnityEngine;

namespace ActionCode2D.Motors
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class SimplePlayerMotor : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private string _horizontalAxisInput = "Horizontal";
        [SerializeField] private string _jumpButtonInput = "Jump";

        [Header("Animator")]
        [SerializeField] private string _hInputParam = "hInput";
        [SerializeField] private string _vSpeedParam = "vSpeed";
        [SerializeField] private string _jumpParam = "jump";
        [SerializeField] private string _groundedParam = "grounded";

        [Header("Physics")]
        [Range(0f, 20f)] public float speed = 10f;
        [Range(0f, 10f)] public float jumpForce = 5f;
        public ContactFilter2D groundFilter;


        [Header("Components")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _renderer;

        private float _horInput = 0f;
        private float _lastDirection = 1f;

        private int _hInputId;
        private int _vSpeedId;
        private int _jumpId;
        private int _groundedId;

        private bool _isGrounded = false;
        private bool _hasJumped = false;

        private void Reset()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _hInputId = Animator.StringToHash(_hInputParam);
            _vSpeedId = Animator.StringToHash(_vSpeedParam);
            _jumpId = Animator.StringToHash(_jumpParam);
            _groundedId = Animator.StringToHash(_groundedParam);
        }

        private void Update()
        {
            if(Time.timeScale > 0.1f)
            {
                UpdateInput();
                UpdateAnimator();
            }
        }

        private void FixedUpdate()
        {
            UpdatePhysics();
        }

        private void UpdatePhysics()
        {
            transform.position += Vector3.right * _horInput * speed * Time.deltaTime;
            _isGrounded = _rigidbody.IsTouching(groundFilter);
            if (_hasJumped) _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        private void UpdateInput()
        {
            _horInput = Input.GetAxis(_horizontalAxisInput);
            if (_horInput * _lastDirection < 0f) FlipHorizontally();

            _hasJumped = _isGrounded && Input.GetButtonDown(_jumpButtonInput);

            if (_horInput < 0f) _lastDirection = -1f;
            else if (_horInput > 0f) _lastDirection = 1f;
        }

        private void UpdateAnimator()
        {
            _animator.SetFloat(_hInputId, Mathf.Abs(_horInput));
            _animator.SetFloat(_vSpeedId, _rigidbody.velocity.y);
            _animator.SetBool(_groundedId, _isGrounded);
            if (_hasJumped) _animator.SetTrigger(_jumpId);
        }



        private void FlipHorizontally()
        {
            _renderer.flipX = !_renderer.flipX;
        }
    }
}