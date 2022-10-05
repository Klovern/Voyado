namespace IVoyadoManagement.Search
{
    /// <summary>
    ///  Some sort of attempt to make generic (ish)
    /// </summary>
    public interface ISearchable
    {
        public Task<long> FetchMatchResult(IEnumerable<string> searchStrings);
    }
}
