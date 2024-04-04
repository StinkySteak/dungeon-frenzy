using Cinemachine;
using Netick.Unity;
using StinkySteak.N2D.Gameplay.Player.Character;
using StinkySteak.N2D.Gameplay.PlayerManager.LocalPlayer;
using StinkySteak.N2D.Netick;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Cam.Manager
{
    public class CameraManager : MonoBehaviour, INetickSceneLoaded
    {
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private NetworkSandbox _networkSandbox;

        private void OnCharacterSpawned(PlayerCharacter playerCharacter)
        {
            NetworkSandbox sandbox = playerCharacter.Object.Sandbox;

            sandbox.Log($"Setting Camera for player: {playerCharacter.Object.Entity.InputSourcePlayerId} Is Input Source: {playerCharacter.Object.IsInputSource} localPlayer: {sandbox.LocalPlayer.PlayerId} _networkSandbox: {sandbox.name}");
            _cinemachineVirtualCamera.Follow = playerCharacter.transform;
        }

        public void OnSceneLoaded(NetworkSandbox sandbox)
        {
            _networkSandbox = sandbox;

            LocalPlayerManager localPlayerManager = sandbox.GetComponent<LocalPlayerManager>();
            localPlayerManager.OnCharacterSpawned += OnCharacterSpawned;

            if (localPlayerManager.TryGetCharacter(out PlayerCharacter character))
            {
                OnCharacterSpawned(character);
            }
        }
    }
}