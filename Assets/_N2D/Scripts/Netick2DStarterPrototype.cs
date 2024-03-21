using Netick;
using Netick.Unity;
using StinkySteak.N2D.Finder;
using StinkySteak.N2D.Gameplay.Player.Character;
using StinkySteak.N2D.Gameplay.Player.Session;
using StinkySteak.N2D.Gameplay.PlayerManager.Global;
using StinkySteak.N2D.Netick;
using System.Collections.Generic;
using UnityEngine;
using NetworkPlayer = Netick.NetworkPlayer;

namespace StinkySteak.N2D.Launcher.Prototype
{
    public class Netick2DStarterPrototype : NetworkEventsListener
    {
        [SerializeField] private NetworkObject _playerSessionPrefab;
        [SerializeField] private NetworkObject _playerCharacterPrefab;

        public override void OnSceneLoaded(NetworkSandbox sandbox)
        {
            List<INetickSceneLoaded> listeners = ObjectFinder.FindPreAlloc<INetickSceneLoaded>();

            foreach (INetickSceneLoaded listener in listeners)
                listener.OnSceneLoaded(sandbox);

            if (!sandbox.IsServer) return;

            sandbox.NetworkInstantiate(_playerCharacterPrefab.gameObject, Vector3.zero, Quaternion.identity, sandbox.LocalPlayer);
            sandbox.NetworkInstantiate(_playerSessionPrefab.gameObject, Vector3.zero, Quaternion.identity, sandbox.LocalPlayer);
        }

        public override void OnClientConnected(NetworkSandbox sandbox, NetworkConnection client)
        {
            sandbox.NetworkInstantiate(_playerCharacterPrefab.gameObject, Vector3.zero, Quaternion.identity, client);
            sandbox.NetworkInstantiate(_playerSessionPrefab.gameObject, Vector3.zero, Quaternion.identity, client);
        }

        public override void OnClientDisconnected(NetworkSandbox sandbox, NetworkConnection client, TransportDisconnectReason transportDisconnectReason)
        {
            DespawnPlayerCharacter(client);
            DespawnPlayerSession(client);
        }

        private void DespawnPlayerSession(NetworkPlayer networkConnection)
        {
            GlobalPlayerManager playerManager = Sandbox.GetComponent<GlobalPlayerManager>();

            if (playerManager.TryGetSession(networkConnection.PlayerId, out PlayerSession session))
                Sandbox.Destroy(session.Object);
        }
        private void DespawnPlayerCharacter(NetworkPlayer networkConnection)
        {
            GlobalPlayerManager playerManager = Sandbox.GetComponent<GlobalPlayerManager>();

            if (playerManager.TryGetCharacter(networkConnection.PlayerId, out PlayerCharacter character))
                Sandbox.Destroy(character.Object);
        }
    }
}