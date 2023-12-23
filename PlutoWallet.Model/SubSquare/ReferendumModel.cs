using System;
using System.Numerics;
using System.Buffers.Binary;
using System.Collections.Generic;
using Newtonsoft.Json;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Primitive;
using static Substrate.NetApi.Model.Meta.Storage;
using Substrate.NetApi.Generated.Model.sp_core.crypto;
using System.Globalization;
using Substrate.NetApi.Generated.Model.pallet_conviction_voting.vote;
using Substrate.NetApi.Generated.Model.bounded_collections.bounded_vec;
using Substrate.NetApi.Model.Types.Base;
using PlutoWallet.Constants;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWallet.Model.SubSquare
{
	public class ReferendumModel
	{
        public static async Task<List<ReferendumInfo>> GetReferenda(SubstrateClientExt[] groupClients, string substrateAddress)
        {
            CancellationToken token = CancellationToken.None;

            List<ReferendumInfo> results = new List<ReferendumInfo>();

            foreach (var client in groupClients)
            {
                if (client.Endpoint.SubSquareChainName == null)
                {
                    continue;
                }

                var account32 = new AccountId32();
                account32.Create(Utils.GetPublicKeyFrom(substrateAddress));

                var keyBytes = RequestGenerator.GetStorageKeyBytesHash("ConvictionVoting", "VotingFor");

                byte[] prefix = keyBytes.Concat(HashExtension.Hash(Hasher.Twox64Concat, account32.Encode())).ToArray();

                string prefixString = Utils.Bytes2HexString(prefix);

                byte[] startKey = null;

                var keysPaged = await client.State.GetKeysPagedAtAsync(prefix, 1000, startKey, string.Empty, CancellationToken.None);

                List<BigInteger> referendumIds;

                List<string[]> storageChanges = new List<string[]>();

                if (keysPaged == null || !keysPaged.Any())
                {
                    return new List<ReferendumInfo>();
                }
                else
                {
                    var tt = await client.State.GetQueryStorageAtAsync(keysPaged.Select(p => Utils.HexToByteArray(p.ToString())).ToList(), string.Empty, token);
                    storageChanges.AddRange(new List<string[]>(tt.ElementAt(0).Changes));

                    referendumIds = keysPaged.Select(p => HashModel.GetBigIntegerFromTwox_64Concat(p.ToString().Substring(146))).ToList();
                }

                if (storageChanges != null)
                {
                    foreach (var storageChangeSet in storageChanges)
                    {
                        EnumVoting voting = new EnumVoting();
                        voting.Create(storageChangeSet[1]);

                        if ((Voting)voting.Value != Voting.Casting)
                        {
                            Console.WriteLine(voting);
                            continue;
                        }

                        BigInteger _category = HashModel.GetBigIntegerFromTwox_64Concat(storageChangeSet[0].Remove(0, Utils.Bytes2HexString(prefix).Length));

                        foreach (var vote in ((Casting)voting.Value2).Votes.Value.Value)
                        {


                            EnumAccountVote accountVote = ((EnumAccountVote)vote.Value[1]);

                            VoteDecision voteDecision = VoteDecision.Nay; // Default

                            if (accountVote.Value == AccountVote.Standard)
                            {
                                BaseTuple<Vote, U128> voteDetail = (BaseTuple<Vote, U128>)accountVote.Value2;

                                ushort voteValue = ((Vote)voteDetail.Value[0]).Value.Value;

                                if (voteValue > 127)
                                {
                                    voteDecision = VoteDecision.Aye;
                                }

                                Console.WriteLine(((U128)voteDetail.Value[1]).Value);
                            }
                            else if (accountVote.Value == AccountVote.Split)
                            {
                                voteDecision = VoteDecision.Split;
                            }

                            // Subsquare things
                            uint referendumId = ((U32)vote.Value[0]).Value;

                            Root root = await GetReferendumInfo(client.Endpoint, referendumId);

                            Console.WriteLine(root.OnchainData.Tally.Ayes);

                            U128 ayes = new U128();
                            ayes.Create(Utils.HexToByteArray(root.OnchainData.Tally.Ayes).Reverse().ToArray());

                            U128 nays = new U128();
                            nays.Create(Utils.HexToByteArray(root.OnchainData.Tally.Nays).Reverse().ToArray());

                            BigInteger total = ayes.Value + nays.Value;

                            results.Add(new ReferendumInfo
                            {
                                Title = root.Title,
                                Ayes = ayes.Value,
                                Nays = nays.Value,
                                AyesPercentage = (double)(ayes.Value) / (double)total,
                                NaysPercentage = (double)(nays.Value) / (double)total,
                                Vote = new ReferendumVote
                                {
                                    Decision = voteDecision,
                                },
                                ReferendumIndex = root.ReferendumIndex,
                                SubSquareLink = "https://" + client.Endpoint.SubSquareChainName + ".subsquare.io/referenda/" + root.ReferendumIndex,
                                Endpoint = client.Endpoint,
                            });
                        }
                    }
                }
            }

            return results;
        }


        public static async Task<Root> GetReferendumInfo(Endpoint endpoint, uint id)
		{
            HttpClient client = new HttpClient();

            string data = await client.GetStringAsync("https://" + endpoint.SubSquareChainName + ".subsquare.io/api/gov2/referendums/" + id);

            return JsonConvert.DeserializeObject<Root>(data);
        }
    }

    
    public class ReferendumInfo
    {
        public string Title { get; set; }

        public BigInteger Ayes { get; set; }

        public BigInteger Nays { get; set; }

        public double AyesPercentage { get; set; }

        public double NaysPercentage { get; set; }

        public double VoteNay { get; set; }

        public double VoteAye { get; set; }

        public ReferendumVote Vote { get; set; }

        public int ReferendumIndex { get; set; }

        public string SubSquareLink { get; set; }

        public Endpoint Endpoint { get; set; }
    }

    public class ReferendumVote
    {
        public double Amount { get; set; }
        public string Symbol { get; set; }
        public VoteDecision Decision { get; set; }
        // Conviction
    }

    public enum VoteDecision
    {
        Aye,
        Nay,
        Split,
    }

    public class Root
    {
        public string Id { get; set; }
        public int ReferendumIndex { get; set; }
        public Indexer Indexer { get; set; }
        public string Proposer { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        public int Track { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime LastActivityAt { get; set; }
        public State State { get; set; }
        public int StateSort { get; set; }
        public bool Edited { get; set; }
        public int PolkassemblyCommentsCount { get; set; }
        public int PolkassemblyId { get; set; }
        public string PolkassemblyPostType { get; set; }
        public string DataSource { get; set; }
        public bool IsBoundDiscussion { get; set; }
        public OnchainData OnchainData { get; set; }
        public RefToPost RefToPost { get; set; }
        public RootPost RootPost { get; set; }
        public Author Author { get; set; }
        public int CommentsCount { get; set; }
        public List<string> Authors { get; set; }
        public List<object> Reactions { get; set; }
    }

    public class Argument
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public object Value { get; set; }
        public string Proposer { get; set; }
        public int ReferendumIndex { get; set; }
        public int Track { get; set; }
        public Proposal Proposal { get; set; }
        public string ProposalHash { get; set; }
        public Tally Tally { get; set; }
        public string Who { get; set; }
        public string Amount { get; set; }
    }

    public class Author
    {
        public string Username { get; set; }
        public string PublicKey { get; set; }
        public string EmailMd5 { get; set; }
        public string Address { get; set; }
    }

    public class Call
    {
        public string CallIndex { get; set; }
        public string Section { get; set; }
        public string Method { get; set; }
        public List<Argument> Args { get; set; }
    }

    public class AnotherCall
    {
        public string CallIndex { get; set; }
        public string Section { get; set; }
        public string Method { get; set; }
        public List<Argument> Args { get; set; }
    }

    public class Deciding
    {
        public int Since { get; set; }
        public object Confirming { get; set; }
    }

    public class DecisionDeposit
    {
        public string Who { get; set; }
        public long Amount { get; set; }
    }

    public class Enactment
    {
        public int After { get; set; }
    }

    public class Indexer
    {
        public int BlockHeight { get; set; }
        public string BlockHash { get; set; }
        public long BlockTime { get; set; }
        public int EventIndex { get; set; }
        public int ExtrinsicIndex { get; set; }
    }

    public class Info
    {
        public int Track { get; set; }
        public Origin Origin { get; set; }
        public Proposal Proposal { get; set; }
        public Enactment Enactment { get; set; }
        public int Submitted { get; set; }
        public SubmissionDeposit SubmissionDeposit { get; set; }
        public DecisionDeposit DecisionDeposit { get; set; }
        public Deciding Deciding { get; set; }
        public Tally Tally { get; set; }
        public bool InQueue { get; set; }
        public List<object> Alarm { get; set; }
    }

    public class LinearDecreasing
    {
        public int Length { get; set; }
        public int Floor { get; set; }
        public int Ceil { get; set; }
    }

    public class Lookup
    {
        public string Hash { get; set; }
        public int Len { get; set; }
    }

    public class MinApproval
    {
        public LinearDecreasing LinearDecreasing { get; set; }
    }

    public class MinSupport
    {
        public Reciprocal Reciprocal { get; set; }
    }

    public class OnchainData
    {
        public string _id { get; set; }
        public int ReferendumIndex { get; set; }
        public Indexer Indexer { get; set; }
        public int Track { get; set; }
        public string Proposer { get; set; }
        public List<string> Authors { get; set; }
        public Proposal Proposal { get; set; }
        public State State { get; set; }
        public Info Info { get; set; }
        public Tally Tally { get; set; }
        public List<object> WhitelistedHashes { get; set; }
        public List<object> WhitelistDispatchedHashes { get; set; }
        public bool IsFinal { get; set; }
        public string ProposalHash { get; set; }
        public List<object> TreasuryBounties { get; set; }
        public bool IsTreasury { get; set; }
        public TreasuryInfo TreasuryInfo { get; set; }
        public TrackInfo TrackInfo { get; set; }
        public List<Timeline> Timeline { get; set; }
    }

    public class Origin
    {
        public string Origins { get; set; }
    }

    public class Reciprocal
    {
        public int Factor { get; set; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }
    }

    public class RefToPost
    {
        public string PostId { get; set; }
        public string PostType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class RootPost
    {
        public string PostId { get; set; }
        public string PostType { get; set; }
        public List<string> Authors { get; set; }
    }

    public class State
    {
        public string Name { get; set; }
        public Indexer Indexer { get; set; }
    }

    public class SubmissionDeposit
    {
        public string Who { get; set; }
        public long Amount { get; set; }
    }

    public class Tally
    {
        public string Ayes { get; set; }
        public string Nays { get; set; }
        public string Support { get; set; }
        public string Electorate { get; set; }
    }

    public class Timeline
    {
        public string Id { get; set; }
        public int ReferendumIndex { get; set; }
        public Indexer Indexer { get; set; }
        public string Name { get; set; }
        public Argument Args { get; set; } // Assuming 'Args' is of type 'Argument'
    }

    public class TrackInfo
    {
        public string _id { get; set; }
        public int Id { get; set; }
        public int ConfirmPeriod { get; set; }
        public string DecisionDeposit { get; set; }
        public int DecisionPeriod { get; set; }
        public int MaxDeciding { get; set; }
        public MinApproval MinApproval { get; set; }
        public int MinEnactmentPeriod { get; set; }
        public MinSupport MinSupport { get; set; }
        public string Name { get; set; }
        public int PreparePeriod { get; set; }
    }

    public class TreasuryInfo
    {
        public List<Call> Calls { get; set; }
        public string Amount { get; set; }
        public string Beneficiary { get; set; }
        public List<string> Beneficiaries { get; set; }
    }

    public class Proposal
    {
        public Call Call { get; set; }
        public bool Shorten { get; set; }
        public Indexer Indexer { get; set; }
        public Lookup Lookup { get; set; }
    }
}

