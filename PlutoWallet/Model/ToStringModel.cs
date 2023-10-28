using System;
using System.Text;
using PlutoWallet.Types;
using Substrate.NetApi;
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

        public static string PrimitiveValueToString(TypeValue type, Memory<byte> bytes, ref int p)
        {
            int m;

            switch (type.Primitive)
            {
                case "U8":
                    m = 1;
                    U8 u8 = new U8();
                    u8.Create(bytes.Slice(p, m).ToArray());
                    p += m;
                    return u8.Value.ToString();
                case "U16":
                    m = 2;
                    U16 u16 = new U16();
                    u16.Create(bytes.Slice(p, m).ToArray());
                    p += m;
                    return u16.Value.ToString();
                case "U32":
                    m = 4;
                    U32 u32 = new U32();
                    u32.Create(bytes.Slice(p, m).ToArray());
                    p += m;
                    return u32.Value.ToString();
                case "U64":
                    m = 8;
                    U64 u64 = new U64();
                    u64.Create(bytes.Slice(p, m).ToArray());
                    p += m;
                    return u64.Value.ToString();
                case "U128":
                    m = 16;
                    U128 u128 = new U128();
                    u128.Create(bytes.Slice(p, m).ToArray());
                    p += m;
                    return u128.Value.ToString();
                case "Bool":
                    m = 1;
                    Bool boolean = new Bool();
                    boolean.Create(bytes.Slice(p, m).ToArray());
                    p += m;
                    return boolean.Value.ToString();
                case "Str":
                    var str = new Str();
                    str.Decode(bytes.ToArray(), ref p);
                    return str.ToString();
                case "Char":
                    m = 1;
                    PrimChar primChar = new PrimChar();
                    primChar.Create(bytes.Slice(p, m).ToArray());
                    p += m;
                    return primChar.Value.ToString();
                default:
                    return "Unable to show";
            }
        }

        public static string SequenceValueToString(TypeValue subType, Memory<byte> bytes, ref int p)
        {
            CompactInteger length = CompactInteger.Decode(bytes.ToArray(), ref p);

            int m;

            string result = "";

            switch (subType.Primitive)
            {
                case "U8":
                    var valueBytes = bytes.Slice(p, length).ToArray();

                    p += length;

                    for (int i = 0; i < length; i++)
                    {
                        result += Encoding.ASCII.GetString(new byte[1] { valueBytes[i] });
                    }

                    break;
                default:

                    return null;
            }

            return result;
        }

        public ToStringModel()
		{
		}
	}
}

