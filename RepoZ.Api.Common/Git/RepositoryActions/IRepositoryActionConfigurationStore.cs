using RepoZ.Api.Git;

namespace RepoZ.Api.Common.Git
{
	public interface IRepositoryActionConfigurationStore
	{
		void Preload();

		RepositoryActionConfiguration RepositoryActionConfiguration { get; }
        RepositoryActionConfiguration LoadRepositoryConfiguration(Repository path);
    }
}