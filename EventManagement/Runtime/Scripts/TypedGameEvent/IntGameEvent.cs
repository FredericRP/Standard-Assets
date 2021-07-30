﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FredericRP.EventManagement
{
  [CreateAssetMenu(menuName = "FredericRP/Integer Game Event")]
  public class IntGameEvent : GameEvent
  {
    public int parameter;

    public override void Raise(GameEventHandler eventHandler = null)
    {
      if (eventHandler == null)
        GameEventHandler.TriggerEvent<int>(this, parameter);
      else
        eventHandler.TriggerInstanceEvent<int>(this, parameter);
    }
  }
}