using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Spawnpoints
{
    public class SpawnPoints : MonoBehaviour
    {
        [SerializeField] private Transform[] _points;
        private int _indexer;

        public Transform GetNext()
        {
            if (_indexer >= _points.Length)
                _indexer = 0;

            return _points[_indexer++];
        }
    }

}
