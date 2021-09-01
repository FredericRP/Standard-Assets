using FredericRP.GameQuest;
using FredericRP.PersistentData;
using FredericRP.Popups;
using UnityEngine;

public class ShowQuestPopup : ShowPopup
{
  [SerializeField]
  bool onAwake = false;

  private void Awake()
  {
    if (onAwake)
      DisplayPopup();
  }

  public override void DisplayPopup()
  {
    GameQuestInfo questInfo = GameQuestManager.Instance.GetWaitingForEnableQuest();
    if (questInfo == null)
    {
      Debug.Log("No quest to launch");
      questInfo = GameQuestManager.Instance.GetQuestFromStatus(GameQuestSavedData.GameQuestStatus.WaitingForReward);
      if (questInfo == null)
      {
        Debug.Log("No quest waiting for reward");
        questInfo = GameQuestManager.Instance.GetInProgressQuest();
        if (questInfo == null)
        {
          Debug.Log("No quest in progress");
          return;
        }
      }
    }
    GameQuestSavedData questSavedData = PersistentDataSystem.Instance.GetSavedData<GameQuestSavedData>();
    GameQuestSavedData.QuestProgress questProgress = null;
    if (questSavedData != null)
      questProgress = questSavedData.GetQuestProgress(questInfo.gameQuestID);
    PopupHandler.Instance.ShowPopup(popup, new object[] { questInfo, questProgress });
  }
}
