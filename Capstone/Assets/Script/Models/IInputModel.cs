using QFramework;
using UnityEngine;

namespace SkateGame
{
	public interface IInputModel : IModel
	{
		BindableProperty<Vector2> Move { get; }
		BindableProperty<bool> Jump { get; }
		BindableProperty<bool> Grind { get; } 
	}

	public class InputModel : AbstractModel, IInputModel
	{
		public BindableProperty<Vector2> Move { get; } = new BindableProperty<Vector2>(Vector2.zero);
		public BindableProperty<bool> Jump { get; } = new BindableProperty<bool>(false);
		public BindableProperty<bool> Grind { get; } = new BindableProperty<bool>(false);

		protected override void OnInit()
		{
		}
	}
}


