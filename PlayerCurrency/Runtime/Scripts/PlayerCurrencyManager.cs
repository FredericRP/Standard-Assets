using FredericRP.EventManagement;
using FredericRP.GenericSingleton;
using FredericRP.PersistentData;
using UnityEngine;

namespace FredericRP.PlayerCurrency
{
  public class PlayerCurrencyManager : Singleton<PlayerCurrencyManager>
  {
    [SerializeField]
    GameEvent CurrencyUpdateEvent;//<string, int>;

    // TODO : create online currency management

    public virtual int GetCurrencyCount(string moneyId)
    {
      return PersistentDataSystem.Instance.GetSavedData<PlayerCurrencyData>().GetCount(moneyId);
    }

    public virtual int AddToCurrency(string moneyId, int count)
    {
      int result = PersistentDataSystem.Instance.GetSavedData<PlayerCurrencyData>().Add(moneyId, count);
      CurrencyUpdateEvent?.Raise<string, int>(moneyId, result);
      return result;
    }
  }
}