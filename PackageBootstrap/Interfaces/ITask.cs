// -----------------------------------------------------------------------
// <copyright file="ITask.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PackageBootstrap.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITask : IDisposable
    {
        string Name { get; set; }
        string TaskDescription { get; set; }

        DateTime TimeStamp { get; set; }

        int TotalActions { get; set; }
        int CompletedActions { get; set; }

        string CurrentAction { get; set; }

        Guid UniqueId { get; set; }

        bool ByPassQueue { get; set; }

        void Cancel();
        void Run();
        void End();
        void ExecuteOnEnd(Action<ITask> SomeMethod);

        event EventHandler<TaskEventArgs> OnStatusChange;
    }
}
