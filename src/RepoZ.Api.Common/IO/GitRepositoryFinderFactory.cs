namespace RepoZ.Api.Common.IO
{
    using RepoZ.Api.Common.IO.VoidToolsEverything;
    using RepoZ.Api.IO;

    public class GitRepositoryFinderFactory : IGitRepositoryFinderFactory
    {
        private readonly IPathSkipper _pathSkipper;
        private bool? _isEverytingInstalled;
        private readonly object _lock = new object();

        public GitRepositoryFinderFactory(IPathSkipper pathSkipper)
        {
            _pathSkipper = pathSkipper;
        }

        public IGitRepositoryFinder Create()
        {
            if (UseEverything())
            {
                return new EverythingGitRepositoryFinder(_pathSkipper);
            }
            else
            {
                return new GravellGitRepositoryFinder(_pathSkipper);
            }
        }

        private bool UseEverything()
        {
            if (_isEverytingInstalled.HasValue)
            {
                return _isEverytingInstalled.Value;
            }

            lock (_lock)
            {
                if (!_isEverytingInstalled.HasValue)
                {
                    _isEverytingInstalled = Everything64Api.IsInstalled();
                }
            }

            return _isEverytingInstalled.Value;
        }
    }
}
