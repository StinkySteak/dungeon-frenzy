using Netick.Unity;
using Netick;
using System;
using UnityEngine;
using StinkySteak.N2D.Gameplay.Player.Character.Dead;

namespace StinkySteak.N2D.Gameplay.Player.Character.Health
{
    public class PlayerCharacterHealth : NetworkBehaviour
    {
        [Networked] private int _health { get; set; }
        [SerializeField] private PlayerCharacterDead _characterDead; 
        public const int MAX_HEALTH = 100;

        public event Action OnHealthChanged;
        public event Action OnHealthReduced;

        public int Health => _health;

        public override void NetworkStart()
        {
            SetHealthToMax();
        }

        public void ReduceHealth(int amount)
        {
            _health -= amount;

            if (_health <= 0)
            {
                _characterDead.SetIsDead(true);
            }
        }

        public void SetHealthToMax()
        {
            _health = MAX_HEALTH;
        }

        [OnChanged(nameof(_health))]
        private void OnChangedHealth(OnChangedData onChangedData)
        {
            OnHealthChanged?.Invoke();

            int previousHealth = onChangedData.GetPreviousValue<int>();
            int currentHealth = _health;

            if (currentHealth < previousHealth)
            {
                OnHealthReduced?.Invoke();
            }
        }
    }
}