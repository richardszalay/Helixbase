using System.Collections.Generic;

namespace Helixbase.Feature.Sitecron.Core.Jobs
{
    public interface ISitecronJobProvider
    {
        IEnumerable<SitecronJob> GetJobs();
    }
}
