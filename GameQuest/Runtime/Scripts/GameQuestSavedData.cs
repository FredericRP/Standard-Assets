using System.Collections.Generic;
using UnityEngine;
using FredericRP.PersistentData;
using System.Globalization;
using System;

namespace FredericRP.GameQuest
{
  [System.Serializable]
  public class GameQuestSavedData : SavedData
  {
    public enum LaunchMode { Immediate, OnUserAction };

    /// <summary>
    /// A quest can be locked, waiting for activation (unlocked but user has to activate it), in progress, ended and waiting for the user to get its reward,
    /// or complete (reward has been dispatched)
    /// </summary>
    public enum GameQuestStatus { Locked, WaitingForEnable, InProgress, WaitingForReward, Complete };

    [System.NonSerialized]
    const string dateTimeFormat = "dd/MM/yyyy HH:mm";
    [System.NonSerialized]
    static CultureInfo dateTimeProvider = CultureInfo.InvariantCulture;

    [System.Serializable]
    public class QuestProgress
    {
      public string gameQuestId;
      [SerializeField]
      protected string launchDate;
      /// <summary>
      /// Launch date of the quest. If it's in the future, it is not launched yet (!)
      /// </summary>
      public System.DateTime LaunchDate { get { return String.IsNullOrEmpty(launchDate) ? System.DateTime.Now.AddYears(1) : System.DateTime.ParseExact(launchDate, dateTimeFormat, dateTimeProvider ); } set { launchDate = value.ToString(dateTimeFormat); } }
      public LaunchMode launchMode;
      public GameQuestStatus gameQuestStatus = GameQuestStatus.Locked;
      public int currentProgress;

      public QuestProgress(string _gameQuestID, System.DateTime _launchDate, LaunchMode _gameQuestLaunchMode, GameQuestStatus _gameQuestStatus)
      {
        gameQuestId = _gameQuestID;
        LaunchDate = _launchDate;
        launchMode = _gameQuestLaunchMode;
        gameQuestStatus = _gameQuestStatus;
      }

      public QuestProgress(string _gameQuestID, GameQuestStatus _gameQuestStatus)
      {
        gameQuestId = _gameQuestID;
        launchMode = LaunchMode.Immediate;
        gameQuestStatus = _gameQuestStatus;
      }

      /// <summary>
      /// Check if find correspondance between all member variable except the status
      /// </summary>
      /// <param name="questProgress"></param>
      /// <returns></returns>
      public bool SimilarTo(QuestProgress questProgress)
      {
        if (questProgress.gameQuestId == gameQuestId && questProgress.launchDate.Equals(launchDate) && questProgress.launchMode == launchMode)
        {
          return true;
        }

        return false;
      }
    }

    /// <summary>
    /// Reference title of quest that the player has unlocked (used as key to find the quest), completed or activated
    /// </summary>
    [SerializeField]
    List<QuestProgress> questProgressList = new List<QuestProgress>();
    [SerializeField]
    protected string lastCheckDate;
    public System.DateTime LastCheckedDate { get { return String.IsNullOrEmpty(lastCheckDate) ? System.DateTime.Now : System.DateTime.ParseExact(lastCheckDate, dateTimeFormat, dateTimeProvider); } set { lastCheckDate = value.ToString(dateTimeFormat); } }
    public override void onDataCreated(string dataVersion)
    {
      base.onDataCreated(dataVersion);
#if UNITY_EDITOR
      Debug.Log("New data created");
#endif
      dataName = "GameQuestSavedData";
    }

    /// <summary>
    /// Get the saved quest progress<br/>
    /// Can filter on not complete nor locked quests, and create right away a quest on the locked status
    /// </summary>
    /// <param name="gameQuestId"></param>
    /// <param name="includeComplete"></param>
    /// <param name="includeLocked"></param>
    /// <param name="createIfNull"></param>
    /// <returns></returns>
    public QuestProgress GetQuestProgress(string gameQuestId, bool includeComplete = true, bool includeLocked = true, bool createIfNull = true)
    {
      QuestProgress questProgress = questProgressList.Find(quest => quest.gameQuestId == gameQuestId && (includeLocked ? true : quest.gameQuestStatus != GameQuestStatus.Locked) && (includeComplete ? true : quest.gameQuestStatus != GameQuestStatus.Complete));
      if (questProgress == null && createIfNull)
      {
        questProgress = new QuestProgress(gameQuestId, GameQuestStatus.Locked);
        SetQuestProgress(questProgress);
      }
      return questProgress;
    }

    public void SetQuestProgress(QuestProgress questProgress)
    {
      if (!questProgressList.Contains(questProgress))
        questProgressList.Add(questProgress);
    }
  }
}
