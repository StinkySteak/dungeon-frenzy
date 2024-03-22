using Netick;
using Netick.Unity;
using StinkySteak.N2D.Gameplay.Player.Character.Health;
using StinkySteak.N2D.Gameplay.Player.Character.Weapon;
using StinkySteak.N2D.Gameplay.Player.Session;
using StinkySteak.N2D.Gameplay.PlayerManager.Global;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StinkySteak.N2D.Gameplay.Player.Character.UI
{
    public class PlayerCharacterUIStatus : NetickBehaviour
    {
        [SerializeField] private TMP_Text _textNametag;

        [SerializeField] private PlayerCharacterHealth _health;
        [SerializeField] private PlayerCharacterWeapon _weapon;
        [SerializeField] private Slider _healthbar;
        [SerializeField] private Slider _ammobar;
        private PlayerSession _session;

        public override void NetworkStart()
        {
            _health.OnHealthChanged += OnHealthChanged;
            _weapon.OnAmmoChanged += OnAmmoChanged;

            GlobalPlayerManager globalPlayerManager = Sandbox.GetComponent<GlobalPlayerManager>();

            if (globalPlayerManager.TryGetSession(Entity.InputSourcePlayerId, out PlayerSession session))
            {
                _textNametag.SetText(session.Nickname);
                _session = session;
                _session.OnNicknameChanged += OnNicknameChanged;
            }
        }

        private void OnNicknameChanged()
        {
            _textNametag.SetText(_session.Nickname);
        }

        private void OnAmmoChanged()
        {
            _ammobar.maxValue = _weapon.MaxAmmo;
            _ammobar.value = _weapon.Ammo;
        }

        private void OnHealthChanged()
        {
            _healthbar.maxValue = PlayerCharacterHealth.MAX_HEALTH;
            _healthbar.value = _health.Health;
        }
    }
}