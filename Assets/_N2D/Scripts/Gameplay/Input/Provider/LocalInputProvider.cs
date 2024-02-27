using Netick.Unity;
using UnityEngine;

namespace StinkySteak.N2D.Gameplay.PlayerInput
{
    public class LocalInputProvider : NetworkEventsListener
    {
        public override void OnInput(NetworkSandbox sandbox)
        {
            PlayerCharacterInput input = new PlayerCharacterInput();
            input.Jump = Input.GetKey(KeyCode.Space);
            input.HorizontalMove = Input.GetAxis("Horizontal");

            sandbox.SetInput(input);
        }
    }
}