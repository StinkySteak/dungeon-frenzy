using Netick.Unity;
using StinkySteak.N2D.Gameplay.Player.Character.Movement;
using StinkySteak.N2D.Gameplay.Player.Character.Weapon;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Player.Character.Animate
{
    public class PlayerCharacterAnimator : NetickBehaviour
    {
        [SerializeField] private PlayerCharacterMovement _movement;
        [SerializeField] private Animator _animator;
        
        [Header("Weapon")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private PlayerCharacterWeapon _weapon;

        private readonly int PARAM_ANIMATION_SPEED = Animator.StringToHash("IsWalking");

        public override void NetworkRender()
        {
            PlayAnimation();
            FlipPlayer();
        }

        private void PlayAnimation()
        {
            bool isWalking = _movement.IsWalking;

            _animator.SetBool(PARAM_ANIMATION_SPEED, isWalking);
        }

        private void FlipPlayer()
        {
            bool flipX = _weapon.Degree < 89 && _weapon.Degree > -89;

            _spriteRenderer.flipX = !flipX;
        }
    }
}