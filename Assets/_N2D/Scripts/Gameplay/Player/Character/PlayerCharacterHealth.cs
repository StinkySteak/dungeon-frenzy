using Netick.Unity;
using Netick;
using System;

namespace StinkySteak.N2D.Gameplay.Player.Character.Health
{
    public class PlayerCharacterHealth : NetworkBehaviour
    {
        [Networked] private int _health { get; set; }

        public event Action OnHealthChanged;
        public event Action OnHealthReduced;

        public int Health => _health;

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