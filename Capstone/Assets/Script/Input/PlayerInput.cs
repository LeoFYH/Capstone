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
        InputAction _jumpAction;
        InputAction _grindAction;
        InputAction _switchItemAction;
        InputAction _trickAction;
        public IArchitecture GetArchitecture() => GameApp.Interface;
        
        void Awake()
        {
            _inputs = new InputSystem_Actions();
            _moveAction = _inputs.Player.Move;
            _jumpAction = _inputs.Player.Jump;
            _grindAction = _inputs.Player.Grind;
            _switchItemAction = _inputs.Player.SwitchItem;
            _trickAction = _inputs.Player.Trick;
        }

        private void OnEnable() => _inputs.Enable();

        private void OnDisable() => _inputs.Disable(); 

        public FrameInput Gather()
        {
            return new FrameInput
            {
                Move = _moveAction.ReadValue<Vector2>(),
                JumpStart = _jumpAction.WasPressedThisFrame(),
                Grind = _grindAction.IsPressed(),
                SwitchItem = _switchItemAction.WasPressedThisFrame(),
                Trick = _trickAction.IsPressed(),
                TrickStart= _trickAction.WasPressedThisFrame(),
            };
        }

        void Update()
        {
            var inputModel = this.GetModel<IInputModel>();
            inputModel.Move.Value = Gather().Move;
            inputModel.JumpStart.Value = Gather().JumpStart;
            inputModel.Grind.Value = Gather().Grind;
            inputModel.SwitchItem.Value = Gather().SwitchItem;
            inputModel.Trick.Value = Gather().Trick;
            inputModel.TrickStart.Value = Gather().TrickStart;
        }

        public struct FrameInput
        {
            public Vector2 Move;
            public bool JumpStart;
            public bool Grind;
            public bool SwitchItem;
            public bool Trick;
            public bool TrickStart;
        }

    }
}