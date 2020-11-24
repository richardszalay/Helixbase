using Helixbase.Feature.Sitecron.Core.Jobs;

namespace Helixbase.Feature.Sitecron.Core.Scheduling
{
    public interface ISitecronScheduler
    {
        void ClearJobs();

        void ScheduleJob(SitecronJob job);
    }
}
