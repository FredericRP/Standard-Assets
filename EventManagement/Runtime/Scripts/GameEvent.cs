using UnityEngine;

namespace FredericRP.EventManagement
{
    [CreateAssetMenu(menuName ="FredericRP/Game Event")]
    public class GameEvent : ScriptableObject
    {
        public virtual void Raise()
        {
            EventHandler.TriggerEvent(this);
        }
    }
}