using Cinemachine;
using Netick.Unity;
using StinkySteak.N2D.Gameplay.Player.Character;
using StinkySteak.N2D.Gameplay.PlayerManager.LocalPlayer;
using StinkySteak.N2D.Netick;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Cam.Manager
{
    public class CameraManager : NetickBehaviour, INetickSceneLoaded
    {
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private void OnCharacterSpawned(PlayerCharacter playerCharacter)
        {
            _cinemachineVirtualCamera.Follow = playerCharacter.transform;
        }

        public void OnSceneLoaded(NetworkSandbox sandbox)
        {
            AttachBehaviour(sandbox);

            LocalPlayerManager localPlayerManager = sandbox.GetComponent<LocalPlayerManager>();
            localPlayerManager.OnCharacterSpawned += OnCharacterSpawned;

            if (localPlayerManager.TryGetCharacter(out PlayerCharacter character))
            {
                OnCharacterSpawned(character);
            }
        }
        private void AttachBehaviour(NetworkSandbox sandbox)
        {
#if UNITY_EDITOR
            sandbox.AttachBehaviour(this);
#endif
        }
#if UNITY_EDITOR

        public override void NetworkRender()
        {
            _cinemachineVirtualCamera.enabled = Sandbox.IsVisible;
        }
#endif
    }
}