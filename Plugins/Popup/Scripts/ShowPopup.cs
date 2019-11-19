using UnityEngine;

namespace FredericRP.Popups
{
	public class ShowPopup : MonoBehaviour
	{
		[SerializeField]
		PopupDescriptor popup = null;

		public void DisplayPopup()
		{
			PopupHandler.ShowPopup(popup);
    }
	}
}
