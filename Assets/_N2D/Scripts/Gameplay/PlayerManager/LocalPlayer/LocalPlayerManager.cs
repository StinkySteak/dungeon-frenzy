using StinkySteak.N2D.Gameplay.Player.Character;
using System;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.PlayerManager.LocalPlayer
{
    public class LocalPlayerManager : MonoBehaviour
    {
        public event Action<PlayerCharacter> OnLocalPlayerSpawned;

        public void LocalPlayerSpawned(PlayerCharacter playerCharacter)
        {
            OnLocalPlayerSpawned?.Invoke(playerCharacter);
        }
    }
}