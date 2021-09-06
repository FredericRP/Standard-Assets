using FredericRP.GenericSingleton;
using FredericRP.PersistentData;

namespace FredericRP.PlayerCurrency
{
    public class PlayerCurrencyManager : Singleton<PlayerCurrencyManager>
    {
        // TODO : create online currency management
        // TODO : create child class to migrate currencies from playerstats to persistentdata (if playerstats has gold, take it)
        //bool onlineManagement;

        public virtual int GetCurrencyCount(string moneyId)
        {
            return PersistentDataSystem.Instance.GetSavedData<PlayerCurrencyData>().GetCount(moneyId);
        }

        public virtual int AddToCurrency(string moneyId, int count)
        {
            return PersistentDataSystem.Instance.GetSavedData<PlayerCurrencyData>().Add(moneyId, count);
        }
    }
}