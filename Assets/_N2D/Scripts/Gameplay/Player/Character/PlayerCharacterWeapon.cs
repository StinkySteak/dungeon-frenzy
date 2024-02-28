using UnityEngine;
using Netick;
using Netick.Unity;
using StinkySteak.N2D.Gameplay.PlayerInput;

namespace StinkySteak.N2D.Gameplay.Player.Character.Weapon
{
    public class PlayerCharacterWeapon : NetworkBehaviour
    {
        [SerializeField] private float _fireRate;
        [Networked][Smooth] private float _degree { get; set; }

        public float Degree => _degree;

        public override void NetworkFixedUpdate()
        {
            if (FetchInput(out PlayerCharacterInput input))
            {
                _degree = input.LookDegree;
            }
        }
    }
}