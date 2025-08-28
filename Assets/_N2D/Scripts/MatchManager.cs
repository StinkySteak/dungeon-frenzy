using Netick;
using Netick.Unity;
using StinkySteak.N2D.Gameplay.Player.Character;
using StinkySteak.N2D.Gameplay.Player.Character.Dead;
using StinkySteak.N2D.Gameplay.Player.Session;
using StinkySteak.N2D.Gameplay.PlayerManager.Global;
using StinkySteak.N2D.Gameplay.Spawnpoints;
using StinkySteak.N2D.Netick;
using System.Collections.Generic;
using UnityEngine;
using NetworkPlayer = Netick.NetworkPlayer;

namespace StinkySteak.N2D.Launcher.Prototype
{
    public class MatchManager : NetworkEventsListener
    {
        [SerializeField] private NetworkObject _playerSessionPrefab;
        [SerializeField] private NetworkObject _playerCharacterPrefab;
        private SpawnPoints _spawnpoints;
        private GlobalPlayerManager _globalPlayerManager;

        public override void OnSceneLoaded(NetworkSandbox sandbox)
        {
            List<INetickSceneLoaded> listeners = sandbox.FindObjectsOfType<INetickSceneLoaded>();

            foreach (INetickSceneLoaded listener in listeners)
                listener.OnSceneLoaded(sandbox);

            if (!sandbox.IsServer) return;

            _spawnpoints = Sandbox.FindObjectOfType<SpawnPoints>();
            _globalPlayerManager = Sandbox.GetComponent<GlobalPlayerManager>();
        }

        private void SpawnPlayerSession(NetworkPlayerId playerId)
        {
            NetworkPlayer inputSource = Sandbox.GetPlayerById(playerId);
            NetworkObject obj = Sandbox.NetworkInstantiate(_playerSessionPrefab.gameObject, Vector3.zero, Quaternion.identity, inputSource);

            if (obj.TryGetComponent(out PlayerSession session))
            {
                session.SetNickname($"Player_{Random.Range(1000, 9999)}");
            }
        }

        public void SpawnPlayerCharacter(NetworkPlayerId playerId)
        {
            bool isPlayerExist = _globalPlayerManager.IsCharacterExist(playerId);

            if (isPlayerExist) return;

            Vector3 nextPosition = _spawnpoints.GetNext().position;

            NetworkPlayer inputSource = Sandbox.GetPlayerById(playerId);
            Sandbox.NetworkInstantiate(_playerCharacterPrefab.gameObject, nextPosition, Quaternion.identity, inputSource);
        }

        public void RespawnPlayerCharacter(NetworkPlayer player)
        {
            if (_globalPlayerManager.TryGetCharacter(player.PlayerId, out PlayerCharacter character))
            {
                PlayerCharacterDead characterDead = character.CharacterDead;

                if (characterDead.IsAlive) return;

                Vector3 nextPosition = _spawnpoints.GetNext().position;

                characterDead.TeleportTo(nextPosition);
                characterDead.SetRespawn();
            }
        }

        public override void OnPlayerJoined(NetworkSandbox sandbox, NetworkPlayerId player)
        {
            if (!Sandbox.IsServer) return;

            SpawnPlayerSession(player);
            SpawnPlayerCharacter(player);
        }

        public override void OnPlayerLeft(NetworkSandbox sandbox, NetworkPlayerId player)
        {
            if (!Sandbox.IsServer) return;

            DespawnPlayerCharacter(player);
            DespawnPlayerSession(player);
        }

        private void DespawnPlayerSession(NetworkPlayerId playerId)
        {
            if (_globalPlayerManager.TryGetSession(playerId, out PlayerSession session))
                Sandbox.Destroy(session.Object);
        }
        private void DespawnPlayerCharacter(NetworkPlayerId playerId)
        {
            if (_globalPlayerManager.TryGetCharacter(playerId, out PlayerCharacter character))
                Sandbox.Destroy(character.Object);
        }
    }
}