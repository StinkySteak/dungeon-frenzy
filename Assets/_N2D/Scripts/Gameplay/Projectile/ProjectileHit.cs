using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Bullet.Dataset
{
    public struct ProjectileHit
    {
        public int Tick;
        public Vector2 OriginPosition;
        public Vector2 HitPosition;
        public bool IsHitPlayer;

        public override string ToString()
        {
            return $"Tick: {Tick} OriginPos: {OriginPosition} HitPos: {HitPosition} IsHitPlayer: {IsHitPlayer}";
        }
    }
}
