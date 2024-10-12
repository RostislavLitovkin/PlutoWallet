using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.Contracts;
using EventAttribute = Nethereum.ABI.FunctionEncoding.Attributes.EventAttribute;

namespace UniqueryPlus.EVM
{

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public class UniqueSellConsole
    {
        public static async Task NotMainAsync()
        {
            await Task.Delay(1);
            var url = "https://rpc.unique.network";
            //var url = "https://mainnet.infura.io";
            var privateKey = "0x7580e7fb49df1c861f0050fae31c2224c6aba908e116b8da44ee8cd927b990b0";
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            var web3 = new Web3(account, url);

            /* Deployment 
           var uniqueSellDeployment = new UniqueSellDeployment();

           var transactionReceiptDeployment = await web3.Eth.GetContractDeploymentHandler<UniqueSellDeployment>().SendRequestAndWaitForReceiptAsync(uniqueSellDeployment);
           var contractAddress = transactionReceiptDeployment.ContractAddress;
            */
            var contractHandler = web3.Eth.GetContractHandler("0x7cccb70061c2725b9a64b13c95f78500d2b3b382");

            /** Function: addAdmin**/
            /*
            var addAdminFunction = new AddAdminFunction();
            addAdminFunction.Admin = admin;
            var addAdminFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(addAdminFunction);
            */


            /** Function: addCurrency**/
            /*
            var addCurrencyFunction = new AddCurrencyFunction();
            addCurrencyFunction.CollectionId = collectionId;
            addCurrencyFunction.Fee = fee;
            var addCurrencyFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(addCurrencyFunction);
            */


            /** Function: addToBlacklist**/
            /*
            var addToBlacklistFunction = new AddToBlacklistFunction();
            addToBlacklistFunction.CollectionId = collectionId;
            var addToBlacklistFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(addToBlacklistFunction);
            */


            /** Function: admins**/
            /*
            var adminsFunction = new AdminsFunction();
            adminsFunction.ReturnValue1 = returnValue1;
            var adminsFunctionReturn = await contractHandler.QueryAsync<AdminsFunction, bool>(adminsFunction);
            */


            /** Function: buildVersion**/
            /*
            var buildVersionFunctionReturn = await contractHandler.QueryAsync<BuildVersionFunction, uint>();
            */


            /** Function: buy**/
            /*
            var buyFunction = new BuyFunction();
            buyFunction.CollectionId = collectionId;
            buyFunction.TokenId = tokenId;
            buyFunction.Amount = amount;
            buyFunction.Buyer = buyer;
            var buyFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(buyFunction);
            */


            /** Function: changePrice**/
            /*
            var changePriceFunction = new ChangePriceFunction();
            changePriceFunction.CollectionId = collectionId;
            changePriceFunction.TokenId = tokenId;
            changePriceFunction.Price = price;
            changePriceFunction.Currency = currency;
            var changePriceFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(changePriceFunction);
            */


            /** Function: checkApproved**/
            /*
            var checkApprovedFunction = new CheckApprovedFunction();
            checkApprovedFunction.CollectionId = collectionId;
            checkApprovedFunction.TokenId = tokenId;
            var checkApprovedFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(checkApprovedFunction);
            */


            /** Function: getCurrency**/
            /*
            var getCurrencyFunction = new GetCurrencyFunction(); 
            getCurrencyFunction.CollectionId = collectionId;
            var getCurrencyOutputDTO = await contractHandler.QueryDeserializingToObjectAsync<GetCurrencyFunction, GetCurrencyOutputDTO>(getCurrencyFunction);
            */


            /** Function: getOrder**/
            /*
            var getOrderFunction = new GetOrderFunction(); 
            getOrderFunction.CollectionId = collectionId;
            getOrderFunction.TokenId = tokenId;
            var getOrderOutputDTO = await contractHandler.QueryDeserializingToObjectAsync<GetOrderFunction, GetOrderOutputDTO>(getOrderFunction);
            */


            /** Function: initialize**/
            /*
            var initializeFunction = new InitializeFunction();
            initializeFunction.Fee = fee;
            var initializeFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(initializeFunction);
            */


            /** Function: marketFee**/
            /*
            var marketFeeFunctionReturn = await contractHandler.QueryAsync<MarketFeeFunction, uint>();
            */


            /** Function: owner**/
            /*
            var ownerFunctionReturn = await contractHandler.QueryAsync<OwnerFunction, string>();
            */


            /** Function: put**/
            /*
            var putFunction = new PutFunction();
            putFunction.CollectionId = collectionId;
            putFunction.TokenId = tokenId;
            putFunction.Price = price;
            putFunction.Currency = currency;
            putFunction.Amount = amount;
            putFunction.Seller = seller;
            var putFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(putFunction);
            */


            /** Function: removeAdmin**/
            /*
            var removeAdminFunction = new RemoveAdminFunction();
            removeAdminFunction.Admin = admin;
            var removeAdminFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(removeAdminFunction);
            */


            /** Function: removeCurrency**/
            /*
            var removeCurrencyFunction = new RemoveCurrencyFunction();
            removeCurrencyFunction.CollectionId = collectionId;
            var removeCurrencyFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(removeCurrencyFunction);
            */


            /** Function: removeFromBlacklist**/
            /*
            var removeFromBlacklistFunction = new RemoveFromBlacklistFunction();
            removeFromBlacklistFunction.CollectionId = collectionId;
            var removeFromBlacklistFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(removeFromBlacklistFunction);
            */


            /** Function: renounceOwnership**/
            /*
            var renounceOwnershipFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync<RenounceOwnershipFunction>();
            */


            /** Function: revoke**/
            /*
            var revokeFunction = new RevokeFunction();
            revokeFunction.CollectionId = collectionId;
            revokeFunction.TokenId = tokenId;
            revokeFunction.Amount = amount;
            var revokeFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(revokeFunction);
            */


            /** Function: revokeAdmin**/
            /*
            var revokeAdminFunction = new RevokeAdminFunction();
            revokeAdminFunction.CollectionId = collectionId;
            revokeAdminFunction.TokenId = tokenId;
            var revokeAdminFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(revokeAdminFunction);
            */


            /** Function: revokeListAdmin**/
            /*
            var revokeListAdminFunction = new RevokeListAdminFunction();
            revokeListAdminFunction.CollectionId = collectionId;
            revokeListAdminFunction.TokenIdList = tokenIdList;
            var revokeListAdminFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(revokeListAdminFunction);
            */


            /** Function: setMarketFee**/
            /*
            var setMarketFeeFunction = new SetMarketFeeFunction();
            setMarketFeeFunction.Fee = fee;
            var setMarketFeeFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(setMarketFeeFunction);
            */


            /** Function: setRoyaltyHelpers**/
            /*
            var setRoyaltyHelpersFunction = new SetRoyaltyHelpersFunction();
            setRoyaltyHelpersFunction.RoyaltyHelpersAddress = royaltyHelpersAddress;
            var setRoyaltyHelpersFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(setRoyaltyHelpersFunction);
            */


            /** Function: transferOwnership**/
            /*
            var transferOwnershipFunction = new TransferOwnershipFunction();
            transferOwnershipFunction.NewOwner = newOwner;
            var transferOwnershipFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction);
            */


            /** Function: version**/
            /*
            var versionFunctionReturn = await contractHandler.QueryAsync<VersionFunction, uint>();
            */


            /** Function: withdraw**/
            /*
            var withdrawFunction = new WithdrawFunction();
            withdrawFunction.To = to;
            withdrawFunction.Currency = currency;
            withdrawFunction.Balance = balance;
            var withdrawFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(withdrawFunction);
            */
        }

    }

    public partial class UniqueSellDeployment : UniqueSellDeploymentBase
    {
        public UniqueSellDeployment() : base(BYTECODE) { }
        public UniqueSellDeployment(string byteCode) : base(byteCode) { }
    }

    public class UniqueSellDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public UniqueSellDeploymentBase() : base(BYTECODE) { }
        public UniqueSellDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class AddAdminFunction : AddAdminFunctionBase { }

    [Function("addAdmin")]
    public class AddAdminFunctionBase : FunctionMessage
    {
        [Parameter("address", "admin", 1)]
        public virtual string Admin { get; set; }
    }

    public partial class AddCurrencyFunction : AddCurrencyFunctionBase { }

    [Function("addCurrency")]
    public class AddCurrencyFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "collectionId", 1)]
        public virtual uint CollectionId { get; set; }
        [Parameter("uint32", "fee", 2)]
        public virtual uint Fee { get; set; }
    }

    public partial class AddToBlacklistFunction : AddToBlacklistFunctionBase { }

    [Function("addToBlacklist")]
    public class AddToBlacklistFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "collectionId", 1)]
        public virtual uint CollectionId { get; set; }
    }

    public partial class AdminsFunction : AdminsFunctionBase { }

    [Function("admins", "bool")]
    public class AdminsFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class BuildVersionFunction : BuildVersionFunctionBase { }

    [Function("buildVersion", "uint32")]
    public class BuildVersionFunctionBase : FunctionMessage
    {

    }

    public partial class BuyFunction : BuyFunctionBase { }

    [Function("buy")]
    public class BuyFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "collectionId", 1)]
        public virtual uint CollectionId { get; set; }
        [Parameter("uint32", "tokenId", 2)]
        public virtual uint TokenId { get; set; }
        [Parameter("uint32", "amount", 3)]
        public virtual uint Amount { get; set; }
        [Parameter("tuple", "buyer", 4)]
        public virtual CrossAddress Buyer { get; set; }
    }

    public partial class ChangePriceFunction : ChangePriceFunctionBase { }

    [Function("changePrice")]
    public class ChangePriceFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "collectionId", 1)]
        public virtual uint CollectionId { get; set; }
        [Parameter("uint32", "tokenId", 2)]
        public virtual uint TokenId { get; set; }
        [Parameter("uint256", "price", 3)]
        public virtual BigInteger Price { get; set; }
        [Parameter("uint32", "currency", 4)]
        public virtual uint Currency { get; set; }
    }

    public partial class CheckApprovedFunction : CheckApprovedFunctionBase { }

    [Function("checkApproved")]
    public class CheckApprovedFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "collectionId", 1)]
        public virtual uint CollectionId { get; set; }
        [Parameter("uint32", "tokenId", 2)]
        public virtual uint TokenId { get; set; }
    }

    public partial class GetCurrencyFunction : GetCurrencyFunctionBase { }

    [Function("getCurrency", typeof(GetCurrencyOutputDTO))]
    public class GetCurrencyFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "collectionId", 1)]
        public virtual uint CollectionId { get; set; }
    }

    public partial class GetOrderFunction : GetOrderFunctionBase { }

    [Function("getOrder", typeof(GetOrderOutputDTO))]
    public class GetOrderFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "collectionId", 1)]
        public virtual uint CollectionId { get; set; }
        [Parameter("uint32", "tokenId", 2)]
        public virtual uint TokenId { get; set; }
    }

    public partial class InitializeFunction : InitializeFunctionBase { }

    [Function("initialize")]
    public class InitializeFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "fee", 1)]
        public virtual uint Fee { get; set; }
    }

    public partial class MarketFeeFunction : MarketFeeFunctionBase { }

    [Function("marketFee", "uint32")]
    public class MarketFeeFunctionBase : FunctionMessage
    {

    }

    public partial class OwnerFunction : OwnerFunctionBase { }

    [Function("owner", "address")]
    public class OwnerFunctionBase : FunctionMessage
    {

    }

    public partial class PutFunction : PutFunctionBase { }

    [Function("put")]
    public class PutFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "collectionId", 1)]
        public virtual uint CollectionId { get; set; }
        [Parameter("uint32", "tokenId", 2)]
        public virtual uint TokenId { get; set; }
        [Parameter("uint256", "price", 3)]
        public virtual BigInteger Price { get; set; }
        [Parameter("uint32", "currency", 4)]
        public virtual uint Currency { get; set; }
        [Parameter("uint32", "amount", 5)]
        public virtual uint Amount { get; set; }
        [Parameter("tuple", "seller", 6)]
        public virtual CrossAddress Seller { get; set; }
    }

    public partial class RemoveAdminFunction : RemoveAdminFunctionBase { }

    [Function("removeAdmin")]
    public class RemoveAdminFunctionBase : FunctionMessage
    {
        [Parameter("address", "admin", 1)]
        public virtual string Admin { get; set; }
    }

    public partial class RemoveCurrencyFunction : RemoveCurrencyFunctionBase { }

    [Function("removeCurrency")]
    public class RemoveCurrencyFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "collectionId", 1)]
        public virtual uint CollectionId { get; set; }
    }

    public partial class RemoveFromBlacklistFunction : RemoveFromBlacklistFunctionBase { }

    [Function("removeFromBlacklist")]
    public class RemoveFromBlacklistFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "collectionId", 1)]
        public virtual uint CollectionId { get; set; }
    }

    public partial class RenounceOwnershipFunction : RenounceOwnershipFunctionBase { }

    [Function("renounceOwnership")]
    public class RenounceOwnershipFunctionBase : FunctionMessage
    {

    }

    public partial class RevokeFunction : RevokeFunctionBase { }

    [Function("revoke")]
    public class RevokeFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "collectionId", 1)]
        public virtual uint CollectionId { get; set; }
        [Parameter("uint32", "tokenId", 2)]
        public virtual uint TokenId { get; set; }
        [Parameter("uint32", "amount", 3)]
        public virtual uint Amount { get; set; }
    }

    public partial class RevokeAdminFunction : RevokeAdminFunctionBase { }

    [Function("revokeAdmin")]
    public class RevokeAdminFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "collectionId", 1)]
        public virtual uint CollectionId { get; set; }
        [Parameter("uint32", "tokenId", 2)]
        public virtual uint TokenId { get; set; }
    }

    public partial class RevokeListAdminFunction : RevokeListAdminFunctionBase { }

    [Function("revokeListAdmin")]
    public class RevokeListAdminFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "collectionId", 1)]
        public virtual uint CollectionId { get; set; }
        [Parameter("uint32[]", "tokenIdList", 2)]
        public virtual List<uint> TokenIdList { get; set; }
    }

    public partial class SetMarketFeeFunction : SetMarketFeeFunctionBase { }

    [Function("setMarketFee")]
    public class SetMarketFeeFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "fee", 1)]
        public virtual uint Fee { get; set; }
    }

    public partial class SetRoyaltyHelpersFunction : SetRoyaltyHelpersFunctionBase { }

    [Function("setRoyaltyHelpers")]
    public class SetRoyaltyHelpersFunctionBase : FunctionMessage
    {
        [Parameter("address", "royaltyHelpersAddress", 1)]
        public virtual string RoyaltyHelpersAddress { get; set; }
    }

    public partial class TransferOwnershipFunction : TransferOwnershipFunctionBase { }

    [Function("transferOwnership")]
    public class TransferOwnershipFunctionBase : FunctionMessage
    {
        [Parameter("address", "newOwner", 1)]
        public virtual string NewOwner { get; set; }
    }

    public partial class VersionFunction : VersionFunctionBase { }

    [Function("version", "uint32")]
    public class VersionFunctionBase : FunctionMessage
    {

    }

    public partial class WithdrawFunction : WithdrawFunctionBase { }

    [Function("withdraw")]
    public class WithdrawFunctionBase : FunctionMessage
    {
        [Parameter("tuple", "to", 1)]
        public virtual CrossAddress To { get; set; }
        [Parameter("uint32", "currency", 2)]
        public virtual uint Currency { get; set; }
        [Parameter("uint256", "balance", 3)]
        public virtual BigInteger Balance { get; set; }
    }

    public partial class InitializedEventDTO : InitializedEventDTOBase { }

    [Event("Initialized")]
    public class InitializedEventDTOBase : IEventDTO
    {
        [Parameter("uint64", "version", 1, false)]
        public virtual ulong Version { get; set; }
    }

    public partial class OwnershipTransferredEventDTO : OwnershipTransferredEventDTOBase { }

    [Event("OwnershipTransferred")]
    public class OwnershipTransferredEventDTOBase : IEventDTO
    {
        [Parameter("address", "previousOwner", 1, true)]
        public virtual string PreviousOwner { get; set; }
        [Parameter("address", "newOwner", 2, true)]
        public virtual string NewOwner { get; set; }
    }

    public partial class TokenIsApprovedEventDTO : TokenIsApprovedEventDTOBase { }

    [Event("TokenIsApproved")]
    public class TokenIsApprovedEventDTOBase : IEventDTO
    {
        [Parameter("uint32", "version", 1, false)]
        public virtual uint Version { get; set; }
        [Parameter("tuple", "item", 2, false)]
        public virtual Order Item { get; set; }
    }

    public partial class TokenIsPurchasedEventDTO : TokenIsPurchasedEventDTOBase { }

    [Event("TokenIsPurchased")]
    public class TokenIsPurchasedEventDTOBase : IEventDTO
    {
        [Parameter("uint32", "version", 1, false)]
        public virtual uint Version { get; set; }
        [Parameter("tuple", "item", 2, false)]
        public virtual Order Item { get; set; }
        [Parameter("uint32", "salesAmount", 3, false)]
        public virtual uint SalesAmount { get; set; }
        [Parameter("tuple", "buyer", 4, false)]
        public virtual CrossAddress Buyer { get; set; }
        [Parameter("tuple[]", "royalties", 5, false)]
        public virtual List<RoyaltyAmount> Royalties { get; set; }
    }

    public partial class TokenIsUpForSaleEventDTO : TokenIsUpForSaleEventDTOBase { }

    [Event("TokenIsUpForSale")]
    public class TokenIsUpForSaleEventDTOBase : IEventDTO
    {
        [Parameter("uint32", "version", 1, false)]
        public virtual uint Version { get; set; }
        [Parameter("tuple", "item", 2, false)]
        public virtual Order Item { get; set; }
    }

    public partial class TokenPriceChangedEventDTO : TokenPriceChangedEventDTOBase { }

    [Event("TokenPriceChanged")]
    public class TokenPriceChangedEventDTOBase : IEventDTO
    {
        [Parameter("uint32", "version", 1, false)]
        public virtual uint Version { get; set; }
        [Parameter("tuple", "item", 2, false)]
        public virtual Order Item { get; set; }
    }

    public partial class TokenRevokeEventDTO : TokenRevokeEventDTOBase { }

    [Event("TokenRevoke")]
    public class TokenRevokeEventDTOBase : IEventDTO
    {
        [Parameter("uint32", "version", 1, false)]
        public virtual uint Version { get; set; }
        [Parameter("tuple", "item", 2, false)]
        public virtual Order Item { get; set; }
        [Parameter("uint32", "amount", 3, false)]
        public virtual uint Amount { get; set; }
    }







    public partial class AdminsOutputDTO : AdminsOutputDTOBase { }

    [FunctionOutput]
    public class AdminsOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class BuildVersionOutputDTO : BuildVersionOutputDTOBase { }

    [FunctionOutput]
    public class BuildVersionOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint32", "", 1)]
        public virtual uint ReturnValue1 { get; set; }
    }







    public partial class GetCurrencyOutputDTO : GetCurrencyOutputDTOBase { }

    [FunctionOutput]
    public class GetCurrencyOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("tuple", "", 1)]
        public virtual Currency ReturnValue1 { get; set; }
    }

    public partial class GetOrderOutputDTO : GetOrderOutputDTOBase { }

    [FunctionOutput]
    public class GetOrderOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("tuple", "", 1)]
        public virtual Order ReturnValue1 { get; set; }
    }



    public partial class MarketFeeOutputDTO : MarketFeeOutputDTOBase { }

    [FunctionOutput]
    public class MarketFeeOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint32", "", 1)]
        public virtual uint ReturnValue1 { get; set; }
    }

    public partial class OwnerOutputDTO : OwnerOutputDTOBase { }

    [FunctionOutput]
    public class OwnerOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }























    public partial class VersionOutputDTO : VersionOutputDTOBase { }

    [FunctionOutput]
    public class VersionOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint32", "", 1)]
        public virtual uint ReturnValue1 { get; set; }
    }



    public partial class CrossAddress : CrossAddressBase { }

    public class CrossAddressBase
    {
        [Parameter("address", "eth", 1)]
        public virtual string Eth { get; set; }
        [Parameter("uint256", "sub", 2)]
        public virtual BigInteger Sub { get; set; }
    }

    public partial class Order : OrderBase { }

    public class OrderBase
    {
        [Parameter("uint32", "id", 1)]
        public virtual uint Id { get; set; }
        [Parameter("uint32", "collectionId", 2)]
        public virtual uint CollectionId { get; set; }
        [Parameter("uint32", "tokenId", 3)]
        public virtual uint TokenId { get; set; }
        [Parameter("uint32", "amount", 4)]
        public virtual uint Amount { get; set; }
        [Parameter("uint256", "price", 5)]
        public virtual BigInteger Price { get; set; }
        [Parameter("uint32", "currency", 6)]
        public virtual uint Currency { get; set; }
        [Parameter("tuple", "seller", 7)]
        public virtual CrossAddress Seller { get; set; }
    }

    public partial class RoyaltyAmount : RoyaltyAmountBase { }

    public class RoyaltyAmountBase
    {
        [Parameter("tuple", "crossAddress", 1)]
        public virtual CrossAddress CrossAddress { get; set; }
        [Parameter("uint256", "amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class Currency : CurrencyBase { }

    public class CurrencyBase
    {
        [Parameter("bool", "isAvailable", 1)]
        public virtual bool IsAvailable { get; set; }
        [Parameter("uint32", "collectionId", 2)]
        public virtual uint CollectionId { get; set; }
        [Parameter("uint32", "fee", 3)]
        public virtual uint Fee { get; set; }
    }
}
