using UnityEngine;
using FredericRP.PersistentData;
using FredericRP.GenericSingleton;
using FredericRP.EventManagement;
using System;

namespace FredericRP.GameQuest
{
  public class GameQuestManager : Singleton<GameQuestManager>
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

    /// <summary>
    /// Locked -> WaitingForEnable
    /// </summary>
    public GameQuestEvent onGameQuestUnlock;
    //WaitingForEnable -> InProgress
    public GameQuestEvent onGameQuestLaunch;
    /// <summary>
    /// InProgress -> WaitingForReward
    /// </summary>
    public GameQuestEvent onGameQuestValidate;
    /// <summary>
    /// WaitingForReward -> Complete
    /// </summary>
    public GameQuestEvent onGameQuestComplete;
    /// <summary>
    /// Quest invalidated
    /// </summary>
    public GameQuestEvent onGameQuestExpire;
    public GameQuestRewardEvent onRewardObtained;

    [SerializeField]
    GameQuestCatalog gameQuestArray;

    public bool obtainRewardDirectlyWhenQuestCompleted = false;

    public int numberOfQuestInHistoric = 5;

    /// <summary>
    /// Get a quest that is unlocked, useful to display the next available quest to the player
    /// </summary>
    /// <returns></returns>
    public GameQuestInfo GetWaitingForEnableQuest()
    {
      return GetQuestFromStatus(GameQuestSavedData.GameQuestStatus.WaitingForEnable);
    }

    public GameQuestInfo GetCompleteQuest()
    {
      return GetQuestFromStatus(GameQuestSavedData.GameQuestStatus.Complete);
    }
    public GameQuestInfo GetInProgressQuest()
    {
      return GetQuestFromStatus(GameQuestSavedData.GameQuestStatus.InProgress);
    }

    public GameQuestInfo GetQuestFromStatus(GameQuestSavedData.GameQuestStatus status)
    {
      GameQuestSavedData gameQuestSavedData = PersistentDataSystem.Instance.GetSavedData<GameQuestSavedData>();
      Debug.Log("Check GameQuestSavedData " + gameQuestSavedData + " count: " + gameQuestArray?.AvailableQuestCount());
      for (int questIndex = 0; questIndex < gameQuestArray.AvailableQuestCount(); questIndex++)
      {
        GameQuestInfo questInfo = gameQuestArray.GetAvailableQuest(questIndex);
        GameQuestSavedData.QuestProgress questProgress = gameQuestSavedData.GetQuestProgress(questInfo.gameQuestID);
        Debug.Log("Check questInfo " + questInfo + "\nWith progress " + questProgress);
        if (questProgress != null && questProgress.gameQuestStatus == status)
          return questInfo;
      }
      return null;
    }

    public GameQuestInfo GetGameQuest(string gameQuestId)
    {
      return gameQuestArray.GetQuest(gameQuestId);
    }


    public void LaunchQuest(GameQuestInfo questInfo, GameQuestSavedData.QuestProgress questProgress)
    {
      Debug.Log("Launch Quest " + questInfo.gameQuestID);
      questProgress.gameQuestStatus = GameQuestSavedData.GameQuestStatus.InProgress;
      GameQuestSavedData gameQuestSavedData = PersistentDataSystem.Instance.GetSavedData<GameQuestSavedData>();
      gameQuestSavedData.SetQuestProgress(questProgress);
      onGameQuestLaunch?.Raise(questInfo, questProgress);
    }

    /// <summary>
    /// Complete the GameQuest and call the gameQuestInfo.Complete() to perform quest actions
    /// </summary>
    /// <param name="gameQuestInfo"></param>
    public void CompleteGameQuest(GameQuestInfo questInfo, GameQuestSavedData.QuestProgress questProgress)
    {
      Debug.Log("Complete Quest " + questInfo.gameQuestID);
      if (obtainRewardDirectlyWhenQuestCompleted)
        questProgress.gameQuestStatus = GameQuestSavedData.GameQuestStatus.Complete;
      else
        questProgress.gameQuestStatus = GameQuestSavedData.GameQuestStatus.WaitingForReward;
      GameQuestSavedData gameQuestSavedData = PersistentDataSystem.Instance.GetSavedData<GameQuestSavedData>();
      gameQuestSavedData.SetQuestProgress(questProgress);
      onGameQuestComplete?.Raise(questInfo, questProgress);
    }

    /// <summary>
    /// Unvalide the GameQuest
    /// </summary>
    /// <param name="questProgress"></param>
    public void ExpireGameQuest(GameQuestSavedData.QuestProgress questProgress)
    {
      GameQuestInfo gameQuestInfo = gameQuestArray.GetQuest(questProgress.gameQuestId);

      GameQuestSavedData gameQuestSavedData = PersistentDataSystem.Instance.GetSavedData<GameQuestSavedData>();
      // when expired, a quest goes back to WaitingForEnable status
      questProgress.gameQuestStatus = GameQuestSavedData.GameQuestStatus.WaitingForEnable;

      gameQuestSavedData.SetQuestProgress(questProgress);

      onGameQuestExpire?.Raise(gameQuestInfo, questProgress);
    }

    public void GetGameQuestReward(GameQuestReward gameQuestReward)
    {
      gameQuestReward.GiveReward();

      onRewardObtained?.Raise<GameQuestReward>(gameQuestReward);
    }
  }
}
