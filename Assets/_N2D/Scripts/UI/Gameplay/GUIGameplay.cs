using Netick.Unity;
using StinkySteak.N2D.Gameplay.Player.Character;
using StinkySteak.N2D.Gameplay.PlayerManager.LocalPlayer;
using StinkySteak.N2D.Netick;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StinkySteak.N2D.UI.Gameplay
{
    public class GUIGameplay : MonoBehaviour, INetickSceneLoaded
    {
        [SerializeField] private Button _buttonRespawn;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _buttonSetNickname;
        private NetworkSandbox _networkSandbox;

        public void OnSceneLoaded(NetworkSandbox sandbox)
        {
            _networkSandbox = sandbox;

            LocalPlayerManager localPlayerManager = _networkSandbox.GetComponent<LocalPlayerManager>();
            localPlayerManager.OnCharacterSpawned += OnCharacterSpawned;
            localPlayerManager.OnCharacterDespawned += OnCharacterDespawned;

            _buttonSetNickname.onClick.AddListener(OnButtonSetNickname);
            _buttonRespawn.onClick.AddListener(OnButtonRespawn);
        }

        private void OnButtonSetNickname()
        {
            _networkSandbox.GetComponent<LocalPlayerManager>().Session.RPC_SetNickname(_inputField.text);
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