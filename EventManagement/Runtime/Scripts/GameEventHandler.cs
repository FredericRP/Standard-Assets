using FredericRP.GenericSingleton;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace FredericRP.EventManagement
{
  public class GameEventHandler : Singleton<GameEventHandler>
  {
    private Dictionary<GameEvent, Delegate> gameEvents = new Dictionary<GameEvent, Delegate>();

    internal static void AddEventListener(GameEvent gameEvent, UnityAction handler)
    {
      Instance.AddInstanceEventListener(gameEvent, handler);
    }
    internal void AddInstanceEventListener(GameEvent gameEvent, UnityAction handler)
    {
      if (!gameEvents.ContainsKey(gameEvent))
      {
        gameEvents.Add(gameEvent, null);
      }

      gameEvents[gameEvent] = (UnityAction)gameEvents[gameEvent] + handler;
    }

    internal static void AddEventListener<T>(GameEvent gameEvent, UnityAction<T> handler)
    {
      Instance.AddInstanceEventListener<T>(gameEvent, handler);
    }
    internal void AddInstanceEventListener<T>(GameEvent gameEvent, UnityAction<T> handler)
    {
      if (!gameEvents.ContainsKey(gameEvent))
      {
        gameEvents.Add(gameEvent, null);
      }

      gameEvents[gameEvent] = (UnityAction<T>)gameEvents[gameEvent] + handler;
    }

    internal static void AddEventListener<T, U>(GameEvent gameEvent, UnityAction<T, U> handler)
    {
      Instance.AddInstanceEventListener<T, U>(gameEvent, handler);
    }
    internal void AddInstanceEventListener<T, U>(GameEvent gameEvent, UnityAction<T, U> handler)
    {
      if (!gameEvents.ContainsKey(gameEvent))
      {
        gameEvents.Add(gameEvent, null);
      }

      gameEvents[gameEvent] = (UnityAction<T, U>)gameEvents[gameEvent] + handler;
    }

    internal static void AddEventListener<T, U, V>(GameEvent gameEvent, UnityAction<T, U, V> handler)
    {
      Instance.AddInstanceEventListener<T, U, V>(gameEvent, handler);
    }
    internal void AddInstanceEventListener<T, U, V>(GameEvent gameEvent, UnityAction<T, U, V> handler)
    {
      if (!gameEvents.ContainsKey(gameEvent))
      {
        gameEvents.Add(gameEvent, null);
      }

      gameEvents[gameEvent] = (UnityAction<T, U, V>)gameEvents[gameEvent] + handler;
    }

    internal static void RemoveEventListener(GameEvent gameEvent, UnityAction handler)
    {
      Instance?.RemoveInstanceEventListener(gameEvent, handler);
    }
    internal void RemoveInstanceEventListener(GameEvent gameEvent, UnityAction handler)
    {
      Delegate eventDelegate;

      if (gameEvents.TryGetValue(gameEvent, out eventDelegate))
      {
        gameEvents[gameEvent] = (UnityAction)gameEvents[gameEvent] - handler;
      }
    }

    internal static void RemoveEventListener<T>(GameEvent gameEvent, UnityAction<T> handler)
    {
      Instance?.RemoveInstanceEventListener<T>(gameEvent, handler);
    }
    internal void RemoveInstanceEventListener<T>(GameEvent gameEvent, UnityAction<T> handler)
    {
      Delegate eventDelegate;

      if (gameEvents.TryGetValue(gameEvent, out eventDelegate))
      {
        gameEvents[gameEvent] = (UnityAction<T>)gameEvents[gameEvent] - handler;
      }
    }

    internal static void RemoveEventListener<T, U>(GameEvent gameEvent, UnityAction<T, U> handler)
    {
      Instance?.RemoveInstanceEventListener<T, U>(gameEvent, handler);
    }
    internal void RemoveInstanceEventListener<T, U>(GameEvent gameEvent, UnityAction<T, U> handler)
    {
      Delegate eventDelegate;

      if (gameEvents.TryGetValue(gameEvent, out eventDelegate))
      {
        gameEvents[gameEvent] = (UnityAction<T, U>)gameEvents[gameEvent] - handler;
      }
    }
    internal static void RemoveEventListener<T, U, V>(GameEvent gameEvent, UnityAction<T, U, V> handler)
    {
      Instance?.RemoveInstanceEventListener<T, U, V>(gameEvent, handler);
    }
    internal void RemoveInstanceEventListener<T, U, V>(GameEvent gameEvent, UnityAction<T, U, V> handler)
    {
      Delegate eventDelegate;

      if (gameEvents.TryGetValue(gameEvent, out eventDelegate))
      {
        gameEvents[gameEvent] = (UnityAction<T, U, V>)gameEvents[gameEvent] - handler;
      }
    }

    internal static bool TriggerEvent(GameEvent gameEvent)
    {
      return Instance.TriggerInstanceEvent(gameEvent);
    }
    internal bool TriggerInstanceEvent(GameEvent gameEvent)
    {
      Delegate eventDelegate;

      if (gameEvents.TryGetValue(gameEvent, out eventDelegate))
      {
        UnityAction callback = eventDelegate as UnityAction;

        if (callback != null)
        {
          callback();

          return true;
        }
      }

      return false;
    }

    internal static bool TriggerEvent<T>(GameEvent gameEvent, T value)
    {
      return Instance.TriggerInstanceEvent<T>(gameEvent, value);
    }
    internal bool TriggerInstanceEvent<T>(GameEvent gameEvent, T value)
    {
      Delegate eventDelegate;

      if (gameEvents.TryGetValue(gameEvent, out eventDelegate))
      {
        UnityAction<T> callback = eventDelegate as UnityAction<T>;

        if (callback != null)
        {
          callback(value);
          return true;
        }
        else if (eventDelegate != null)
        {
          UnityEngine.Debug.LogWarning("Try to trigger an event with the wrong generic pattern: " + gameEvent);
        }
      }

      return false;
    }

    internal static bool TriggerEvent<T, U>(GameEvent gameEvent, T value, U secondValue)
    {
      return Instance.TriggerInstanceEvent<T, U>(gameEvent, value, secondValue);
    }
    internal bool TriggerInstanceEvent<T, U>(GameEvent gameEvent, T value, U secondValue)
    {
      Delegate eventDelegate;

      if (gameEvents.TryGetValue(gameEvent, out eventDelegate))
      {
        UnityAction<T, U> callback = eventDelegate as UnityAction<T, U>;

        if (callback != null)
        {
          callback(value, secondValue);
          return true;
        }
        else if (eventDelegate != null)
        {
          UnityEngine.Debug.LogWarning("Try to trigger an event with the wrong generic (T, U) pattern: " + gameEvent);
        }
      }

      return false;
    }

    internal static bool TriggerEvent<T, U, V>(GameEvent gameEvent, T value, U secondValue, V thirdValue)
    {
      return Instance.TriggerInstanceEvent<T, U, V>(gameEvent, value, secondValue, thirdValue);
    }
    internal bool TriggerInstanceEvent<T, U, V>(GameEvent gameEvent, T value, U secondValue, V thirdValue)
    {
      Delegate eventDelegate;

      if (gameEvents.TryGetValue(gameEvent, out eventDelegate))
      {
        UnityAction<T, U, V> callback = eventDelegate as UnityAction<T, U, V>;

        if (callback != null)
        {
          callback(value, secondValue, thirdValue);
          return true;
        }
        else if (eventDelegate != null)
        {
          UnityEngine.Debug.LogWarning("Try to trigger an event with the wrong generic (T, U, V) pattern: " + gameEvent);
        }
      }

      return false;
    }
  }
}