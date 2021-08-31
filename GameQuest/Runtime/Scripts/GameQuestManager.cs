using UnityEngine;
using FredericRP.PersistentData;
using FredericRP.GenericSingleton;
using FredericRP.EventManagement;
using System;
using UnityEngine.Events;

namespace FredericRP.GameQuest
{
  public class GameQuestManager : Singleton<GameQuestManager>
  {
    /// <summary>
    /// Simplify the call to generic methods of GameEvent
    /// </summary>
    public class GameQuestEvent : GameEvent
    {
      public void Raise(GameQuestInfo info, GameQuestSavedData.QuestProgress questProgress, GameEventHandler eventHandler = null) =>
        Raise<GameQuestInfo, GameQuestSavedData.QuestProgress>(info, questProgress, eventHandler);
    }

    /// <summary>
    /// Simplify the call to generic methods of GameEvent
    /// </summary>
    public class GameQuestRewardEvent : GameEvent
    {
      public void Raise(GameQuestReward questReward, GameEventHandler eventHandler = null) =>
        Raise<GameQuestReward>(questReward, eventHandler);
    }
    public static GameQuestEvent onGameQuestLaunch;
    public static GameQuestEvent onGameQuestCompleted;
    public static GameQuestEvent onGameQuestExpired;
    public static GameQuestEvent onGameQuestActivated;
    public static GameQuestRewardEvent onRewardObtained;

    [SerializeField]
    GameQuestCatalog gameQuestArray;

    public bool obtainRewardDirectlyWhenQuestCompleted = false;

    public int numberOfQuestInHistoric = 5;

    void Awake()
    {
      /*
      GameQuestSavedData gameQuestSavedData = PersistentDataSystem.Instance.GetSavedData<GameQuestSavedData>();

      for (int i = gameQuestSavedData.unlockedGameQuestList.Count - 1; i >= 0; i--)
      {
        var info = gameQuestSavedData.unlockedGameQuestList[i];
        GameQuestInfo gameQuestInfo = gameQuestArray.GetQuest(info.gameQuestId);

        if (gameQuestInfo == null)
        {
          gameQuestSavedData.unlockedGameQuestList.Remove(info);
        }
        else
        {
          onGameQuestLaunch.Raise<GameQuestInfo>(gameQuestInfo);
          //gameQuestInfo.OnQuestLaunched(info);
        }
      }

      for (int i = gameQuestSavedData.completedGameQuestList.Count - 1; i >= 0; i--)
      {
        var info = gameQuestSavedData.completedGameQuestList[i];
        GameQuestInfo gameQuestInfo = gameQuestArray.GetQuest(info.gameQuestId);

        if (gameQuestInfo == null)
        {
          gameQuestSavedData.completedGameQuestList.Remove(info);
        }
      }
      // */
    }

    void Update()
    {
      GameQuestSavedData gameQuestSavedData = PersistentDataSystem.Instance.GetSavedData<GameQuestSavedData>();
      /*
      //Check if some GameQuest in all availableQuests are unlocked
      int totalGameQuestDuration = gameQuestArray.GetTotalGameQuestDuration();
      int repartitionTime = 0;
      int numberOfTurn = -1;

      if (totalGameQuestDuration > 0)
        numberOfTurn = (int)(((System.DateTime.Now.Subtract(gameQuestSavedData.firstGameLaunchDate).TotalSeconds)) / totalGameQuestDuration);

      foreach (GameQuestInfo gameQuestInfo in gameQuestArray.)
      {
        int deltaSeconds = (int)System.DateTime.Now.Subtract(gameQuestSavedData.firstGameLaunchDate).TotalSeconds % totalGameQuestDuration;

        if (deltaSeconds >= repartitionTime && deltaSeconds < repartitionTime + gameQuestInfo.totalDuration
            && gameQuestInfo.IsUnlocked()
            && !gameQuestSavedData.unlockedGameQuestList.Exists(x => { return x.launchMode == GameQuestLaunchMode.IMMEDIATE && x.gameQuestId == gameQuestInfo.gameQuestID; })
            && !gameQuestSavedData.completedGameQuestList.Exists(x => { return x.launchMode == GameQuestLaunchMode.IMMEDIATE && x.gameQuestId == gameQuestInfo.gameQuestID && x.launchDate == gameQuestSavedData.firstGameLaunchDate.AddSeconds(repartitionTime + numberOfTurn * totalGameQuestDuration); }))
        {
          AddAditionnalGameQuest(gameQuestInfo, gameQuestSavedData.firstGameLaunchDate.AddSeconds(numberOfTurn * totalGameQuestDuration + repartitionTime));
        }

        repartitionTime += gameQuestInfo.totalDuration;
      }

      //For current quest in the game, check if it can be completed and complete them if so.
      for (int i = gameQuestSavedData.unlockedGameQuestList.Count - 1; i >= 0; i--)
      {
        GameQuestSavedData.Info info = gameQuestSavedData.unlockedGameQuestList[i];

        GameQuestInfo gameQuestInfo = gameQuestArray.infos.FindQuestInfo(info.gameQuestId);

        if (gameQuestInfo == null) //Caused by a change in gameQuestID
        {
          gameQuestSavedData.unlockedGameQuestList.RemoveAt(i);
        }
        else
        {
          if (gameQuestInfo.IsCompleted(info))
          {
            CompleteGameQuest(info);
          }
          else if (System.DateTime.Now > info.launchDate.AddSeconds(gameQuestInfo.totalDuration))
          {
            ExpireGameQuest(info);
          }
        }
      }
      // */
    }

    /// <summary>
    /// Complete the GameQuest and call the gameQuestInfo.Complete() to perform quest actions
    /// </summary>
    /// <param name="gameQuestInfo"></param>
    public void CompleteGameQuest(GameQuestSavedData.QuestProgress questProgress)
    {
      GameQuestInfo gameQuestInfo = gameQuestArray.GetQuest(questProgress.gameQuestId);

      GameQuestSavedData gameQuestSavedData = PersistentDataSystem.Instance.GetSavedData<GameQuestSavedData>();

      if (obtainRewardDirectlyWhenQuestCompleted)
        questProgress.gameQuestStatus = GameQuestSavedData.GameQuestStatus.Complete;
      else
        questProgress.gameQuestStatus = GameQuestSavedData.GameQuestStatus.WaitingForReward;

      gameQuestSavedData.SetQuestProgress(questProgress);

      onGameQuestCompleted?.Raise<GameQuestInfo, GameQuestSavedData.QuestProgress>(gameQuestInfo, questProgress);
    }

    /// <summary>
    /// Unvalide the GameQuest
    /// </summary>
    /// <param name="questProgress"></param>
    public void ExpireGameQuest(GameQuestSavedData.QuestProgress questProgress)
    {
      GameQuestInfo gameQuestInfo = gameQuestArray.GetQuest(questProgress.gameQuestId);

      GameQuestSavedData gameQuestSavedData = PersistentDataSystem.Instance.GetSavedData<GameQuestSavedData>();
      GameQuestSavedData.QuestProgress info = questProgress;
      // when expired, a quest goes back to waitingForActivation status
      info.gameQuestStatus = GameQuestSavedData.GameQuestStatus.WaitingForEnable;

      gameQuestSavedData.SetQuestProgress(questProgress);

      onGameQuestExpired?.Raise<GameQuestInfo, GameQuestSavedData.QuestProgress>(gameQuestInfo, info);
    }

    /// <summary>
    /// To had some quest by code, use this function. 
    /// Will also register your quest in the persistentDataSystem.
    /// </summary>
    /// <param name="gameQuestSavedInfo"></param>
    public void AddAvailableGameQuest(GameQuestInfo gameQuestInfo)
    {
      GameQuestSavedData gameQuestSavedData = PersistentDataSystem.Instance.GetSavedData<GameQuestSavedData>();
      gameQuestSavedData.SetQuestProgress(new GameQuestSavedData.QuestProgress(gameQuestInfo.gameQuestID, System.DateTime.Now, GameQuestSavedData.LaunchMode.Immediate, GameQuestSavedData.GameQuestStatus.Enabled));
    }

    public void GetGameQuestReward(GameQuestReward gameQuestReward)
    {
      gameQuestReward.GiveReward();

      onRewardObtained?.Raise<GameQuestReward>(gameQuestReward);
    }
  }
}
