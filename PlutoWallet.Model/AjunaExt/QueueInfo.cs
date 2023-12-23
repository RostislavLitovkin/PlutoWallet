using System;
namespace PlutoWallet.Model.AjunaExt
{
    public enum QueueInfoState
    {
        None,
        Ready,
        Future,
        Invalid,
        Dropped,
        InBlock,
        Finalized
    }

    public class QueueInfo
    {
        public QueueInfoState State { get; private set; }

        public string ExtrinsicType { get; }

        public DateTime Created { get; private set; }

        public DateTime LastUpdated { get; private set; }

        public bool IsSuccess { get; private set; }

        public bool IsCompleted { get; private set; }

        public bool IsInBlock { get; private set; }

        public string? BlockHash { get; private set; }

        public double TimeElapsed => DateTime.Now.Subtract(LastUpdated).TotalSeconds;

        public QueueInfo(string extrinsicType)
        {
            ExtrinsicType = extrinsicType;
            Created = DateTime.Now;
            LastUpdated = Created;
            State = QueueInfoState.None;
            IsSuccess = false;
            IsCompleted = false;
            IsInBlock = false;
        }

        internal void Update(QueueInfoState state, string? blockHash = null)
        {
            LastUpdated = DateTime.Now;
            State = state;

            switch (state)
            {
                case QueueInfoState.Invalid:
                    IsCompleted = true;
                    break;

                case QueueInfoState.Dropped:
                    IsCompleted = true;
                    break;

                case QueueInfoState.InBlock:
                    IsInBlock = true;
                    IsSuccess = true;
                    BlockHash = blockHash;
                    break;

                case QueueInfoState.Finalized:
                    IsCompleted = true;
                    IsSuccess = true;
                    BlockHash = blockHash;
                    break;
            }
        }
    }
}

