using QFramework;
using UnityEngine;

namespace SkateGame
{
	public interface IInputModel : IModel
	{
		BindableProperty<Vector2> Move { get; }
		BindableProperty<bool> JumpStart { get; }
		BindableProperty<bool> Grind { get; } 
		BindableProperty<bool> SwitchItem { get; }
		BindableProperty<bool> Trick { get; }
		BindableProperty<bool> TrickStart { get; }
	}

	public class InputModel : AbstractModel, IInputModel
	{
		public BindableProperty<Vector2> Move { get; } = new BindableProperty<Vector2>(Vector2.zero);
		public BindableProperty<bool> JumpStart { get; } = new BindableProperty<bool>(false);
		public BindableProperty<bool> Grind { get; } = new BindableProperty<bool>(false);
		public BindableProperty<bool> SwitchItem { get; } = new BindableProperty<bool>(false);
		public BindableProperty<bool> Trick { get; } = new BindableProperty<bool>(false);
		public BindableProperty<bool> TrickStart { get; } = new BindableProperty<bool>(false);
		protected override void OnInit()
		{
		}
	}
}


