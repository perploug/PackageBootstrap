// -----------------------------------------------------------------------
// <copyright file="TaskManagerBase.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PackageBootstrap.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using System.Collections.Concurrent;
using PackageBootstrap.Interfaces;
    using System.Threading.Tasks;
    using PackageBootstrap.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TaskManagerBase
    {
        private BlockingCollection<ITask> tasks = new BlockingCollection<ITask>();
        private BlockingCollection<ITask> secondaryTasks = new BlockingCollection<ITask>();

        public ConcurrentDictionary<Guid, TaskEntry> QueuedTasks = new ConcurrentDictionary<Guid, TaskEntry>();
        public ConcurrentDictionary<Guid, TaskEntry> ProcessedTasks = new ConcurrentDictionary<Guid, TaskEntry>();

        public ConcurrentDictionary<Guid, ITask> CurrentTasks = new ConcurrentDictionary<Guid, ITask>();

        private int m_NumberOfWorkers = 5;
        public int NumberOfWorkers
        {
            get { return m_NumberOfWorkers; }
            set { m_NumberOfWorkers = value; }

        }

        public TaskManagerBase()
        {
            //our main queue of tasks, this handles all the high-priority stuff, which includes database calls.
            //the system is currently limited in regards to having multiple database sessions running
            //so we have to stick with having just one (and having multiples whould also cause some problems if data is changed during packaging)

            for (int i = 0; i < m_NumberOfWorkers; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    foreach (ITask value in tasks.GetConsumingEnumerable())
                    {
                        //Helpers.Logging._Debug("Starting executing task with id" + value.UniqueId.ToString() + " type: " + value.GetType().ToString());
                        CurrentTasks.TryAdd(value.UniqueId, value);
                        manageTask(value);
                    }
                }, TaskCreationOptions.LongRunning);
            }


            /*
            Task.Factory.StartNew(() =>
            {
                foreach (ITask value in secondaryTasks.GetConsumingEnumerable())
                {
                    //Helpers.Logging._Debug("Starting executing task with id" + value.UniqueId.ToString() + " type: " + value.GetType().ToString());
                    CurrentTasks.TryAdd(value.UniqueId, value);
                    manageTask(value);
                }
            }, TaskCreationOptions.LongRunning);
             * */
        }

        private void manageTask(ITask value)
        {
            StartingTask.Raise(this, new TaskmanagerEventArgs() { Task = value });

            TaskEntry entry;
            if (QueuedTasks.TryGetValue(value.UniqueId, out entry))
            {
                if (entry.State == "pending")
                {
                    Exception exception = null;
                    try
                    {
                        CurrentTasks.TryAdd(value.UniqueId, value);

                        TaskEntry s;
                        QueuedTasks.TryRemove(value.UniqueId, out s);

                        StartedTask.Raise(this, new TaskmanagerEventArgs() { Task = value });
                        value.Run();
                    }
                    catch (Exception ex)
                    {
                        exception = ex;

                        Log.Error(ex.ToString());

                        TaskEntry s;
                        QueuedTasks.TryRemove(value.UniqueId, out s);

                        s.State = "error";
                        s.Message = ex.ToString() + "\n " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();


                        s.UniqueId = value.UniqueId;
                        s.Description = value.TaskDescription; ;
                        s.Name = value.Name;
                        s.TimeStamp = value.TimeStamp;

                        ProcessedTasks.TryAdd(s.UniqueId, s);

                        ITask irt;
                        CurrentTasks.TryRemove(s.UniqueId, out irt);
                    }
                    finally
                    {
                        var ea = new TaskmanagerEventArgs() { Task = value, Exception = exception };
                        EndingTask.Raise(this, ea);

                        TaskEntry s;
                        QueuedTasks.TryRemove(value.UniqueId, out s);

                        s.State = "completed";
                        s.Message = "Completed on: " + DateTime.Now.ToString();

                        s.UniqueId = value.UniqueId;
                        s.Name = value.Name;
                        s.Description = value.TaskDescription;

                        s.TimeStamp = value.TimeStamp;

                        ProcessedTasks.TryAdd(s.UniqueId, s);

                        ITask irt;
                        CurrentTasks.TryRemove(s.UniqueId, out irt);

                        if (exception == null)
                            value.End();

                        EndedTask.Raise(this, ea);
                        value.Dispose();
                    }
                }
                else
                {
                    TaskEntry s;
                    QueuedTasks.TryRemove(value.UniqueId, out s);

                    ProcessedTasks.TryAdd(s.UniqueId, s);
                    value.Cancel();
                }
            }
        }

        void value_OnStatusChange(object sender, TaskEventArgs e)
        {
            UpdatedTask.Raise(sender, e);
        }

        public void Add(ITask task)
        {
            var ea = new TaskmanagerEventArgs() { Task = task };
            AddingTask.Raise(this, ea);

            var t = task;
            t.TimeStamp = DateTime.Now;
            t.UniqueId = Guid.NewGuid();

            TaskEntry te = new TaskEntry();
            te.State = "pending";
            te.UniqueId = t.UniqueId;
            te.User = "admin";
            te.TimeStamp = t.TimeStamp;
            te.Name = t.Name;

            te.Message = "Awaiting processing.";

            te.Description = task.TaskDescription;

            //Display of queued items:
            QueuedTasks.TryAdd(t.UniqueId, te);

            t.OnStatusChange += new EventHandler<TaskEventArgs>(value_OnStatusChange);

            //Processing the tasks:
            if (t.ByPassQueue)
            {
                if (secondaryTasks.TryAdd(t))
                    AddedTask.Raise(this, ea);
            }
            else
            {
                if (tasks.TryAdd(t))
                    AddedTask.Raise(this, ea);
            }
        }

        public void Cancel(Guid taskId)
        {
            if (CurrentTasks.ContainsKey(taskId))
            {
                var ea = new TaskmanagerEventArgs() { Task = new TaskBase() { UniqueId = taskId } };

                CancellingTask(this, ea);

                var value = CurrentTasks[taskId];
                TaskEntry s = new TaskEntry();

                s.State = "cancelled";
                s.Message = "Cancelled by user";

                s.UniqueId = value.UniqueId;
                s.Description = value.TaskDescription; ;
                s.Name = value.Name;
                s.TimeStamp = value.TimeStamp;
                ProcessedTasks.TryAdd(s.UniqueId, s);

                value.Cancel();

                ITask irt;
                if (CurrentTasks.TryRemove(s.UniqueId, out irt))
                {
                    ea.Task = irt;
                    CancelledTask(this, ea);
                    irt.Dispose();
                }
            }
            else if (QueuedTasks.ContainsKey(taskId))
            {
                TaskEntry te;
                if (QueuedTasks.TryRemove(taskId, out te))
                {
                    te.State = "cancelled";
                    te.Message = "Cancelled by user";
                    ProcessedTasks.TryAdd(te.UniqueId, te);
                }
            }
        }

        public event EventHandler<TaskmanagerEventArgs> AddingTask;
        public event EventHandler<TaskmanagerEventArgs> AddedTask;

        public event EventHandler<TaskmanagerEventArgs> CancellingTask;
        public event EventHandler<TaskmanagerEventArgs> CancelledTask;

        public event EventHandler<TaskmanagerEventArgs> StartingTask;
        public event EventHandler<TaskmanagerEventArgs> StartedTask;

        public event EventHandler<TaskmanagerEventArgs> EndingTask;
        public event EventHandler<TaskmanagerEventArgs> EndedTask;

        public event EventHandler<TaskEventArgs> UpdatedTask;


        public struct TaskEntry
        {
            public string State { get; set; }
            public Guid UniqueId { get; set; }
            public string User { get; set; }
            public DateTime TimeStamp { get; set; }

            public string Name { get; set; }
            public string Description { get; set; }

            public string Message { get; set; }
            public int Progress { get; set; }
        }
    }
}
