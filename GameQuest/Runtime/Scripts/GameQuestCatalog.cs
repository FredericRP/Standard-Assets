using System.Collections.Generic;
using UnityEngine;

namespace FredericRP.GameQuest
{
  [CreateAssetMenu(fileName = "GameQuestCatalog", menuName = "FredericRP/Game Quest/Catalog")]
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

    [System.NonSerialized]
    private System.DateTime cachedDate;
    [System.NonSerialized]
    private List<GameQuestInfo> cachedSelectedQuestList = null;

    private void Awake()
    {
      cachedSelectedQuestList = null;
    }

    public GameQuestInfo GetRandomAvailableQuest()
    {
      UpdateTodayQuestList();
      int rand = Random.Range(0, cachedSelectedQuestList.Count);

      return cachedSelectedQuestList[rand];
    }

    public void UpdateTodayQuestList(bool forceUpdate = false)
    {
      Debug.Log("Update Today quest count force=" + forceUpdate);

      List<GameQuestInfo> selectedQuestList = gameQuestInfoList;
      System.DateTime date = System.DateTime.Now;
      if (cachedSelectedQuestList != null && cachedDate.Equals(date) && !forceUpdate)
        selectedQuestList = cachedSelectedQuestList;
      else
      {
        Debug.Log("Search for quest for date: " + date.Day + " / " + date.Month + " / " + date.Year);
        selectedQuestList = gameQuestInfoList.FindAll(quest => (quest.dayInMonth == 0 || quest.dayInMonth == date.Day) && (quest.month == 0 || quest.month == date.Month) && (quest.year == 0 || quest.year == date.Year));
        //selectedQuestList = gameQuestInfoList.FindAll(quest => (quest.dayInMonth == 0 || quest.dayInMonth == date.Day));
        // Save filtered list for future reference
        cachedSelectedQuestList = selectedQuestList;
        cachedDate = date;
      }
    }

    public int TodayQuestCount()
    {
      Debug.Log("Today quest count from list " + cachedSelectedQuestList);
      if (cachedSelectedQuestList == null)
        UpdateTodayQuestList();
      Debug.Log("Available quest " + cachedSelectedQuestList + " count:" + cachedSelectedQuestList?.Count);
      if (cachedSelectedQuestList == null)
        return 0;
      return cachedSelectedQuestList.Count;
    }

    public GameQuestInfo GetAvailableQuest(int index)
    {
      if (cachedSelectedQuestList == null)
        UpdateTodayQuestList();
      if (cachedSelectedQuestList == null || index >= cachedSelectedQuestList.Count || index < 0)
        return null;
      return cachedSelectedQuestList[index];
    }

  }
}
