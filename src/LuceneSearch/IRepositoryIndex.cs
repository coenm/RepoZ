namespace LuceneSearch;

using System.Diagnostics.CodeAnalysis;
using Lucene.Net.Search;

internal interface IRepositoryIndex
{
    Task<bool> ReIndexMediaFileAsync(RepositorySearchModel data);

    int Count(Query query = null, Filter filter = null);

    RepositorySearchResult Search(Guid guid);

    List<RepositorySearchResult> Search(string queryString, out int totalHits);

    List<RepositorySearchResult> Search(Query query, Filter filter, out int totalHits);
    void RemoveFromIndexByPath([NotNull] string path);
    void FlushAndCommit();
}