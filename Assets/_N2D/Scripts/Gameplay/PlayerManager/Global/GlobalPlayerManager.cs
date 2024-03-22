using Netick.Unity;
using StinkySteak.N2D.Gameplay.Player.Character;
using StinkySteak.N2D.Gameplay.Player.Session;
using System.Collections.Generic;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.PlayerManager.Global
{
    /// <summary>
    /// Functions wrapper for <see cref="PlayerManager"/> to find player objects on the scene
    /// </summary>
    public class GlobalPlayerManagerCache
    {
        private GlobalPlayerManager _playerManager;
        private const int LIST_CAPACITY = 8;

        private bool _hasCached;
        private List<PlayerSession> _cacheSessions = new List<PlayerSession>(LIST_CAPACITY);
        private List<PlayerCharacter> _cacheCharacters = new List<PlayerCharacter>(LIST_CAPACITY);

        private Dictionary<int, PlayerSession> _playerSessionsRef;
        private Dictionary<int, PlayerCharacter> _playerCharactersRef;

        public void Initialize(Dictionary<int, PlayerSession> playerSessionsRef, Dictionary<int, PlayerCharacter> playerCharactersRef)
        {
            _playerSessionsRef = playerSessionsRef;
            _playerCharactersRef = playerCharactersRef;
        }

        public void TryCache()
        {
            if (_hasCached) return;

            NetworkSandbox sandbox = Object.FindObjectOfType<NetworkSandbox>();

            sandbox.FindObjectsOfType(_cacheSessions);
            sandbox.FindObjectsOfType(_cacheCharacters);

            ProcessCache();

            _hasCached = true;
        }

        private void ProcessCache()
        {
            foreach (PlayerSession session in _cacheSessions)
            {
                if (!_playerManager.IsSessionExist(session.InputSourcePlayerId))
                    _playerSessionsRef.Add(session.InputSourcePlayerId, session);
            }

            foreach (PlayerCharacter character in _cacheCharacters)
            {
                if (!_playerManager.IsCharacterExist(character.InputSourcePlayerId))
                    _playerCharactersRef.Add(character.InputSourcePlayerId, character);
            }
        }
    }

    public class GlobalPlayerManager : MonoBehaviour
    {
        private const int DICT_CAPACITY = 8;

        private Dictionary<int, PlayerSession> _playerSessions = new(DICT_CAPACITY);
        private Dictionary<int, PlayerCharacter> _playerCharacters = new(DICT_CAPACITY);
        private GlobalPlayerManagerCache _playerManagerCacheManager = new();



        public void AddPlayerSession(int playerId, PlayerSession playerSession)
        {
            if (IsSessionExist(playerId)) return;

            _playerSessions.Add(playerId, playerSession);
        }

        public void AddPlayerCharacter(int playerId, PlayerCharacter playerCharacter)
        {
            if (IsCharacterExist(playerId)) return;

            _playerCharacters.Add(playerId, playerCharacter);
        }

        public void RemovePlayerCharacter(int playerId)
        {
            if (!IsCharacterExist(playerId)) return;

            _playerCharacters.Remove(playerId);
        }


        public bool IsSessionExist(int playerId)
            => _playerSessions.ContainsKey(playerId);

        public bool IsCharacterExist(int playerId)
           => _playerCharacters.ContainsKey(playerId);

        public bool TryGetCharacter(int playerId, out PlayerCharacter character)
        {
            if (_playerCharacters.TryGetValue(playerId, out character))
            {
                return true;
            }

            character = null;
            return false;
        }

        public bool TryGetSession(int playerId, out PlayerSession playerSession)
        {
            if (_playerSessions.TryGetValue(playerId, out playerSession))
            {
                return true;
            }

            playerSession = null;
            return false;
        }

        public void TriggerCache()
        {
            _playerManagerCacheManager.Initialize(_playerSessions, _playerCharacters);
            _playerManagerCacheManager.TryCache();
        }
    }
}
