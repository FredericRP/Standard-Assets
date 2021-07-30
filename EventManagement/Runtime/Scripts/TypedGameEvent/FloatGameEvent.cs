using UnityEngine;

namespace FredericRP.EventManagement
{
  [CreateAssetMenu(menuName = "FredericRP/Float Game Event")]
  public class FloatGameEvent : GameEvent
  {
    public float parameter;

    public override void Raise(GameEventHandler eventHandler = null)
    {
      if (eventHandler == null)
        GameEventHandler.TriggerEvent<float>(this, parameter);
      else
        eventHandler.TriggerInstanceEvent<float>(this, parameter);
    }
  }
}