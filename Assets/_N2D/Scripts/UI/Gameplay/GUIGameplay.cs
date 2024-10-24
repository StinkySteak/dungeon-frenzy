using Netick.Unity;
using StinkySteak.N2D.Gameplay.Player.Character;
using StinkySteak.N2D.Gameplay.Player.Character.Dead;
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
        private PlayerCharacterDead _localPlayerDead;
        private LocalPlayerManager _localPlayerManager;

        public void OnSceneLoaded(NetworkSandbox sandbox)
        {
            _networkSandbox = sandbox;

            _localPlayerManager = _networkSandbox.GetComponent<LocalPlayerManager>();
            _localPlayerManager.OnCharacterSpawned += OnCharacterSpawned;

            _buttonSetNickname.onClick.AddListener(OnButtonSetNickname);
            _buttonRespawn.onClick.AddListener(OnButtonRespawn);
        }

        private void OnButtonSetNickname()
        {
            _localPlayerManager.Session.RPC_SetNickname(_inputField.text);
        }

        private void OnButtonRespawn()
        {
            _localPlayerManager.Session.RPC_Respawn();
        }

        private void OnCharacterSpawned(PlayerCharacter obj)
        {
            _localPlayerDead = obj.CharacterDead;
            _localPlayerDead.OnIsDeadChanged += OnLocalPlayerIsDead;

            _buttonRespawn.gameObject.SetActive(false);
        }

        private void OnLocalPlayerIsDead(bool isDead)
        {
            _buttonRespawn.gameObject.SetActive(isDead);
        }
    }
}