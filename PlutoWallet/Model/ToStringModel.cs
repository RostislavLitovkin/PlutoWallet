using System;
using System.Text;
using Substrate.NetApi.Model.Types.Primitive;

namespace PlutoWallet.Model
{
	public class ToStringModel
	{
		public static string VecU8ToString(U8[] value)
        {
            string result = "";
            for (int i = 0; i < value.Count(); i++)
            {
                result += Encoding.ASCII.GetString(new byte[1] { value.ElementAt(i).Value });
            }

            return result;
        }

        public ToStringModel()
		{
		}
	}
}

