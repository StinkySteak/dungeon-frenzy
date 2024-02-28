using Netick.Unity;
using Netick;
using System;

namespace StinkySteak.N2D.Gameplay.Player.Character.Health
{
    public class PlayerCharacterHealth : NetworkBehaviour
    {
        [Networked] private int _damagedCount { get; set; }

        public event Action OnDamaged;

        public void TriggerDamage()
            => _damagedCount++;

        [OnChanged(nameof(_damagedCount))]
        private void OnDamagedCountChanged(OnChangedData onChangedData)
        {
            OnDamaged?.Invoke();
        }
    }
}