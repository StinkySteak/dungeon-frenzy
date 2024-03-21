using Netick.Unity;
using StinkySteak.N2D.Gameplay.PlayerManager.Global;
using StinkySteak.N2D.Gameplay.PlayerManager.LocalPlayer;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Player.Character
{
    public class PlayerCharacter : NetickBehaviour
    {
        public int InputSourcePlayerId => Entity.InputSourcePlayerId;

        public override void NetworkStart()
        {
            if (Sandbox.TryGetComponent(out GlobalPlayerManager globalPlayerManager))
                globalPlayerManager.AddPlayerCharacter(InputSourcePlayerId, this);

            if (!Object.IsInputSource) return;

            if (Sandbox.TryGetComponent(out LocalPlayerManager localPlayerManager))
                localPlayerManager.CharacterSpawned(this);
        }
    }
}
