﻿using Newtonsoft.Json;
using RepoZ.Api.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using RepoZ.Api.Git;

namespace RepoZ.Api.Common.Git
{
	public class DefaultRepositoryActionConfigurationStore : FileRepositoryStore, IRepositoryActionConfigurationStore
	{
		private const string REPOSITORY_ACTIONS_FILENAME = "RepositoryActions.json";
		private object _lock = new object();
		private readonly IAppDataPathProvider _appDataPathProvider;

		public DefaultRepositoryActionConfigurationStore(IErrorHandler errorHandler,
			IAppDataPathProvider appDataPathProvider)
			: base(errorHandler)
		{
			_appDataPathProvider = appDataPathProvider ?? throw new ArgumentNullException(nameof(appDataPathProvider));
		}

		public override string GetFileName()
		{
			return Path.Combine(_appDataPathProvider.GetAppDataPath(), REPOSITORY_ACTIONS_FILENAME);
		}

		public RepositoryActionConfiguration LoadRepositoryConfiguration(Repository repo)
		{
			var file = Path.Combine(repo.Path, ".git", REPOSITORY_ACTIONS_FILENAME);
			if (File.Exists(file))
			{
				var result = LoadRepositoryActionConfiguration(file);
				if (result.State == RepositoryActionConfiguration.LoadState.Ok)
				{
					return result;
				}
			}

			file = Path.Combine(repo.Path, REPOSITORY_ACTIONS_FILENAME);
			if (File.Exists(file))
			{
				var result = LoadRepositoryActionConfiguration(file);
				if (result.State == RepositoryActionConfiguration.LoadState.Ok)
				{
					return result;
				}
			}

			return null;
		}

		public void Preload()
		{
			lock (_lock)
			{
				if (!File.Exists(GetFileName()))
				{
					if (!TryCopyDefaultJsonFile())
					{
						RepositoryActionConfiguration = new RepositoryActionConfiguration();
						RepositoryActionConfiguration.State = RepositoryActionConfiguration.LoadState.None;
						return;
					}
				}

				RepositoryActionConfiguration = LoadRepositoryActionConfiguration(GetFileName());
			}
		}

		public RepositoryActionConfiguration LoadRepositoryActionConfiguration(string filename)
		{
			try
			{
				var lines = Get(filename)?.ToList() ?? new List<string>();
				var json = string.Join(Environment.NewLine, lines.Select(RemoveComment));
				var repositoryActionConfiguration = JsonConvert.DeserializeObject<RepositoryActionConfiguration>(json) ?? new RepositoryActionConfiguration();
				repositoryActionConfiguration.State = RepositoryActionConfiguration.LoadState.Ok;
				return repositoryActionConfiguration;
			}
			catch (Exception ex)
			{
				return new RepositoryActionConfiguration
				{
					State = RepositoryActionConfiguration.LoadState.Error,
					LoadError = ex.Message
				};
			}
		}

		private bool TryCopyDefaultJsonFile()
		{
			var defaultFile = Path.Combine(_appDataPathProvider.GetAppResourcesPath(), REPOSITORY_ACTIONS_FILENAME);
			var targetFile = GetFileName();

			try
			{
				File.Copy(defaultFile, targetFile);
			}
			catch
			{
				/* lets ignore errors here, we just want to know if if worked or not by checking the file existence */
			}

			return File.Exists(targetFile);
		}

		private string RemoveComment(string line)
		{
			var indexOfComment = line.IndexOf('#');
			return indexOfComment < 0 ? line : line.Substring(0, indexOfComment);
		}

		public RepositoryActionConfiguration RepositoryActionConfiguration { get; private set; }
	}
}
