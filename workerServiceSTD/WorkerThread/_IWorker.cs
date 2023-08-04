
namespace WorkerThread {
    public interface _IWorker
    {
        void DoWork(object anObject);
    }

    public enum WorkerState
    {
        Starting = 0,
        Running,
        Paused,
        Stopping,
        Stopped,
        Faulted
    }
}
