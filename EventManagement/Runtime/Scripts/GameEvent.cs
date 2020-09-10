using UnityEngine;
using UnityEngine.Events;

namespace FredericRP.EventManagement
{
  [CreateAssetMenu(menuName = "FredericRP/Game Event")]
  public class GameEvent : ScriptableObject
  {
    public virtual void Raise()
    {
      GameEventHandler.TriggerEvent(this);
    }

    public void Raise<T>(T value)
    {
      GameEventHandler.TriggerEvent<T>(this, value);
    }
    public void Raise<T, U>(T arg1, U arg2)
    {
      GameEventHandler.TriggerEvent<T, U>(this, arg1, arg2);
    }
    public void Raise<T, U, V>(T arg1, U arg2, V arg3)
    {
      GameEventHandler.TriggerEvent<T, U, V>(this, arg1, arg2, arg3);
    }

    public void Listen(UnityAction action)
    {
      GameEventHandler.AddEventListener(this, action);
    }
    public void Listen<T>(UnityAction<T> action)
    {
      GameEventHandler.AddEventListener<T>(this, action);
    }
    public void Listen<T, U>(UnityAction<T, U> action)
    {
      GameEventHandler.AddEventListener<T, U>(this, action);
    }
    public void Listen<T, U, V>(UnityAction<T, U, V> action)
    {
      GameEventHandler.AddEventListener<T, U, V>(this, action);
    }

    public void Delete(UnityAction action)
    {
      GameEventHandler.RemoveEventListener(this, action);
    }
    public void Delete<T>(UnityAction<T> action)
    {
      GameEventHandler.RemoveEventListener<T>(this, action);
    }
    public void Delete<T, U>(UnityAction<T, U> action)
    {
      GameEventHandler.RemoveEventListener<T, U>(this, action);
    }
    public void Delete<T, U, V>(UnityAction<T, U, V> action)
    {
      GameEventHandler.RemoveEventListener<T, U, V>(this, action);
    }
  }
}