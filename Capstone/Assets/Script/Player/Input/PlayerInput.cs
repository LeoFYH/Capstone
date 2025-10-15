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
        InputAction _pushAction;
        InputAction _shootStartAction;
        InputAction _shootEndAction;
        InputAction _aimDirectionAction;
        public IArchitecture GetArchitecture() => GameApp.Interface;
        
        void Awake()
        {
            _inputs = new InputSystem_Actions();
            _moveAction = _inputs.Player.Move;
            _jumpAction = _inputs.Player.Jump;
            _grindAction = _inputs.Player.Grind;
            _switchItemAction = _inputs.Player.SwitchItem;
            _trickAction = _inputs.Player.Trick;
            _pushAction = _inputs.Player.Push;
            _shootStartAction = _inputs.Player.Shoot;
            _shootEndAction = _inputs.Player.Shoot;
            _aimDirectionAction = _inputs.Player.AimDirection;
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
                Push = _pushAction.WasPressedThisFrame(),
                ShootStart = _shootStartAction.WasPressedThisFrame(),
                ShootEnd = _shootEndAction.WasReleasedThisFrame(),
                AimDirection = _aimDirectionAction.ReadValue<Vector2>(),
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
            inputModel.Push.Value = Gather().Push;
            inputModel.ShootStart.Value = Gather().ShootStart;
            inputModel.ShootEnd.Value = Gather().ShootEnd;
			inputModel.AimDirection.Value = GetAimDirection();
        }

		private Vector2 GetAimDirection()
		{
            if (_inputs.Player.AimDirection.activeControl == null) return Vector2.right;
            else if (_inputs.Player.AimDirection.activeControl.device is Gamepad)
            {
                    return Gather().AimDirection;
            }
            else if (_inputs.Player.AimDirection.activeControl.device is Mouse)
            {
            // 获取鼠标屏幕位置
            Vector3 mouseScreenPos = Gather().AimDirection;
            
            // 根据相机投影模式设置正确的z值
            if (Camera.main.orthographic)
            {
                // 正交模式下，z值不重要，可以使用0
                mouseScreenPos.z = 0;
            }
            else
            {
                // 透视模式下，需要设置z值为玩家在屏幕空间的深度
                mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
            }
            
            Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
                return (worldMousePos - (Vector2)transform.position).normalized;
            }
            else if (_inputs.Player.AimDirection.activeControl.device is Keyboard)
            {
                return Gather().AimDirection;
            }
            else return Vector2.right;
		}

        public struct FrameInput
        {
            public Vector2 Move;
            public bool JumpStart;
            public bool Grind;
            public bool SwitchItem;
            public bool Trick;
            public bool TrickStart;
            public bool Push;
            public bool ShootStart;
            public bool ShootEnd;
            public Vector2 AimDirection;
        }

    }
}