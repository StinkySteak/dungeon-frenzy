using Netick;

namespace StinkySteak.N2D.Gameplay.PlayerInput
{
    public struct PlayerCharacterInput : INetworkInput
    {
        public float HorizontalMove;
        public bool Jump;
        public float LookDegree;
    }
}
