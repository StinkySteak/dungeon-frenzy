using Netick.Unity;
using StinkySteak.Netick.Timer;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Player.Character.Health.Visual
{
    public class PlayerCharacterHealthVisual : NetickBehaviour
    {
        [SerializeField] private PlayerCharacterHealth _health;

        [Space]
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Material _materialDefault;
        [SerializeField] private Material _materialOnHit;
        [SerializeField] private float _materialOnHitLifetime = 0.2f;

        private AuthTickTimer _timerMaterialOnHitLifetime;

        public override void NetworkStart()
        {
            _health.OnHealthChanged += OnDamaged;
        }

        public override void NetworkRender()
        {
            if (_timerMaterialOnHitLifetime.IsExpired(Sandbox))
            {
                _renderer.material = _materialDefault;
                _timerMaterialOnHitLifetime = AuthTickTimer.None;
            }
        }

        private void OnDamaged()
        {
            _renderer.material = _materialOnHit;
            _timerMaterialOnHitLifetime = AuthTickTimer.CreateFromSeconds(Sandbox, _materialOnHitLifetime);
        }
    }
}
