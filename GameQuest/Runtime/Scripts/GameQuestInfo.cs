using FredericRP.PlayerCurrency;
using FredericRP.StringDataList;
using System.Collections.Generic;
using UnityEngine;

namespace FredericRP.GameQuest
{
  [System.Serializable]
  public class GameQuestInfo
  {
    public string gameQuestID;
    public string localizationId = "";

    /// <summary>
    /// Is this quest enabled only on a specific month? (-1 otherwise)
    /// </summary>
    public int month = -1;
    /// <summary>
    /// Is this quest enabled only a specific day in the month? (-1 otherwise)
    /// </summary>
    public int dayInMonth = -1;
    /// <summary>
    /// Is this quest enabled only a specific year? (-1 otherwise)
    /// </summary>
    public int year = -1;

    /// <summary>
    /// Duration in minutes of this quest when activated
    /// </summary>
    public int duration = 0;

    /// <summary>
    /// Target amount to reach
    /// </summary>
    public int target = 5;

    /// <summary>
    /// What does the target represents? Link that to your favorite Enum or own IDs in game
    /// </summary>
    [Select(PlayerCurrencyData.CurrencyList)]
    public int targetId;
       
    public List<GameQuestReward> gameQuestRewardList;

    public static System.TimeSpan RemainingTime(GameQuestInfo questInfo, GameQuestSavedData.QuestProgress questProgress)
    {
      System.TimeSpan time = questProgress.LaunchDate.AddSeconds(questInfo.duration).Subtract(System.DateTime.Now);

      if (time.TotalSeconds > 0)
        return time;

      return new System.TimeSpan(0);
    }

    //public abstract void OnQuestLaunched(GameQuestSavedData.Info info);
    //public abstract bool IsUnlocked();
    //public abstract bool IsCompleted(GameQuestSavedData.Info info);
    /*public virtual void Complete(bool directObtainReward)
    {
      if (directObtainReward)
        GameQuestManager.Instance.GetGameQuestReward(gameQuestReward);
    }
    // */
    //public abstract void Fail();

    //public abstract float CompletionRate(GameQuestSavedData.Info info);
  }
}
