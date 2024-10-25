using Netick.Unity;
using Netick;
using StinkySteak.N2D.Gameplay.PlayerInput;
using UnityEngine;
using StinkySteak.Netick.Timer;
using StinkySteak.N2D.Gameplay.Player.Character.Dead;

namespace StinkySteak.N2D.Gameplay.Player.Character.Movement
{
    public class PlayerCharacterMovement : NetworkBehaviour
    {
        [SerializeField] private PlayerCharacterDead _characterDead;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _doubleJumpForce;
        [SerializeField] private float _doubleJumpDelay = 0.2f;
        [Networked] private bool _isGrounded { get; set; }
        [Networked] private int _jumpCount { get; set; }
        [Networked] private bool _jumpButtonPressed { get; set; }
        [Networked] private bool _isWalking { get; set; }

        private TickTimer _timerDoubleJumpDelay;

        [SerializeField] private Transform _groundChecker;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _groundCheckOverlapRadius = 0.1f;

        public bool IsWalking => _isWalking;

        public override void NetworkFixedUpdate()
        {
            if (_characterDead.IsDead) return;

            _isGrounded = GetIsGrounded();

            if (_isGrounded)
            {
                _jumpCount = 2;
            }

            if (FetchInput(out PlayerCharacterInput input))
            {
                bool jumpButtonWasPressedThisTick = _jumpButtonPressed == false && input.Jump;

                Vector2 velocity = _moveSpeed * input.HorizontalMove * Sandbox.FixedDeltaTime * Vector2.right;
                
                // First Jump
                if (jumpButtonWasPressedThisTick && _jumpCount == 2)
                {
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
                    _jumpCount--;
                    _timerDoubleJumpDelay = TickTimer.CreateFromSeconds(Sandbox, _doubleJumpDelay);
                }

                // Second Jump
                if (jumpButtonWasPressedThisTick && _jumpCount == 1 && _timerDoubleJumpDelay.IsExpiredOrNotRunning(Sandbox))
                {
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _doubleJumpForce);
                    _jumpCount--;
                }

                _isWalking = Mathf.Abs(velocity.x) > 0.1f;
                transform.Translate(velocity);
                _jumpButtonPressed = input.Jump;
            }
        }

        private bool GetIsGrounded()
        {
            Collider2D ground = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckOverlapRadius, _groundLayer);

            if (ground == null)
                return false;

            return true;
        }
    }
}