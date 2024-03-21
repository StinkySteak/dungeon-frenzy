using Netick.Unity;
using Netick;
using StinkySteak.N2D.Gameplay.PlayerManager.Global;
using StinkySteak.N2D.Gameplay.PlayerManager.LocalPlayer;

namespace StinkySteak.N2D.Gameplay.Player.Session
{
    public class PlayerSession : NetworkBehaviour
    {
        [Networked] private NetworkString32 _nickname { get; set; }
        [Networked] private int _kill { get; set; }
        [Networked] private int _death { get; set; }

        public NetworkString32 Nickname => _nickname;

        public void AddKill()
            => _kill++;

        public void AddDeath()
            => _death++;


        [Rpc(RpcPeers.InputSource, RpcPeers.Owner, true)]
        private void RPC_SetNickname(NetworkString32 nickname)
        {
            _nickname = nickname;
        }

        public override void NetworkStart()
        {
            if (Sandbox.TryGetComponent(out GlobalPlayerManager globalPlayerManager))
                globalPlayerManager.AddPlayerSession(InputSourcePlayerId, this);

            if (!Object.IsInputSource) return;

            if (Sandbox.TryGetComponent(out LocalPlayerManager localPlayerManager))
                localPlayerManager.SessionSpawned(this);
        }
    }
}
