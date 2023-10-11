using System;
using System.Numerics;
using System.Collections.Generic;
using Newtonsoft.Json;
using Substrate.NetApi.Model.Types.Primitive;

namespace PlutoWallet.Model.SubSquare
{
	public class ReferendumModel
	{
		public static async Task<ReferendumInfo> GetReferendumInfo(int id)
		{
            HttpClient client = new HttpClient();

            string data = await client.GetStringAsync("https://polkadot.subsquare.io/api/gov2/referendums/" + id);

            Root root = JsonConvert.DeserializeObject<Root>(data);

            U128 ayes = new U128();
            ayes.Create(root.OnchainData.Tally.Ayes);

            U128 nays = new U128();
            nays.Create(root.OnchainData.Tally.Nays);

            return new ReferendumInfo
            {
                Title = root.Title,
                Ayes = ayes.Value,
                Nays = nays.Value,
                ReferendumIndex = root.ReferendumIndex,
            };
        }
    }

    public class ReferendumInfo
    {
        public string Title { get; set; }

        public BigInteger Ayes { get; set; }

        public BigInteger Nays { get; set; }

        public int ReferendumIndex { get; set; }
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

