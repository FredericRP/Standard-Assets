using UnityEngine;

namespace FredericRP.Popups
{
	[CreateAssetMenu(menuName ="FredericRP/Popup Descriptor")]
	public class PopupDescriptor : ScriptableObject
	{
		public enum PopupSource { Pool, Prefab};
		public PopupSource popupSource;
		public string pooledObjectName;
		public PopupBase prefab;
	}
}