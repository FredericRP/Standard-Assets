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
        // Unlock quest by default
        if (flowerGrabbedList[i].questProgress.gameQuestStatus == GameQuestSavedData.GameQuestStatus.Locked)
          flowerGrabbedList[i].questProgress.gameQuestStatus = GameQuestSavedData.GameQuestStatus.WaitingForEnable;

      }
      // Ensure data is saved right away to be available for other systems
      //PersistentDataSystem.Instance.SaveData<GameQuestSavedData>();
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
      FlowerGrabStatus status = flowerGrabbedList.Find(item => item.gameQuestId == questInfo.gameQuestID);
      if (status != null)
      {
        Debug.Log("Received quest launched for flower id " + questInfo.targetId);
        status.questProgress.currentProgress = 0;
      }
    }

    void FlowerGrabbed(int id)
    {
      FlowerGrabStatus status = flowerGrabbedList.Find(item => item.questInfo.targetId == id);
      if (status != null)
      {
        status.totalAmount++;
        if (status.questProgress.currentProgress < status.questInfo.target)
          status.questProgress.currentProgress++;
        else
        {
          status.questProgress.currentProgress = status.questInfo.target;
          GameQuestManager.Instance.CompleteGameQuest(status.questInfo, status.questProgress);
        }
      }
    }
  }
}