using FredericRP.EventManagement;
using FredericRP.PersistentData;
using System.Collections.Generic;
using UnityEngine;

namespace FredericRP.GameQuest
{
  public class FlowerGrabQuestReceiver : MonoBehaviour
  {
    [SerializeField]
    GameEvent flowerGrabEvent;
    [SerializeField]
    GameQuestEvent questLaunched;
    [SerializeField]
    GameQuestEvent questValidate;

    [System.Serializable]
    public class FlowerGrabStatus
    {
      public string gameQuestId;
      //[HideInInspector]
      public int totalAmount = 0;
      //[HideInInspector]
      public GameQuestInfo questInfo = null;
      //[HideInInspector]
      public GameQuestSavedData.QuestProgress questProgress = null;
    }

    [SerializeField]
    [Tooltip("Game quest to follow")]
    List<FlowerGrabStatus> flowerGrabbedList;

    private void Start()
    {
      // Initialise quest data from quest manager and persistent data system
      GameQuestSavedData questSavedData = PersistentDataSystem.Instance.GetSavedData<GameQuestSavedData>();
      for (int i = 0; i < flowerGrabbedList.Count; i++)
      {
        GameQuestInfo questInfo = GameQuestManager.Instance.GetGameQuest(flowerGrabbedList[i].gameQuestId);
        flowerGrabbedList[i].questInfo = questInfo;
        flowerGrabbedList[i].questProgress = questSavedData.GetQuestProgress(questInfo.gameQuestID);
      }
    }

    private void OnEnable()
    {
      flowerGrabEvent.Listen<int>(FlowerGrabbed);
      questLaunched.Listen<GameQuestInfo, GameQuestSavedData.QuestProgress>(QuestLaunched);
    }
    private void OnDisable()
    {
      flowerGrabEvent.Delete<int>(FlowerGrabbed);
      questLaunched.Delete<GameQuestInfo, GameQuestSavedData.QuestProgress>(QuestLaunched);
    }


    private void QuestLaunched(GameQuestInfo questInfo, GameQuestSavedData.QuestProgress questProgress)
    {
      Debug.Log("Received quest launched " + questInfo.gameQuestID + " for target id " + questInfo.targetId);
      FlowerGrabStatus status = flowerGrabbedList.Find(status => status.gameQuestId == questInfo.gameQuestID);
      if (status != null)
      {
        Debug.Log("Received quest launched for flower id " + questInfo.targetId);
        status.questProgress.currentProgress = 0;
      }
    }

    void FlowerGrabbed(int id)
    {
      Debug.Log("Received grab event flower id " + id);
      FlowerGrabStatus status = flowerGrabbedList.Find(status => status.questInfo.targetId == id);
      if (status != null)
      {
        status.questProgress.currentProgress++;
        status.totalAmount++;
        Debug.Log("Grab event progress " + status.questProgress.currentProgress + " out of " + status.questInfo.target);
        if (status.questProgress.currentProgress >= status.questInfo.target)
        {
          Debug.Log("Quest complete ! " + status.questProgress.gameQuestId);
          GameQuestManager.Instance.CompleteGameQuest(status.questInfo, status.questProgress);
        }
      }
    }
  }
}