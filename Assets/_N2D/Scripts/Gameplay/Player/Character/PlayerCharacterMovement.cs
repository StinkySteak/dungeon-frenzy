using Netick.Unity;
using Netick;
using StinkySteak.N2D.Gameplay.PlayerInput;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Player.Character.Movement
{
    public class PlayerCharacterMovement : NetworkBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpForce;
        [Networked] private bool _isGrounded { get; set; }
        [SerializeField] private Transform _groundChecker;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _groundCheckOverlapRadius = 0.1f;

        public override void NetworkFixedUpdate()
        {
            _isGrounded = GetIsGrounded();

            if (FetchInput(out PlayerCharacterInput input))
            {
                Vector2 velocity = _moveSpeed * input.HorizontalMove * Sandbox.FixedDeltaTime * Vector2.right;

                if (input.Jump && _isGrounded)
                {
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
                }

                transform.Translate(velocity);
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