using Netick;
using Netick.Unity;
using StinkySteak.N2D.Gameplay.Player.Character.Health;
using System;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Player.Character.Dead
{
    public class PlayerCharacterDead : NetworkBehaviour
    {
        [SerializeField] private PlayerCharacterHealth _health;

        [Space]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private CircleCollider2D _collider;
        [SerializeField] private SpriteRenderer[] _renderers;
        [SerializeField] private Canvas _playerCanvas;

        [Networked] private bool _isDead { get; set; }

        public bool IsDead => _isDead;
        public bool IsAlive => !_isDead;

        public event Action<bool> OnIsDeadChanged;

        /// <summary>
        /// Sync components state for clients
        /// </summary>
        /// <param name="onChangedData"></param>
        [OnChanged(nameof(_isDead))]
        private void OnChangedIsDead(OnChangedData onChangedData)
        {
            SyncDeadToComponents(_isDead);
            OnIsDeadChanged?.Invoke(_isDead);
        }

        public void SetIsDead(bool isDead)
        {
            _isDead = isDead;

            SyncDeadToComponents(_isDead);
        }

        public void SetRespawn()
        {
            SetIsDead(false);
            _health.SetHealthToMax();
        }

        private void SyncDeadToComponents(bool isDead)
        {
            bool isAlive = !isDead;
            _rigidbody.simulated = isAlive;
            _collider.enabled = isAlive;
            _playerCanvas.enabled = isAlive;

            foreach (SpriteRenderer renderer in _renderers)
            {
                renderer.enabled = isAlive;
            }
        }
    }
}