using UnityEngine;

namespace FredericRP.GameQuest
{
  [System.Serializable]
  public abstract class GameQuestReward : ScriptableObject
  {
    /// <summary>
    /// Abstraction layer to give a quest reward to a player
    /// </summary>
    /// <param name="playerId"></param>
    public abstract void GiveReward(int playerId = 0);
  }
}
