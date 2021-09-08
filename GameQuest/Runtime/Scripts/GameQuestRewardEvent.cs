using FredericRP.EventManagement;
using UnityEngine;

namespace FredericRP.GameQuest
{
  /// <summary>
  /// Simplify the call to generic methods of GameEvent
  /// </summary>
  [CreateAssetMenu(menuName = "FredericRP/Game Quest/Reward event")]
  public class GameQuestRewardEvent : GameEvent
  {
    public void Raise(GameQuestReward questReward, GameEventHandler eventHandler = null) =>
      Raise<GameQuestReward>(questReward, eventHandler);
  }
}