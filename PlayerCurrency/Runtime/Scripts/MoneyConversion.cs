using System.Collections.Generic;
using UnityEngine;

namespace FredericRP.PlayerCurrency
{
	[CreateAssetMenu(menuName = "Money Conversion")]
	public class MoneyConversion : ScriptableObject
	{
		[System.Serializable]
		public class MoneyData
		{
			public string sourceCurrencyId;
			public string targetCurrencyId;
			public float factor;
			public int min = 0;
			public int max = int.MaxValue;
		}

		public List<MoneyData> conversionList;

		/// <summary>
		/// Gets the resulting number for the target currency from source currency and source number
		/// </summary>
		/// <returns>The sourceNumber converted to the new currency.</returns>
		/// <param name="sourceNumber">Source account</param>
		/// <param name="sourceCurrencyId">Source currency.</param>
		/// <param name="targetCurrencyId">Target currency.</param>
		public int GetTargetCurrencyNumber(int sourceNumber, string sourceCurrencyId, string targetCurrencyId) {
			MoneyData data = conversionList.Find(
				element => 
				element.sourceCurrencyId.Equals(sourceCurrencyId) && element.targetCurrencyId.Equals(targetCurrencyId)
				&& sourceNumber >= element.min && sourceNumber <= element.max);
			
			if (data != null) {
				return (int)(sourceNumber * data.factor);
			}
			return sourceNumber;
		}
	}
}