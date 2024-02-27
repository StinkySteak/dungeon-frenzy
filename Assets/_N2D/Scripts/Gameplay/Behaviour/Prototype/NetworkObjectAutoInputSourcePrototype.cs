using Netick.Unity;

namespace StinkySteak.N2D.Gameplay.Behaviour
{
    /// <summary>
    /// <b>Prototyping</b> <br/>
    /// Automatically grant input source to local player (server only) on <see cref="NetworkStart"/>
    /// </summary>
    public class NetworkObjectAutoInputSourcePrototype : NetworkBehaviour
    {
        public override void NetworkStart()
        {
            if (!IsServer) return;

            Object.InputSource = Sandbox.LocalPlayer;
        }
    }
}