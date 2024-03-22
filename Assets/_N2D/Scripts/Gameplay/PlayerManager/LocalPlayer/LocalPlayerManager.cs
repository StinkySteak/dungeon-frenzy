using Netick;
using StinkySteak.N2D.Gameplay.Player.Character;
using StinkySteak.N2D.Gameplay.Player.Session;
using System;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.PlayerManager.LocalPlayer
{
    public class LocalPlayerManager : MonoBehaviour
    {
        private PlayerSession _session;
        public PlayerSession Session => _session;

        public event Action<PlayerCharacter> OnCharacterSpawned;
        public event Action OnCharacterDespawned;
        public event Action<PlayerSession> OnSessionSpawned;

        public void CharacterSpawned(PlayerCharacter playerCharacter)
        {
            OnCharacterSpawned?.Invoke(playerCharacter);
        }
        public void CharacterDespawned()
        {
            OnCharacterDespawned?.Invoke();
        }
        public void SessionSpawned(PlayerSession playerSession)
        {
            _session = playerSession;
            OnSessionSpawned?.Invoke(playerSession);
        }
    }
}