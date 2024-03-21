using StinkySteak.N2D.Gameplay.Player.Character;
using StinkySteak.N2D.Gameplay.Player.Session;
using System;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.PlayerManager.LocalPlayer
{
    public class LocalPlayerManager : MonoBehaviour
    {
        public event Action<PlayerCharacter> OnCharacterSpawned;
        public event Action<PlayerSession> OnSessionSpawned;

        public void CharacterSpawned(PlayerCharacter playerCharacter)
        {
            OnCharacterSpawned?.Invoke(playerCharacter);
        }
        public void SessionSpawned(PlayerSession playerSession)
        {
            OnSessionSpawned?.Invoke(playerSession);
        }
    }
}