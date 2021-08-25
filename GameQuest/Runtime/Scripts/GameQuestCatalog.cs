using System.Collections.Generic;
using UnityEngine;

namespace FredericRP.GameQuest
{
  [CreateAssetMenu(fileName = "GameQuestCatalog", menuName = "FredericRP/GameQuest/Create quest catalog")]
  public class GameQuestCatalog : ScriptableObject
  {
    [SerializeField]
    List<GameQuestInfo> gameQuestInfoList;

    /// <summary>
    /// Return the GameQuestInfo with the corresponding title by checking referenced GameQuestInfo in the gameQuestinfos array
    /// </summary>
    /// <param name="gameQuestTitle"></param>
    /// <returns></returns>
    public GameQuestInfo GetQuest(string gameQuestID)
    {
      return gameQuestInfoList.Find(quest => quest.gameQuestID.Equals(gameQuestID));
    }
/*
    public int GetTotalGameQuestDuration()
    {
      int toTalGameQuestDuration = 0;

      for (int i = 0; i < gameQuestInfoList.Count; i++)
      {
        toTalGameQuestDuration += gameQuestInfoList[i].totalDuration;
      }

      return toTalGameQuestDuration;
    }
    // */

    System.DateTime cachedDate;
    List<GameQuestInfo> cachedSelectedQuestList;

    public GameQuestInfo GetRandomAvailableQuest(bool forCurrentDate = true)
    {
      List<GameQuestInfo> selectedQuestList = gameQuestInfoList;
      System.DateTime date = System.DateTime.Now;
      if (cachedDate.Equals(date))
        selectedQuestList = cachedSelectedQuestList;
      else if (forCurrentDate) {
        selectedQuestList = gameQuestInfoList.FindAll(quest => (quest.dayInMonth == -1 || quest.dayInMonth == date.Day) && (quest.month == -1 || quest.month == date.Month) && (quest.year == -1 || quest.year == date.Year));
        // Save filtered list for future reference
        cachedSelectedQuestList = selectedQuestList;
        cachedDate = date;
      }

      int rand = Random.Range(0, selectedQuestList.Count);

      return selectedQuestList[rand];
    }

  }
}
