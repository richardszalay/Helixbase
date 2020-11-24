namespace Helixbase.Feature.Sitecron.Core
{
    public interface IScheduleManager
    {
        void ScheduleAllJobs();
        void CleanUpExistingJobs();
    }
}
