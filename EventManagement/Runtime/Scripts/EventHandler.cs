using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace FredericRP.EventManagement
{
  public class EventHandler
  {
    private static Dictionary<GameEvent, Delegate> gameEvents = new Dictionary<GameEvent, Delegate>();

    public static void AddEventListener(GameEvent gameEvent, UnityAction value)
    {
      if (!gameEvents.ContainsKey(gameEvent))
      {
        gameEvents.Add(gameEvent, null);
      }

      gameEvents[gameEvent] = (UnityAction)gameEvents[gameEvent] + value;
    }

    public static void AddEventListener<T>(GameEvent gameEvent, UnityAction<T> handler)
    {
      if (!gameEvents.ContainsKey(gameEvent))
      {
        gameEvents.Add(gameEvent, null);
      }

      gameEvents[gameEvent] = (UnityAction<T>)gameEvents[gameEvent] + handler;
    }

    public static void AddEventListener<T, U>(GameEvent gameEvent, UnityAction<T, U> handler)
    {
      if (!gameEvents.ContainsKey(gameEvent))
      {
        gameEvents.Add(gameEvent, null);
      }

      gameEvents[gameEvent] = (UnityAction<T, U>)gameEvents[gameEvent] + handler;
    }

    public static void RemoveEventListener(GameEvent gameEvent, UnityAction handler)
    {
      Delegate eventDelegate;

      if (gameEvents.TryGetValue(gameEvent, out eventDelegate))
      {
        gameEvents[gameEvent] = (UnityAction)gameEvents[gameEvent] - handler;
      }
    }

    public static void RemoveEventListener<T>(GameEvent gameEvent, UnityAction<T> handler)
    {
      Delegate eventDelegate;

      if (gameEvents.TryGetValue(gameEvent, out eventDelegate))
      {
        gameEvents[gameEvent] = (UnityAction<T>)gameEvents[gameEvent] - handler;
      }
    }

    public static void RemoveEventListener<T, U>(GameEvent gameEvent, UnityAction<T, U> handler)
    {
      Delegate eventDelegate;

      if (gameEvents.TryGetValue(gameEvent, out eventDelegate))
      {
        gameEvents[gameEvent] = (UnityAction<T, U>)gameEvents[gameEvent] - handler;
      }
    }

    public static bool TriggerEvent(GameEvent gameEvent)
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

    public static bool TriggerEvent<T>(GameEvent gameEvent, T value)
    {
      Delegate eventDelegate;

      if (gameEvents.TryGetValue(gameEvent, out eventDelegate))
      {
        UnityAction<T> callback = eventDelegate as UnityAction<T>;

        if (callback != null)
        {
          callback(value);
          return true;
        } else if (eventDelegate != null)
        {
          UnityEngine.Debug.LogWarning("Try to trigger an event with the wrong generic pattern: " + gameEvent);
        }
      }

      return false;
    }

    public static bool TriggerEvent<T, U>(GameEvent gameEvent, T value, U secondValue)
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
          UnityEngine.Debug.LogWarning("Try to trigger an event with the wrong generic pattern: " + gameEvent);
        }
      }

      return false;
    }
  }
}