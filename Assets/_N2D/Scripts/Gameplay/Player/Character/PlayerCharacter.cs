using Netick.Unity;
using StinkySteak.N2D.Gameplay.PlayerManager.Global;
using StinkySteak.N2D.Gameplay.PlayerManager.LocalPlayer;

namespace StinkySteak.N2D.Gameplay.Player.Character
{
    public class PlayerCharacter : NetickBehaviour
    {
        public int InputSourcePlayerId => Entity.InputSourcePlayerId;

        private int _inputSourcePlayerId;
        private bool _isInputSource;

        public override void NetworkStart()
        {
            if (Sandbox.TryGetComponent(out GlobalPlayerManager globalPlayerManager))
                globalPlayerManager.AddPlayerCharacter(InputSourcePlayerId, this);

            _inputSourcePlayerId = InputSourcePlayerId;
            _isInputSource = Object.IsInputSource;

            if (!Object.IsInputSource) return;

            if (Sandbox.TryGetComponent(out LocalPlayerManager localPlayerManager))
                localPlayerManager.CharacterSpawned(this);
        }

        public override void NetworkDestroy()
        {
            if (_isInputSource)
            {
                if (Sandbox.TryGetComponent(out LocalPlayerManager localPlayerManager))
                    localPlayerManager.CharacterDespawned();
            }

            if (Sandbox.TryGetComponent(out GlobalPlayerManager globalPlayerManager))
                globalPlayerManager.RemovePlayerCharacter(_inputSourcePlayerId);
        }
    }
}
