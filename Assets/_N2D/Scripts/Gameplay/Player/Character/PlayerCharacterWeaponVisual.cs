using Netick;
using Netick.Unity;
using StinkySteak.N2D.Gameplay.Bullet.Dataset;
using StinkySteak.N2D.Gameplay.Bullet.VFX;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StinkySteak.N2D.Gameplay.Player.Character.Weapon
{
    public class PlayerCharacterWeaponVisual : NetickBehaviour
    {
        [SerializeField] private PlayerCharacterWeapon _weapon;
        [SerializeField] private Transform _weaponVisual;
        [SerializeField] private SpriteRenderer _weaponRenderer;
        [SerializeField] private BulletTravelVFX _bulletVfxPrefab;

        public override void NetworkStart()
        {
            _weapon.OnLastProjectileHitChanged += OnLastProjectileHitChanged;
        }

        private void OnLastProjectileHitChanged()
        {
            ProjectileHit lastProjectileHit = _weapon.LastProjectileHit;
            Vector2 originPossition = lastProjectileHit.OriginPosition;
            Vector2 hitPosition = lastProjectileHit.HitPosition;
            Vector2 bulletDirection = (hitPosition - originPossition).normalized;

            BulletTravelVFX bullet = Instantiate(_bulletVfxPrefab, originPossition, Quaternion.identity);
            bullet.Initialize(Sandbox, hitPosition, bulletDirection);
            
            //TODO: Temporary multipeer compatibility
            SceneManager.MoveGameObjectToScene(bullet.gameObject, Sandbox.Scene);
        }

        public override void NetworkRender()
        {
            UpdateWeaponRotationVisual();
        }

        private void UpdateWeaponRotationVisual()
        {
            var interpolator = _weapon.FindInterpolator(nameof(_weapon.Degree));
            bool didGetData = interpolator.GetInterpolationData(InterpolationMode.Auto, out float from, out float to, out float alpha);

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