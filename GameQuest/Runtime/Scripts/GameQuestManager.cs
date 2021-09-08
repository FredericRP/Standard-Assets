using FredericRP.EventManagement;
using FredericRP.GenericSingleton;
using FredericRP.PersistentData;
using System;
using UnityEngine;

namespace FredericRP.GameQuest
{
  public class GameQuestManager : Singleton<GameQuestManager>
  {
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
    /// InProgress/WaitingForReward -> Complete
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

    public int GetWaitingForRewardCount()
    {
      // TODO: optimize data to be able to link between progress and quests easily
      GameQuestSavedData gameQuestSavedData = PersistentDataSystem.Instance.GetSavedData<GameQuestSavedData>();
      int count = 0;
      for (int questIndex = 0; questIndex < gameQuestArray.TodayQuestCount(); questIndex++)
      {
        GameQuestInfo questInfo = gameQuestArray.GetAvailableQuest(questIndex);
        GameQuestSavedData.QuestProgress questProgress = gameQuestSavedData.GetQuestProgress(questInfo.gameQuestID);
        if (questProgress != null && questProgress.gameQuestStatus == GameQuestSavedData.GameQuestStatus.WaitingForReward)
          count++;
      }
      return count;
    }

    public GameQuestInfo GetQuestFromStatus(GameQuestSavedData.GameQuestStatus status)
    {
      GameQuestSavedData gameQuestSavedData = PersistentDataSystem.Instance.GetSavedData<GameQuestSavedData>();
      for (int questIndex = 0; questIndex < gameQuestArray.TodayQuestCount(); questIndex++)
      {
        GameQuestInfo questInfo = gameQuestArray.GetAvailableQuest(questIndex);
        GameQuestSavedData.QuestProgress questProgress = gameQuestSavedData.GetQuestProgress(questInfo.gameQuestID);
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
      GameQuestSavedData gameQuestSavedData = PersistentDataSystem.Instance.GetSavedData<GameQuestSavedData>();
      gameQuestSavedData.SetQuestProgress(questProgress);

      if (obtainRewardDirectlyWhenQuestCompleted)
      {
        GiveGameQuestReward(questInfo, questProgress);
      }
      else
      {
        questProgress.gameQuestStatus = GameQuestSavedData.GameQuestStatus.WaitingForReward;
        onGameQuestValidate?.Raise(questInfo, questProgress);
      }
    }

    /// <summary>
    /// Unvalide the GameQuest
    /// </summary>
    /// <param name="questProgress"></param>
    public void ExpireGameQuest(GameQuestInfo questInfo, GameQuestSavedData.QuestProgress questProgress)
    {
      GameQuestSavedData gameQuestSavedData = PersistentDataSystem.Instance.GetSavedData<GameQuestSavedData>();
      // when expired, a quest goes back to WaitingForEnable status
      questProgress.gameQuestStatus = GameQuestSavedData.GameQuestStatus.WaitingForEnable;

      gameQuestSavedData.SetQuestProgress(questProgress);

      onGameQuestExpire?.Raise(questInfo, questProgress);
    }

    public void GiveGameQuestReward(GameQuestInfo questInfo, GameQuestSavedData.QuestProgress questProgress)
    {
      questProgress.gameQuestStatus = GameQuestSavedData.GameQuestStatus.Complete;
      onGameQuestComplete?.Raise(questInfo, questProgress);

      GameQuestSavedData gameQuestSavedData = PersistentDataSystem.Instance.GetSavedData<GameQuestSavedData>();
      gameQuestSavedData.SetQuestProgress(questProgress);

      for (int i = 0; i < questInfo.gameQuestRewardList.Count; i++)
      {
        questInfo.gameQuestRewardList[i].GiveReward();
        onRewardObtained?.Raise<GameQuestReward>(questInfo.gameQuestRewardList[i]);
      }
    }
  }
}
