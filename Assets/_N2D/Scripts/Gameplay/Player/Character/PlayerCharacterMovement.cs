using Netick.Unity;
using StinkySteak.N2D.Gameplay.PlayerInput;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Player.Movement
{
    public class PlayerCharacterMovement : NetworkBehaviour
    {
        [SerializeField] private float _moveSpeed;

        public override void NetworkFixedUpdate()
        {
            if (FetchInput(out PlayerCharacterInput input))
            {
                Vector2 velocity = _moveSpeed * input.HorizontalMove * Sandbox.FixedDeltaTime * Vector2.right;

                transform.Translate(velocity);
            }
        }
    }
}