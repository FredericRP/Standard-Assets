using FredericRP.EventManagement;
using UnityEngine;

namespace FredericRP.GameQuest
{
  /// <summary>
  /// Simplify the call to generic methods of GameEvent
  /// </summary>
  [CreateAssetMenu(menuName = "FredericRP/Game Quest/Quest event")]
  public class GameQuestEvent : GameEvent
  {
    public void Raise(GameQuestInfo info, GameQuestSavedData.QuestProgress questProgress, GameEventHandler eventHandler = null) =>
      Raise<GameQuestInfo, GameQuestSavedData.QuestProgress>(info, questProgress, eventHandler);
  }
}