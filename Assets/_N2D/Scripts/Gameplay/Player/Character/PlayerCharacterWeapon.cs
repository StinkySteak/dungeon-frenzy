using UnityEngine;
using Netick;
using Netick.Unity;
using StinkySteak.N2D.Gameplay.PlayerInput;

namespace StinkySteak.N2D.Gameplay.Player.Character.Weapon
{
    public class PlayerCharacterWeapon : NetworkBehaviour
    {
        [SerializeField] private float _fireRate;
        [Networked][Smooth(false)] public float Degree { get; private set; }

        public override void NetworkFixedUpdate()
        {
            if (FetchInput(out PlayerCharacterInput input))
            {
                Degree = input.LookDegree;
            }
        }
    }
}