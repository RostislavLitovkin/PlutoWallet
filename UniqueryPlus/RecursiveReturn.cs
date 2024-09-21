namespace UniqueryPlus
{
    /// <summary>
    /// Think of a better name :)
    /// </summary>
    public class RecursiveReturn<T>
    {
        public byte[]? LastKey { get; set; }
        public required IEnumerable<T> Items { get; set; }
    }
}
