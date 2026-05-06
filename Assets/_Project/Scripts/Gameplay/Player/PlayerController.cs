using UnityEngine;
using ColorMaze.Core;

namespace ColorMaze.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 4f;
        [SerializeField] private float sprintMultiplier = 1.6f;
        [SerializeField] private float rotationSpeed = 720f;

        [Header("Jump & Gravity")]
        [SerializeField] private float jumpHeight = 1.2f;
        [SerializeField] private float gravity = -19.62f;

        [Header("References")]
        [SerializeField] private Transform cameraTransform;

        private CharacterController _cc;
        private InputSystem_Actions _input;
        private Vector2 _moveInput;
        private bool _sprintHeld;
        private bool _jumpQueued;
        private Vector3 _velocity;

        private void Awake()
        {
            _cc = GetComponent<CharacterController>();
            _input = new InputSystem_Actions();

            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;
        }

        private void OnEnable()
        {
            _input.Player.Enable();
            _input.Player.Move.performed   += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _input.Player.Move.canceled    += _   => _moveInput = Vector2.zero;
            _input.Player.Sprint.performed += _   => _sprintHeld = true;
            _input.Player.Sprint.canceled  += _   => _sprintHeld = false;
            _input.Player.Jump.performed   += _   => _jumpQueued = true;
        }

        private void OnDisable() => _input.Player.Disable();
        private void OnDestroy() => _input?.Dispose();

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Playing)
                return;

            Vector3 horizontal = ComputeHorizontalMove();
            UpdateRotation(horizontal);
            UpdateGravityAndJump();

            Vector3 motion = horizontal + Vector3.up * _velocity.y;
            _cc.Move(motion * Time.deltaTime);
        }

        private Vector3 ComputeHorizontalMove()
        {
            Vector3 forward = cameraTransform.forward; forward.y = 0; forward.Normalize();
            Vector3 right   = cameraTransform.right;   right.y   = 0; right.Normalize();

            Vector3 dir = forward * _moveInput.y + right * _moveInput.x;
            float speed = walkSpeed * (_sprintHeld ? sprintMultiplier : 1f);
            return dir * speed;
        }

        private void UpdateRotation(Vector3 horizontalMove)
        {
            if (horizontalMove.sqrMagnitude > 0.01f)
            {
                Quaternion target = Quaternion.LookRotation(horizontalMove);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, target, rotationSpeed * Time.deltaTime);
            }
        }

        private void UpdateGravityAndJump()
        {
            if (_cc.isGrounded)
            {
                if (_velocity.y < 0) _velocity.y = -2f;
                if (_jumpQueued)
                    _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            _jumpQueued = false;
            _velocity.y += gravity * Time.deltaTime;
        }
    }
}