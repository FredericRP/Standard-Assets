using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FredericRP.EventManagement
{
  [CreateAssetMenu(menuName = "FredericRP/Float Game Event")]
  public class FloatGameEvent : GameEvent
  {
    public float parameter;

    public override void Raise()
    {
      EventHandler.TriggerEvent<float>(this, parameter);
    }
  }
}