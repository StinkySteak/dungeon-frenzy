using Netick;
using StinkySteak.N2D.Gameplay.Player.Character;
using StinkySteak.N2D.Gameplay.Player.Session;
using System.Collections.Generic;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.PlayerManager.Global
{
    public class GlobalPlayerManager : MonoBehaviour
    {
        private const int DICT_CAPACITY = 8;

        private Dictionary<NetworkPlayerId, PlayerSession> _playerSessions = new(DICT_CAPACITY);
        private Dictionary<NetworkPlayerId, PlayerCharacter> _playerCharacters = new(DICT_CAPACITY);
        public void AddPlayerSession(NetworkPlayerId playerId, PlayerSession playerSession)
        {
            if (IsSessionExist(playerId)) return;

            _playerSessions.Add(playerId, playerSession);
        }

        public void AddPlayerCharacter(NetworkPlayerId playerId, PlayerCharacter playerCharacter)
        {
            if (IsCharacterExist(playerId)) return;

            _playerCharacters.Add(playerId, playerCharacter);
        }

        public void RemovePlayerCharacter(NetworkPlayerId playerId)
        {
            if (!IsCharacterExist(playerId)) return;

            _playerCharacters.Remove(playerId);
        }


        public bool IsSessionExist(NetworkPlayerId playerId)
            => _playerSessions.ContainsKey(playerId);

        public bool IsCharacterExist(NetworkPlayerId playerId)
           => _playerCharacters.ContainsKey(playerId);

        public bool TryGetCharacter(NetworkPlayerId playerId, out PlayerCharacter character)
        {
            if (_playerCharacters.TryGetValue(playerId, out character))
            {
                return true;
            }

            character = null;
            return false;
        }

        public bool TryGetSession(NetworkPlayerId playerId, out PlayerSession playerSession)
        {
            if (_playerSessions.TryGetValue(playerId, out playerSession))
            {
                return true;
            }

            playerSession = null;
            return false;
        }
    }
}
