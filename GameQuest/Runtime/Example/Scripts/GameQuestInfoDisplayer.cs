using UnityEngine;
using UnityEngine.UI;
using System;
using FredericRP.Popups;
using UnityEngine.Localization;

namespace FredericRP.GameQuest
{
  public class GameQuestInfoDisplayer : PopupBase
  {
    [Header("Links")]
    [SerializeField]
    Text titleText;
    [SerializeField]
    Text descriptionText;
    [SerializeField]
    Text completionText;
    [SerializeField]
    Text giftText;
    [SerializeField]
    Text okText;
    [SerializeField]
    Text statusText;


    [SerializeField]
    Image statusImage;
    [SerializeField]
    Image giftImage;

    [Header("Status sprites")]
    [SerializeField]
    Sprite lockMarkSprite;
    [SerializeField]
    Sprite checkMarkSprite;
    [Header("Localization")]
    [SerializeField]
    LocalizedStringTable stringTable;

    private GameQuestInfo questInfo;
    private GameQuestSavedData.QuestProgress questProgress;

    public override void Init(object parameters)
    {
      base.Init(parameters);
      Init((GameQuestInfo)GetParameter(0), (GameQuestSavedData.QuestProgress)GetParameter(1));
    }
    public void Init(GameQuestInfo gameQuestInfo, GameQuestSavedData.QuestProgress questProgress)
    {
      this.questInfo = gameQuestInfo;
      this.questProgress = questProgress;

      titleText.text = stringTable.GetTable().GetEntry("quest." + gameQuestInfo.localizationId + ".title").GetLocalizedString();
      descriptionText.text = stringTable.GetTable().GetEntry("quest." + gameQuestInfo.localizationId + ".description").GetLocalizedString();
      completionText.text = String.Format(stringTable.GetTable().GetEntry("quest.progress").GetLocalizedString(), questProgress.currentProgress, gameQuestInfo.target);

      switch (questProgress.gameQuestStatus)
      {
        case GameQuestSavedData.GameQuestStatus.Locked:
          statusImage.enabled = true;
          statusImage.sprite = lockMarkSprite;
          giftText.enabled = false;
          okText.enabled = true;
          giftImage.enabled = false;
          break;
        case GameQuestSavedData.GameQuestStatus.WaitingForEnable:
          statusImage.enabled = false;
          giftText.enabled = false;
          okText.enabled = true;
          giftImage.enabled = false;
          break;
        case GameQuestSavedData.GameQuestStatus.InProgress:
          statusImage.enabled = true;
          statusImage.sprite = checkMarkSprite;
          giftText.enabled = false;
          okText.enabled = true;
          giftImage.enabled = false;
          break;
        case GameQuestSavedData.GameQuestStatus.WaitingForReward:
          statusImage.enabled = false;
          giftText.enabled = true;
          okText.enabled = false;
          giftImage.enabled = true;
          statusText.enabled = true;
          break;
        default:
          statusImage.enabled = false;
          giftText.enabled = false;
          okText.enabled = true;
          giftImage.enabled = false;
          statusText.enabled = false;
          break;
      }
    }

    public void Validate()
    {
      if (questProgress.gameQuestStatus == GameQuestSavedData.GameQuestStatus.WaitingForEnable)
      {
        LaunchQuest();
      }
      else if (questProgress.gameQuestStatus == GameQuestSavedData.GameQuestStatus.WaitingForReward)
      {
        GetReward();
      }
      Close();
    }

    public void LaunchQuest()
    {
      GameQuestManager.Instance.LaunchQuest(questInfo, questProgress);
    }

    public void GetReward()
    {
      GameQuestManager.Instance.GiveGameQuestReward(questInfo, questProgress);
    }
  }
}
