using System;
using AzeroIdResolver;

namespace PlutoWallet.Model.AzeroId
{
	public class AzeroIdModel
	{
        public static async Task<string> GetReservedUntilStringForName(string name)
        {
            var period = await TzeroId.GetRegistrationPeriodForName(name.ToLower());

            if (period != null)
            {
                return period.Value.Item2.Day + "." + period.Value.Item2.Month + "." + period.Value.Item2.Year;
            }

            return "";
        }
	}
}

