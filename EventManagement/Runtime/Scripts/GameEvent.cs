using UnityEngine;
using UnityEngine.Events;

namespace FredericRP.EventManagement
{
  [CreateAssetMenu(menuName = "FredericRP/Game Event")]
  public class GameEvent : ScriptableObject
  {
    public virtual void Raise(GameEventHandler eventHandler = null)
    {
      if (eventHandler == null)
        GameEventHandler.TriggerEvent(this);
      else
        eventHandler.TriggerInstanceEvent(this);
    }

    public void Raise<T>(T value, GameEventHandler eventHandler = null)
    {
      if (eventHandler == null)
        GameEventHandler.TriggerEvent<T>(this, value);
      else
        eventHandler.TriggerInstanceEvent<T>(this, value);
    }
    public void Raise<T, U>(T arg1, U arg2, GameEventHandler eventHandler = null)
    {
      if (eventHandler == null)
        GameEventHandler.TriggerEvent<T, U>(this, arg1, arg2);
      else
        eventHandler.TriggerInstanceEvent<T, U>(this, arg1, arg2);
    }
    public void Raise<T, U, V>(T arg1, U arg2, V arg3, GameEventHandler eventHandler = null)
    {
      if (eventHandler == null)
        GameEventHandler.TriggerEvent<T, U, V>(this, arg1, arg2, arg3);
      else
        eventHandler.TriggerInstanceEvent<T, U, V>(this, arg1, arg2, arg3);
    }

    public void Listen(UnityAction action, GameEventHandler eventHandler = null)
    {
      GameEventHandler.AddEventListener(this, action);
    }
    public void Listen<T>(UnityAction<T> action, GameEventHandler eventHandler = null)
    {
      GameEventHandler.AddEventListener<T>(this, action);
    }
    public void Listen<T, U>(UnityAction<T, U> action, GameEventHandler eventHandler = null)
    {
      GameEventHandler.AddEventListener<T, U>(this, action);
    }
    public void Listen<T, U, V>(UnityAction<T, U, V> action, GameEventHandler eventHandler = null)
    {
      GameEventHandler.AddEventListener<T, U, V>(this, action);
    }

    public void Delete(UnityAction action, GameEventHandler eventHandler = null)
    {
      GameEventHandler.RemoveEventListener(this, action);
    }
    public void Delete<T>(UnityAction<T> action, GameEventHandler eventHandler = null)
    {
      GameEventHandler.RemoveEventListener<T>(this, action);
    }
    public void Delete<T, U>(UnityAction<T, U> action, GameEventHandler eventHandler = null)
    {
      GameEventHandler.RemoveEventListener<T, U>(this, action);
    }
    public void Delete<T, U, V>(UnityAction<T, U, V> action, GameEventHandler eventHandler = null)
    {
      GameEventHandler.RemoveEventListener<T, U, V>(this, action);
    }
  }
}