using UnityEngine;
using UnityEngine.InputSystem;
using QFramework;

namespace SkateGame
{
    public class PlayerInput : MonoBehaviour, ICanGetModel, IBelongToArchitecture
    {
        private IInputModel inputModel;
        InputSystem_Actions _inputs;
        InputAction _moveAction;
        InputAction _interactAction;
        InputAction _restartAction;
        InputAction _jumpAction;
        InputAction _grindAction;

        public IArchitecture GetArchitecture() => GameApp.Interface;
        
        void Awake()
        {
            _inputs = new InputSystem_Actions();
            _moveAction = _inputs.Player.Move;
            _jumpAction = _inputs.Player.Jump;
            _grindAction = _inputs.Player.Grind;
        }

        private void OnEnable() => _inputs.Enable();

        private void OnDisable() => _inputs.Disable(); 

        public FrameInput Gather()
        {
            return new FrameInput
            {
                Move = _moveAction.ReadValue<Vector2>(),
                Jump = _jumpAction.IsPressed(),
                Grind = _grindAction.IsPressed(),
            };
        }

        void Update()
        {
            var inputModel = this.GetModel<IInputModel>();
            inputModel.Move.Value = Gather().Move;
            inputModel.Jump.Value = Gather().Jump;
            inputModel.Grind.Value = Gather().Grind;
        }

        public struct FrameInput
        {
            public Vector2 Move;
            public bool Jump;
            public bool Grind;
        }

    }
}