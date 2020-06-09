using UnityEngine;
using UnityEngine.EventSystems;

namespace FredericRP.Popups
{
	public class ClosePopup : MonoBehaviour, IPointerDownHandler
	{
		public void OnPointerDown(PointerEventData data)
		{
			PopupHandler.ClosePopup();
		}
	}
}
