using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;
using FredericRP.Popups;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace FredericRP.GameQuest
{
  public class GameQuestInfoDisplayer : PopupBase
  {
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

    private GameQuestInfo gameQuestInfo;
    private GameQuestSavedData.QuestProgress questProgress;

    [Header("Links")]
    [SerializeField]
    Button rewardButton;
    [SerializeField]
    Image backgroundImage;

    [SerializeField]
    Image statusImage;

    [SerializeField]
    Image giftImage;

    [Header("Status sprites")]
    [SerializeField]
    Sprite currentlyActiveQuestSprite;

    [SerializeField]
    Sprite validatedQuestSprite;

    [SerializeField]
    Sprite unvalidedQuestSprite;

    [SerializeField]
    Sprite notUnlockedQuestSprite;

    [SerializeField]
    Sprite waitGetRewardQuestSprite;

    [Space(10)]
    [SerializeField]
    Sprite lockMarkSprite;

    [SerializeField]
    Sprite checkMarkSprite;

    [SerializeField]
    LocalizedStringTable stringTable;

    public override void Init(object parameters)
    {
      base.Init(parameters);
      Init((GameQuestInfo)GetParameter(0), (GameQuestSavedData.QuestProgress)GetParameter(1));
    }
    public void Init(GameQuestInfo gameQuestInfo, GameQuestSavedData.QuestProgress questProgress)
    {
      this.gameQuestInfo = gameQuestInfo;
      this.questProgress = questProgress;

      titleText.text = stringTable.GetTable().GetEntry("quest." + gameQuestInfo.localizationId + ".title").GetLocalizedString();
      descriptionText.text = stringTable.GetTable().GetEntry("quest." + gameQuestInfo.localizationId + ".description").GetLocalizedString();
      completionText.text = questProgress.currentProgress + "/" + gameQuestInfo.target ;// gameQuestInfo.Completion(info);
      
      switch (questProgress.gameQuestStatus)
      {
        case GameQuestSavedData.GameQuestStatus.Enabled:
          rewardButton.onClick.RemoveAllListeners();
          rewardButton.onClick.AddListener(Hide);
          statusImage.enabled = false;
          backgroundImage.sprite = currentlyActiveQuestSprite;

          giftText.enabled = false;
          okText.enabled = true;
          giftImage.enabled = false;

          break;
        case GameQuestSavedData.GameQuestStatus.Locked:
          rewardButton.onClick.RemoveAllListeners();
          rewardButton.onClick.AddListener(Hide);
          statusImage.enabled = true;
          statusImage.sprite = lockMarkSprite;
          backgroundImage.sprite = notUnlockedQuestSprite;
          giftText.enabled = false;
          okText.enabled = true;
          giftImage.enabled = false;


          break;
        case GameQuestSavedData.GameQuestStatus.WaitingForEnable:
          rewardButton.onClick.RemoveAllListeners();
          rewardButton.onClick.AddListener(Hide);
          statusImage.enabled = false;
          backgroundImage.sprite = unvalidedQuestSprite;
          giftText.enabled = false;
          okText.enabled = true;
          giftImage.enabled = false;

          //statusText.text = unvalidatedID;// SmartLocalization.LanguageManager.Instance.GetTextValue(unvalidatedID);

          break;
        case GameQuestSavedData.GameQuestStatus.InProgress:
          rewardButton.onClick.RemoveAllListeners();
          rewardButton.onClick.AddListener(Hide);
          statusImage.enabled = true;
          statusImage.sprite = checkMarkSprite;
          backgroundImage.sprite = validatedQuestSprite;
          giftText.enabled = false;
          okText.enabled = true;
          giftImage.enabled = false;

          //statusText.text = validatedID;// SmartLocalization.LanguageManager.Instance.GetTextValue(validatedID);

          break;
        case GameQuestSavedData.GameQuestStatus.WaitingForReward:
          rewardButton.onClick.RemoveAllListeners();
          rewardButton.onClick.AddListener(GetReward);
          statusImage.enabled = false;
          backgroundImage.sprite = waitGetRewardQuestSprite;
          giftText.enabled = true;
          okText.enabled = false;
          giftImage.enabled = true;
          statusText.enabled = false;

          statusText.enabled = true;
          //statusText.text = validatedID + " " + questProgress.LaunchDate.ToString(CultureInfo.InvariantCulture);// SmartLocalization.LanguageManager.Instance.GetTextValue(validatedID) + " " + info.launchDate.ToString(CultureInfo.InvariantCulture);

          break;
      }
    }

    void OnDisable()
    {
      /*
      GameQuestManager.OnGameQuestCompleted -= UpdateSimilarInformation;
      GameQuestManager.OnGameQuestExpired -= UpdateSimilarInformation;
      // */
    }

    public void UpdateSimilarInformation(GameQuestInfo newGameQuestInfo, GameQuestSavedData.QuestProgress newInfo)
    {
      if (newGameQuestInfo == gameQuestInfo && newInfo == questProgress)
      {
        Init(newGameQuestInfo, newInfo);
      }
    }

    public void GetReward()
    {
      GameQuestManager.Instance.GetGameQuestReward(this.gameQuestInfo.gameQuestReward);
      this.questProgress.gameQuestStatus = GameQuestSavedData.GameQuestStatus.Complete;

      Init(this.gameQuestInfo, this.questProgress);
    }
  }
}
