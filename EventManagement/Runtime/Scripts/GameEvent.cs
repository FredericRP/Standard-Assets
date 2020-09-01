using UnityEngine;
using UnityEngine.Events;

namespace FredericRP.EventManagement
{
  [CreateAssetMenu(menuName = "FredericRP/Game Event")]
  public class GameEvent : ScriptableObject
  {
    public virtual void Raise()
    {
      EventHandler.TriggerEvent(this);
    }

    public void Raise<T>(T value)
    {
      EventHandler.TriggerEvent<T>(this, value);
    }
    public void Raise<T, U>(T arg1, U arg2)
    {
      EventHandler.TriggerEvent<T, U>(this, arg1, arg2);
    }
    public void Raise<T, U, V>(T arg1, U arg2, V arg3)
    {
      EventHandler.TriggerEvent<T, U, V>(this, arg1, arg2, arg3);
    }

    public void Listen(UnityAction action)
    {
      EventHandler.AddEventListener(this, action);
    }
    public void Listen<T>(UnityAction<T> action)
    {
      EventHandler.AddEventListener<T>(this, action);
    }
    public void Listen<T, U>(UnityAction<T, U> action)
    {
      EventHandler.AddEventListener<T, U>(this, action);
    }
    public void Listen<T, U, V>(UnityAction<T, U, V> action)
    {
      EventHandler.AddEventListener<T, U, V>(this, action);
    }

    public void Delete(UnityAction action)
    {
      EventHandler.RemoveEventListener(this, action);
    }
    public void Delete<T>(UnityAction<T> action)
    {
      EventHandler.RemoveEventListener<T>(this, action);
    }
    public void Delete<T, U>(UnityAction<T, U> action)
    {
      EventHandler.RemoveEventListener<T, U>(this, action);
    }
    public void Delete<T, U, V>(UnityAction<T, U, V> action)
    {
      EventHandler.RemoveEventListener<T, U, V>(this, action);
    }
  }
}