using Netick.Unity;
using StinkySteak.Netick.Timer;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Bullet.VFX
{
    public class BulletTravelVFX : NetickBehaviour
    {
        [SerializeField] private float _distanceToDestroy = 2f;
        [SerializeField] private float _bulletSpeed = 2f;
        [SerializeField] private float _lifetime = 2f;
        [SerializeField] private GameObject _bulletImpactVFX;

        private Vector2 _targetPosition;
        private Vector2 _bulletDirection;
        private TickTimer _timerLifetime;
        private bool _isHitPlayer;

        public void Initialize(NetworkSandbox networkSandbox, Vector2 targetPosition, Vector2 bulletDirection, bool isHitPlayer)
        {
            networkSandbox.AttachBehaviour(this);
            _targetPosition = targetPosition;
            _bulletDirection = bulletDirection;
            _isHitPlayer = isHitPlayer;

            _timerLifetime = TickTimer.CreateFromSeconds(networkSandbox, _lifetime);
        }

        public void LateUpdate()
        {
            float distance = Vector2.Distance(transform.position, _targetPosition);

            bool isDestinationReached = distance <= _distanceToDestroy;

            if (isDestinationReached)
            {
                Destroy();
                return;
            }

            if (_timerLifetime.IsExpired(Sandbox))
            {
                Destroy();
                return;
            }

            transform.position += (Vector3)_bulletDirection * Time.deltaTime * _bulletSpeed;
        }

        private void Destroy()
        {
            if (!_isHitPlayer)
            {
                Sandbox.LocalInstantiate(_bulletImpactVFX, transform.position, Quaternion.identity);
            }

            Sandbox.DetachBehaviour(this);
            Destroy(gameObject);
        }
    }
}