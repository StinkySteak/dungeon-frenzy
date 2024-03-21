using Netick.Unity;
using Netick;
using System;

namespace StinkySteak.N2D.Gameplay.Player.Character.Health
{
    public class PlayerCharacterHealth : NetworkBehaviour
    {
        [Networked] private int _health { get; set; }
        public const int MAX_HEALTH = 100;

        public event Action OnHealthChanged;
        public event Action OnHealthReduced;

        public int Health => _health;

        public override void NetworkStart()
        {
            _health = MAX_HEALTH;
        }

        public void ReduceHealth(int amount)
        {
            _health -= amount;

            if (_health <= 0)
            {
                Sandbox.Destroy(Object);
            }
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