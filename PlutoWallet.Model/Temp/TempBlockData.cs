using Newtonsoft.Json;
namespace PlutoWallet.Model.Temp
{
    public class TempBlockData
    {
        //
        // Summary:
        //     Block
        public TempBlock Block { get; set; }

        //
        // Summary:
        //     Justification
        public object Justification { get; set; }

        //
        // Summary:
        //     Block Data Constructor
        //
        // Parameters:
        //   block:
        //
        //   justification:
        public TempBlockData(TempBlock block, object justification)
        {
            Block = block;
            Justification = justification;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
