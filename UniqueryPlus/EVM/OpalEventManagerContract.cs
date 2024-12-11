using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace UniqueryPlus.EVM
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public class EventManagerConsole
    {
        public static async Task NotMainAsync()
        {
            await Task.Delay(1);
            var url = "http://testchain.nethereum.com:8545";
            //var url = "https://mainnet.infura.io";
            var privateKey = "0x7580e7fb49df1c861f0050fae31c2224c6aba908e116b8da44ee8cd927b990b0";
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            var web3 = new Web3(account, url);

            /* Deployment 
           var eventManagerDeployment = new EventManagerDeployment();
               eventManagerDeployment.CollectionCreationFee = collectionCreationFee;
           var transactionReceiptDeployment = await web3.Eth.GetContractDeploymentHandler<EventManagerDeployment>().SendRequestAndWaitForReceiptAsync(eventManagerDeployment);
           var contractAddress = transactionReceiptDeployment.ContractAddress;
            */
            var contractHandler = web3.Eth.GetContractHandler(UniqueContracts.OPAL_EVENT_MANAGER_CONTRACT_ADDRESS);

            /** Function: createCollection**/
            /*
            var createCollectionFunction = new CreateCollectionFunction();
            createCollectionFunction.Name = name;
            createCollectionFunction.Description = description;
            createCollectionFunction.Symbol = symbol;
            createCollectionFunction.EventConfig = eventConfig;
            var createCollectionFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(createCollectionFunction);
            */


            /** Function: createToken**/
            /*
            var createTokenFunction = new CreateTokenFunction();
            createTokenFunction.CollectionAddress = collectionAddress;
            createTokenFunction.Owner = owner;
            var createTokenFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(createTokenFunction);
            */


            /** Function: getCollectionCreationFee**/
            /*
            var getCollectionCreationFeeFunctionReturn = await contractHandler.QueryAsync<GetCollectionCreationFeeFunction, BigInteger>();
            */


            /** Function: getEventConfig**/
            /*
            var getEventConfigFunction = new GetEventConfigFunction(); 
            getEventConfigFunction.CollectionAddress = collectionAddress;
            var getEventConfigOutputDTO = await contractHandler.QueryDeserializingToObjectAsync<GetEventConfigFunction, GetEventConfigOutputDTO>(getEventConfigFunction);
            */
        }

    }

    public partial class EventManagerDeployment : EventManagerDeploymentBase
    {
        public EventManagerDeployment() : base(BYTECODE) { }
        public EventManagerDeployment(string byteCode) : base(byteCode) { }
    }

    public class EventManagerDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public EventManagerDeploymentBase() : base(BYTECODE) { }
        public EventManagerDeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("uint256", "_collectionCreationFee", 1)]
        public virtual BigInteger CollectionCreationFee { get; set; }
    }

    public partial class CreateCollectionFunction : CreateCollectionFunctionBase { }

    [Function("createCollection")]
    public class CreateCollectionFunctionBase : FunctionMessage
    {
        [Parameter("string", "_name", 1)]
        public virtual string Name { get; set; }
        [Parameter("string", "_description", 2)]
        public virtual string Description { get; set; }
        [Parameter("string", "_symbol", 3)]
        public virtual string Symbol { get; set; }
        [Parameter("tuple", "_eventConfig", 4)]
        public virtual EventConfig EventConfig { get; set; }
    }

    public partial class CreateTokenFunction : CreateTokenFunctionBase { }

    [Function("createToken")]
    public class CreateTokenFunctionBase : FunctionMessage
    {
        [Parameter("address", "_collectionAddress", 1)]
        public virtual string CollectionAddress { get; set; }
        [Parameter("tuple", "_owner", 2)]
        public virtual CrossAddress Owner { get; set; }
    }

    public partial class GetCollectionCreationFeeFunction : GetCollectionCreationFeeFunctionBase { }

    [Function("getCollectionCreationFee", "uint256")]
    public class GetCollectionCreationFeeFunctionBase : FunctionMessage
    {

    }

    public partial class GetEventConfigFunction : GetEventConfigFunctionBase { }

    [Function("getEventConfig", typeof(GetEventConfigOutputDTO))]
    public class GetEventConfigFunctionBase : FunctionMessage
    {
        [Parameter("address", "_collectionAddress", 1)]
        public virtual string CollectionAddress { get; set; }
    }

    public partial class EventCreatedEventDTO : EventCreatedEventDTOBase { }

    [Event("EventCreated")]
    public class EventCreatedEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "collectionId", 1, false)]
        public virtual BigInteger CollectionId { get; set; }
        [Parameter("address", "collectionAddress", 2, false)]
        public virtual string CollectionAddress { get; set; }
    }

    public partial class TokenClaimedEventDTO : TokenClaimedEventDTOBase { }

    [Event("TokenClaimed")]
    public class TokenClaimedEventDTOBase : IEventDTO
    {
        [Parameter("tuple", "owner", 1, true)]
        public virtual CrossAddress Owner { get; set; }
        [Parameter("uint256", "collectionId", 2, true)]
        public virtual BigInteger CollectionId { get; set; }
        [Parameter("uint256", "tokenId", 3, false)]
        public virtual BigInteger TokenId { get; set; }
    }





    public partial class GetCollectionCreationFeeOutputDTO : GetCollectionCreationFeeOutputDTOBase { }

    [FunctionOutput]
    public class GetCollectionCreationFeeOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetEventConfigOutputDTO : GetEventConfigOutputDTOBase { }

    [FunctionOutput]
    public class GetEventConfigOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("tuple", "", 1)]
        public virtual EventConfig ReturnValue1 { get; set; }
    }

    public partial class CrossAddress : CrossAddressBase { }

    //  Already included
    /*public class CrossAddressBase
    {
        [Parameter("address", "eth", 1)]
        public virtual string Eth { get; set; }
        [Parameter("uint256", "sub", 2)]
        public virtual BigInteger Sub { get; set; }
    }*/

    public partial class Attribute : AttributeBase { }

    public class AttributeBase
    {
        [Parameter("string", "trait_type", 1)]
        public virtual string TraitType { get; set; }
        [Parameter("string", "value", 2)]
        public virtual string Value { get; set; }
    }

    public partial class EventConfig : EventConfigBase { }

    public class EventConfigBase
    {
        [Parameter("uint256", "startTimestamp", 1)]
        public virtual BigInteger StartTimestamp { get; set; }
        [Parameter("uint256", "endTimestamp", 2)]
        public virtual BigInteger EndTimestamp { get; set; }
        [Parameter("uint256", "accountLimit", 3)]
        public virtual BigInteger AccountLimit { get; set; }
        [Parameter("string", "collectionCoverImage", 4)]
        public virtual string CollectionCoverImage { get; set; }
        [Parameter("string", "tokenImage", 5)]
        public virtual string TokenImage { get; set; }
        [Parameter("bool", "soulbound", 6)]
        public virtual bool Soulbound { get; set; }
        [Parameter("tuple[]", "attributes", 7)]
        public virtual List<Attribute> Attributes { get; set; }
        [Parameter("tuple", "owner", 8)]
        public virtual CrossAddress Owner { get; set; }
    }
}
