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
    /// A quest can be locked, waiting for activation (unlocked but user has to activate it), activated (but not necessarily) launched, in progress, ended and waiting for the user to get its reward,
    /// or complete (reward has been dispatched)
    /// </summary>
    public enum GameQuestStatus { Locked, WaitingForEnable, Enabled, InProgress, WaitingForReward, Complete };

    [System.Serializable]
    public class QuestProgress
    {
      public string gameQuestId;
      protected string launchDate;
      public LaunchMode launchMode;
      public GameQuestStatus gameQuestStatus;
      public int currentProgress;

      [System.NonSerialized]
      const string dateTimeFormat = "dd/MM/yyyy HH:mm";
      [System.NonSerialized]
      CultureInfo dateTimeProvider = CultureInfo.InvariantCulture;
      public System.DateTime LaunchDate { get { return String.IsNullOrEmpty(launchDate) ? System.DateTime.Now : System.DateTime.ParseExact(launchDate, dateTimeFormat, dateTimeProvider ); } set { launchDate = value.ToString(dateTimeFormat); } }

      public QuestProgress(string _gameQuestID, System.DateTime _launchDate, LaunchMode _gameQuestLaunchMode, GameQuestStatus _gameQuestStatus)
      {
        gameQuestId = _gameQuestID;
        LaunchDate = _launchDate;
        launchMode = _gameQuestLaunchMode;
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

    public override void onDataCreated(string dataVersion)
    {
      base.onDataCreated(dataVersion);
#if UNITY_EDITOR
      Debug.Log("New data created");
#endif
      dataName = "GameQuestSavedData";
    }

    QuestProgress GetQuestProgress(string gameQuestId, bool includeComplete = true)
    {
      return questProgressList.Find(quest => quest.gameQuestId == gameQuestId && quest.gameQuestStatus != GameQuestStatus.Locked && (includeComplete ? true : quest.gameQuestStatus != GameQuestStatus.Complete));
    }

    public void SetQuestProgress(QuestProgress questProgress)
    {
      if (!questProgressList.Contains(questProgress))
        questProgressList.Add(questProgress);
    }
  }
}
