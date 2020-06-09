using UnityEngine;

namespace FredericRP.Popups
{
	public class PopupBase : MonoBehaviour
	{
		[SerializeField]
		private Animator animator = null;
		[SerializeField]
		private Canvas canvas = null;

		private bool visible = false;

		private object parameters;
		public object Parameters
		{
			get { return parameters; }
			set { parameters = value; }
		}

		public bool IsVisible { get { return visible; } }
		public Canvas Canvas { get { return canvas; } }

		/// <summary>
		/// Show this popup
		/// </summary>
		public void Show()
		{
			if (visible)
				return;
			
			animator.SetBool("visible", true);

			visible = true;
		}

		/// <summary>
		/// Hide this popup
		/// </summary>
		public void Hide()
		{
			if (!visible)
				return;
			animator.SetBool("visible", false);

			visible = false;
		}

		/// <summary>
		/// Called when the popup is initialized
		/// </summary>
		public virtual void Init(object parameters) {
			this.parameters = parameters;
		}
	}
}