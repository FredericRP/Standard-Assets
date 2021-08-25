using FredericRP.GameQuest;
using FredericRP.Popups;
using UnityEngine;

public class ShowQuestPopup : ShowPopup
{
  [SerializeField]
  GameQuestInfo quest;
  [SerializeField]
  GameQuestSavedData.QuestProgress questProgress;

  public override void DisplayPopup()
  {
    PopupHandler.Instance.ShowPopup(popup, new object[] { quest, questProgress });
  }
}
