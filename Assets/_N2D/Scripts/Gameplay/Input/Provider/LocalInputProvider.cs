using Netick.Unity;
using StinkySteak.N2D.Gameplay.Player.Character;
using StinkySteak.N2D.Gameplay.PlayerManager.LocalPlayer;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.PlayerInput
{
    public class LocalInputProvider : NetworkEventsListener
    {
        private PlayerCharacter _localPlayer;

        public override void OnStartup(NetworkSandbox sandbox)
        {
            if (sandbox.TryGetComponent(out LocalPlayerManager localPlayerManager))
            {
                localPlayerManager.OnCharacterSpawned += OnLocalPlayerSpawned;
            }
        }

        private void OnLocalPlayerSpawned(PlayerCharacter playerCharacter)
        {
            _localPlayer = playerCharacter;
        }

        public override void OnInput(NetworkSandbox sandbox)
        {
            PlayerCharacterInput input = new PlayerCharacterInput();
            input.Jump = Input.GetKey(KeyCode.Space);
            input.HorizontalMove = Input.GetAxis("Horizontal");
            input.LookDegree = GetLookDegree();
            input.IsFiring = Input.GetKey(KeyCode.Mouse0);

            sandbox.SetInput(input);
        }

        private float GetLookDegree()
        {
            Vector2 mouseWorldSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 playerPosition = GetPlayerPosition();
            Vector2 lookDirection = mouseWorldSpace - playerPosition;

            return Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        }

        private Vector2 GetPlayerPosition()
        {
            if (_localPlayer == null)
            {
                return Vector2.zero;
            }

            return _localPlayer.transform.position;
        }
    }
}