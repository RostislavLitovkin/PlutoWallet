using Substrate.NetApi.Model.Types.Base;
using System.Text;
using Wasmtime;
namespace PlutoWallet.Model
{
    public class HydrationMath
    {
        public static string CalculateSpotPrice(string assetAReserve, string assetAHubReserve, string assetBReserve, string assetBHubReserve)
        {
            var filePath = "./Hydration/hydra_dx_wasm_bg.wasm";

            // Create an engine and a store
            using var engine = new Engine();
            using var module = Wasmtime.Module.FromFile(engine, filePath);
            using var store = new Store(engine);

            // Create a linker to link the module and the store
            var linker = new Linker(engine);

            // Instantiate the WebAssembly module
            var instance = linker.Instantiate(store, module);

            // Access and invoke a function from the module
            var runFunction = instance.GetFunction("calculate_spot_price")?.WrapAction<int,int,int,int ,int,int,int,int, int>();

            if (runFunction != null)
            {
                runFunction(1, 1, 1, 1,  1, 1, 1, 1, 1); // Call the function with an argument
                return "Good";//result;
            }
            else
            {
                return "Error";
            }
        }
    }
}
