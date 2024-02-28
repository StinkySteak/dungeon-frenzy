using Netick.Unity;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Player.Character.Weapon
{
    public class PlayerCharacterWeaponVisual : NetickBehaviour
    {
        [SerializeField] private PlayerCharacterWeapon _weapon;
        [SerializeField] private Transform _weaponVisual;
        [SerializeField] private SpriteRenderer _weaponRenderer;

        public override void NetworkRender()
        {
            _weaponVisual.rotation = Quaternion.Euler(0, 0, _weapon.Degree);

            bool inQ1 = _weapon.Degree > 90 && _weapon.Degree < 180;
            bool inQ2 = _weapon.Degree < -90 && _weapon.Degree > -180;
            bool flipY = inQ1 || inQ2;
            _weaponRenderer.flipY = flipY;
        }
    }
}