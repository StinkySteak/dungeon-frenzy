using Netick;
using Netick.Unity;
using StinkySteak.N2D.Gameplay.Player.Character.Dead;
using StinkySteak.N2D.Gameplay.PlayerManager.Global;
using StinkySteak.N2D.Gameplay.PlayerManager.LocalPlayer;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.Player.Character
{
    [ExecutionOrder(-99)]
    public class PlayerCharacter : NetickBehaviour
    {
        public NetworkPlayerId InputSourcePlayerId => Entity.InputSourcePlayerId;

        [SerializeField] private PlayerCharacterDead _characterDead;
        private NetworkPlayerId _inputSourcePlayerId;
        private bool _isInputSource;

        public PlayerCharacterDead CharacterDead => _characterDead;

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
