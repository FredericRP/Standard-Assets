using UnityEngine;

namespace FredericRP.Popups
{
	public class ShowPopup : MonoBehaviour
	{
		[SerializeField]
		protected PopupDescriptor popup = null;

		public virtual void DisplayPopup()
		{
			PopupHandler.ShowPopup(popup);
    }
	}
}
