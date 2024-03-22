using UnityEngine;
using Netick;
using Netick.Unity;
using StinkySteak.N2D.Gameplay.PlayerInput;
using StinkySteak.N2D.Gameplay.Player.Character.Health;
using System;
using StinkySteak.N2D.Gameplay.Bullet.Dataset;
using StinkySteak.Netick.Timer;

namespace StinkySteak.N2D.Gameplay.Player.Character.Weapon
{
    public class PlayerCharacterWeapon : NetworkBehaviour
    {
        [SerializeField] private float _fireRate;
        [SerializeField] private int _damage;
        [SerializeField] private float _distance;
        [SerializeField] private float _weaponOriginPointOffset = 1.5f;
        [SerializeField] private LayerMask _hitableLayer;
        [SerializeField] private float _ammoReplenishDelay = 2f;
        [SerializeField] private float _ammoReplenishSpeed = 1f;
        [SerializeField] private float _maxAmmo = 10f;

        [Networked][Smooth(false)] public float Degree { get; private set; }
        [Networked] private ProjectileHit _lastProjectileHit { get; set; }
        [Networked] private TickTimer _timerFireRate { get; set; }
        [Networked] private float _ammo { get; set; }
        [Networked] private TickTimer _timerAmmoReplenish { get; set; }

        public ProjectileHit LastProjectileHit => _lastProjectileHit;

        public event Action OnLastProjectileHitChanged;
        public event Action OnAmmoChanged;

        public float MaxAmmo => _maxAmmo;
        public float Ammo => _ammo;

        [OnChanged(nameof(_ammo))]
        private void OnChangedAmmo(OnChangedData onChangedData)
        {
            OnAmmoChanged?.Invoke();
        }

        public override void NetworkStart()
        {
            _ammo = _maxAmmo;
        }

        [OnChanged(nameof(_lastProjectileHit))]
        private void OnChanged(OnChangedData onChangedData)
        {
            ProjectileHit old = onChangedData.GetPreviousValue<ProjectileHit>();
            ProjectileHit current = _lastProjectileHit;

            if (old.Tick == current.Tick) return;

            OnLastProjectileHitChanged?.Invoke();
        }

        public override void NetworkFixedUpdate()
        {
            ProcessAim();
            ProcessShooting();
            ProcessAmmoReplenish();
        }

        private void ProcessAmmoReplenish()
        {
            if (!IsServer) return;

            if (_timerAmmoReplenish.IsExpired(Sandbox))
            {
                _ammo += Sandbox.FixedDeltaTime * _ammoReplenishSpeed;

                if (_ammo >= _maxAmmo)
                    _ammo = _maxAmmo;
            }
        }

        private void ProcessAim()
        {
            if (!FetchInput(out PlayerCharacterInput input)) return;

            Degree = input.LookDegree;
        }

        private void ProcessShooting()
        {
            if (!FetchInput(out PlayerCharacterInput input)) return;

            if (!input.IsFiring) return;

            if (!_timerFireRate.IsExpiredOrNotRunning(Sandbox)) return;

            if (!IsServer) return;

            if (_ammo <= 0) return;

            _timerFireRate = TickTimer.CreateFromSeconds(Sandbox, _fireRate);

            Vector2 direction = DegreesToDirection(Degree);

            Vector2 originPoint = GetWeaponOriginPoint(direction);

            RaycastHit2D hit = Physics2D.Raycast(originPoint, direction, _distance, _hitableLayer);
            _timerAmmoReplenish = TickTimer.CreateFromSeconds(Sandbox, _ammoReplenishDelay);
            _ammo--;

            if (!hit)
            {
                Vector2 fakeHitPosition = originPoint + (direction * 1000f);

                _lastProjectileHit = new ProjectileHit()
                {
                    Tick = Sandbox.Tick.TickValue,
                    HitPosition = fakeHitPosition,
                    OriginPosition = originPoint,
                    IsHitPlayer = false,
                };
                return;
            }

            bool isHitPlayer = false;

            if (hit.transform.TryGetComponent(out PlayerCharacterHealth playerCharacterHealth))
            {
                isHitPlayer = true;

                playerCharacterHealth.ReduceHealth(_damage);
            }

            _lastProjectileHit = new ProjectileHit()
            {
                Tick = Sandbox.Tick.TickValue,
                HitPosition = hit.point,
                OriginPosition = originPoint,
                IsHitPlayer = isHitPlayer,
            };
        }

        private Vector2 GetWeaponOriginPoint(Vector3 direction)
          => transform.position + (direction * _weaponOriginPointOffset);

        public Vector2 DegreesToDirection(float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;

            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);

            return new Vector2(x, y);
        }
    }
}