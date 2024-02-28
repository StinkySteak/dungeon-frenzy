using Netick.Unity;
using StinkySteak.N2D.Gameplay.PlayerManager.LocalPlayer;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Player.Character
{
    public class PlayerCharacter : NetickBehaviour
    {
        public override void NetworkStart()
        {
            if (!Object.IsInputSource) return;

            if (Sandbox.TryGetComponent(out LocalPlayerManager localPlayerManager))
                localPlayerManager.LocalPlayerSpawned(this);
        }
    }
}
