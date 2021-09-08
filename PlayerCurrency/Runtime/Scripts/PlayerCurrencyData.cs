using FredericRP.PersistentData;
using FredericRP.StringDataList;
using System.Collections.Generic;
using UnityEngine;

namespace FredericRP.PlayerCurrency
{
  [System.Serializable]
  public class PlayerCurrencyData : SavedData
  {
    public const string CurrencyList = "CurrencyList";

    [System.Serializable]
    public class CurrencyData
    {
      [Select(CurrencyList)]
      public string money;
      public int count = 10;
    }

    [SerializeField]
    public List<CurrencyData> currencyList;

    /// <summary>
    /// Gets the count for the specified money type
    /// </summary>
    /// <returns>The count.</returns>
    /// <param name="moneyId">Money type.</param>
    public int GetCount(string moneyId)
    {
      CurrencyData currencyData = GetCurrencyData(moneyId);
      if (currencyData == null)
        return 0;
      return currencyData.count;
    }

    protected CurrencyData GetCurrencyData(string moneyId)
    {
      // 1. Get data from current list
      CurrencyData currencyData = null;
      if (currencyList == null)
        currencyList = new List<CurrencyData>();
      else
        currencyData = currencyList.Find(element => element.money.Equals(moneyId));
      return currencyData;
    }

    /// <summary>
    /// Credit to the specified money the specified count
    /// </summary>
    /// <returns>The resulting count</returns>
    /// <param name="moneyId">Money type.</param>
    /// <param name="count">Count.</param>
    public int Add(string moneyId, int count)
    {
      CurrencyData currencyData = GetCurrencyData(moneyId);
      if (currencyData == null)
      {
        currencyData = new CurrencyData();
        currencyData.money = moneyId;
        currencyData.count = 0;
        currencyList.Add(currencyData);
      }
      currencyData.count += count;
      // Protection against going below 0 / underflow
      if (currencyData.count < 0)
        currencyData.count = 0;

      return currencyData.count;
    }
  }
}