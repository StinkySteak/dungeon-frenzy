using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Behaviour
{
    public class AutoDestroyGO : MonoBehaviour
    {
        [SerializeField] private float _lifetime;

        private void OnEnable()
        {
            Destroy(gameObject, _lifetime);
        }
    }
}
