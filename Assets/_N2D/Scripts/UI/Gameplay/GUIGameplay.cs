using Netick.Unity;
using StinkySteak.N2D.Gameplay.Player.Character;
using StinkySteak.N2D.Gameplay.PlayerManager.LocalPlayer;
using StinkySteak.N2D.Netick;
using UnityEngine;
using UnityEngine.UI;

namespace StinkySteak.N2D.UI.Gameplay
{
    public class GUIGameplay : MonoBehaviour, INetickSceneLoaded
    {
        [SerializeField] private Button _buttonRespawn;
        private NetworkSandbox _networkSandbox;

        public void OnSceneLoaded(NetworkSandbox sandbox)
        {
            _networkSandbox = sandbox;

            LocalPlayerManager localPlayerManager = _networkSandbox.GetComponent<LocalPlayerManager>();
            localPlayerManager.OnCharacterSpawned += OnCharacterSpawned;
            localPlayerManager.OnCharacterDespawned += OnCharacterDespawned;

            _buttonRespawn.onClick.AddListener(OnButtonRespawn);
        }

        private void OnButtonRespawn()
        {
            _networkSandbox.GetComponent<LocalPlayerManager>().Session.RPC_Respawn();
        }

        private void OnCharacterDespawned()
        {
            _buttonRespawn.gameObject.SetActive(true);
        }

        private void OnCharacterSpawned(PlayerCharacter obj)
        {
            _buttonRespawn.gameObject.SetActive(false);
        }
    }
}