using Helixbase.Feature.Sitecron.Core.Quartz;
using Sitecore.Pipelines;

namespace Helixbase.Feature.Sitecron
{
    public class InitializeSitecron
    {
        public virtual void Process(PipelineArgs args)
        {
            ScheduleHelper scheduler = new ScheduleHelper();
            scheduler.InitializeScheduler();
        }
    }
}