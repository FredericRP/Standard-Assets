using UnityEngine;

namespace FredericRP.EventManagement
{
  public abstract class GenericReference<T> : ScriptableObject
  {
    public T value;
  }
}