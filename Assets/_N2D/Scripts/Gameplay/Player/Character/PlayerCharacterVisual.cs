using Netick.Unity;
using StinkySteak.N2D.Gameplay.Player.Character.Weapon;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Player.Character.Visual
{
    public class PlayerCharacterVisual : NetworkBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private PlayerCharacterWeapon _weapon;

        public override void NetworkRender()
        {
            bool flipX = _weapon.Degree < 89 && _weapon.Degree > -89;

            _spriteRenderer.flipX = !flipX;
        }
    }
}