namespace RepoZ.Api.Git;

using System;
using System.Collections.Generic;

public class RepositorySeparatorAction : RepositoryAction/*, RepositoryActionBase*/
{
    public RepositorySeparatorAction() : base(string.Empty /*todo*/)
    {
    }
}

public class RepositoryAction : RepositoryActionBase
{
    public RepositoryAction(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public Action<object?, object>? Action { get; set; }

    public bool ExecutionCausesSynchronizing { get; set; }

    public bool CanExecute { get; set; } = true;

    public Func<RepositoryAction[]>? DeferredSubActionsEnumerator { get; set; }

    public IEnumerable<RepositoryAction>? SubActions { get; set; }
}

public abstract class RepositoryActionBase
{
}