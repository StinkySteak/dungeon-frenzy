using Netick;
using Netick.Unity;
using StinkySteak.N2D.Gameplay.Bullet.Dataset;
using StinkySteak.N2D.Gameplay.Bullet.VFX;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Player.Character.Weapon
{
    public class PlayerCharacterWeaponVisual : NetickBehaviour
    {
        [SerializeField] private PlayerCharacterWeapon _weapon;
        [SerializeField] private Transform _weaponVisual;
        [SerializeField] private SpriteRenderer _weaponRenderer;
        [SerializeField] private BulletTravelVFX _bulletVfxPrefab;

        private ProjectileHit _queuedProjectile;

        public override void NetworkStart()
        {
            _weapon.OnLastProjectileHitChanged += QueueProjectileForNextFrame;
        }

        private void QueueProjectileForNextFrame()
        {
            _queuedProjectile = _weapon.LastProjectileHit;
        }

        public override void NetworkRender()
        {
            UpdateWeaponRotationVisual();
            DequeueProjectile();
        }

        private void DequeueProjectile()
        {
            bool isProjectileQueued = _queuedProjectile.IsValid;

            if (!isProjectileQueued) return;

            ProjectileHit lastProjectileHit = _queuedProjectile;
            Vector2 originPossition = transform.position;
            Vector2 hitPosition = lastProjectileHit.HitPosition;
            Vector2 bulletDirection = (hitPosition - originPossition).normalized;
            bool isHitPlayer = lastProjectileHit.IsHitPlayer;

            BulletTravelVFX bullet = Sandbox.Instantiate(_bulletVfxPrefab, originPossition, Quaternion.identity);
            bullet.Initialize(Sandbox, hitPosition, bulletDirection, isHitPlayer);

            _queuedProjectile = ProjectileHit.None;
        }

        private void UpdateWeaponRotationVisual()
        {
            var interpolator = _weapon.FindInterpolator(nameof(_weapon.Degree));
            bool didGetData = interpolator.GetInterpolationData(InterpolationSource.Auto, out float from, out float to, out float alpha);

            float interpolatedDegree;
           
            if (didGetData)
                interpolatedDegree = LerpDegree(from, to, alpha);
            else
                interpolatedDegree = _weapon.Degree;

            _weaponVisual.rotation = Quaternion.Euler(0, 0, interpolatedDegree);

            bool flipY = _weapon.Degree < 89 && _weapon.Degree > -89;
            _weaponRenderer.flipY = !flipY;
        }

        private const float INTERPOLATION_TOLERANCE = 100f;

        private float LerpDegree(float from, float to, float alpha)
        {
            float difference = Mathf.Abs(from - to);

            if (difference >= INTERPOLATION_TOLERANCE)
            {
                return to;
            }

            return Mathf.Lerp(from, to, alpha);
        }
    }
}