using Netick;
using Netick.Unity;
using UnityEngine;

namespace StinkySteak.N2D.Launcher.Prototype
{
    public class Netick2DStarterPrototype : NetworkEventsListener
    {
        [SerializeField] private NetworkObject _playerCharacterPrefab;

        public override void OnSceneLoaded(NetworkSandbox sandbox)
        {
            if (!sandbox.IsServer) return;

            sandbox.NetworkInstantiate(_playerCharacterPrefab.gameObject, Vector3.zero, Quaternion.identity, sandbox.LocalPlayer);
        }

        public override void OnClientConnected(NetworkSandbox sandbox, NetworkConnection client)
        {
            sandbox.NetworkInstantiate(_playerCharacterPrefab.gameObject, Vector3.zero, Quaternion.identity, client);
        }
    }
}