﻿namespace Helixbase.Feature.Sitecron.Core.Jobs
{
    public interface ISitecronJobValidator
    {
        bool IsValid(SitecronJob job);
    }
}
