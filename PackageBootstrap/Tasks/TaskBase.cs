// -----------------------------------------------------------------------
// <copyright file="TaskBase.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PackageBootstrap.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PackageBootstrap.Interfaces;
    using PackageBootstrap.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TaskBase : ITask
    {
        private string m_name = string.Empty;
        public virtual string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(m_name))
                    return m_name;
                return this.GetType().Name;
            }
            set
            {
                m_name = value;
            }
        }

        public string TaskDescription { get; set; }

        public DateTime TimeStamp { get; set; }

        public int TotalActions { get; set; }

        private int m_CompletedActions = 0;
        public int CompletedActions
        {
            get { return m_CompletedActions; }
            set
            {
                if (value > m_CompletedActions)
                {
                    m_CompletedActions = value;

                    TaskEventArgs tea = new TaskEventArgs();
                    OnStatusChange.Raise(this, tea);
                }
            }
        }

        private string m_CurrentAction = "Initializing...";
        public string CurrentAction
        {
            get { return m_CurrentAction; }
            set
            {
                m_CurrentAction = value;

                TaskEventArgs tea = new TaskEventArgs();
                OnStatusChange.Raise(this, tea);
            }
        }

        public Guid UniqueId { get; set; }
        public bool ByPassQueue { get; set; }

        public virtual void Cancel()
        {
            throw new NotImplementedException();
        }

        public virtual void Run()
        {
            throw new NotImplementedException();
        }

        public virtual void End()
        {
            if (m_EndAction != null)
                m_EndAction.Invoke(this);
        }

        public event EventHandler<TaskEventArgs> OnStatusChange;

        public void Dispose()
        {

        }

        private Action<ITask> m_EndAction = null;
        public void ExecuteOnEnd(Action<ITask> SomeMethod)
        {
            m_EndAction = SomeMethod;
        }
    }
}
