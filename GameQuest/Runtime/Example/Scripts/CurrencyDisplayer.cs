using FredericRP.EventManagement;
using FredericRP.PlayerCurrency;
using FredericRP.StringDataList;
using TMPro;
using UnityEngine;

public class CurrencyDisplayer : MonoBehaviour
{
  [SerializeField]
  GameEvent CurrencyUpdateEvent;//<string, int>;
  [SerializeField]
  TextMeshProUGUI text;
  [SerializeField]
  [Select(PlayerCurrencyData.CurrencyList)]
  string moneyId;

  private void OnEnable()
  {
    CurrencyUpdateEvent.Listen<string, int>(CheckCurrency);
    // update right away the text from current player currency
    text.text = PlayerCurrencyManager.Instance.GetCurrencyCount(moneyId).ToString();
  }

  private void OnDisable()
  {
    CurrencyUpdateEvent.Delete<string, int>(CheckCurrency);
  }

  private void CheckCurrency(string moneyId, int newCount)
  {
    if (this.moneyId.Equals(moneyId))
    {
      // TODO: tween
      text.text = newCount.ToString();
    }
  }
}
