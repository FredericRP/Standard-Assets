using FredericRP.EventManagement;
using UnityEngine;
using UnityEngine.Events;

public class SimpleEventTrigger : MonoBehaviour
{
    [SerializeField]
    GameEvent gameEvent;
    [SerializeField]
    UnityEvent unityEvent;

    private void Start()
    {
        EventHandler.AddEventListener(gameEvent, unityEvent.Invoke);
    }
}
