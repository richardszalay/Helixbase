﻿using System;
using Sitecore.Diagnostics;
using Helixbase.Feature.Sitecron.SitecronSettings;

namespace Helixbase.Feature.Sitecron.Core.Jobs
{
    public class SitecronJobValidator : ISitecronJobValidator
    {
        public bool IsValid(SitecronJob job)
        {
            if (job == null)
            {
                return false;
            }

            if (job.JobType == null)
            {
                Log.Info($"SiteCron - Job Not Loaded - Could not load type '{job.JobTypeSignature}' for job - Job Source: {job.JobSource} - {job.Name} - Job ItemId:{job.ItemId}", this);
                return false;
            }

            if (string.IsNullOrEmpty(job.CronExpression) &&  job.ExecuteExactlyAtDateTime.Value == DateTime.MinValue)
            {
                Log.Info($"SiteCron - Job Not Loaded - Invalid ExecuteExactlyAtDateTime or Cron Expression: Job Source: {job.JobSource} - {job.Name} ExecuteExactlyAtDateTime: {job.ExecuteExactlyAtDateTime.Value} Cron Expression: {job.CronExpression} - Job ItemId:{job.ItemId}", this);
                return false;
            }

            if (!string.IsNullOrEmpty(job.CronExpression) && job.ExecuteExactlyAtDateTime.Value != DateTime.MinValue)
            {
                Log.Info($"SiteCron - Job Not Loaded - Both ExecuteExactlyAtDateTime and Cron Expression specified: Job Source: {job.JobSource} - {job.Name} ExecuteExactlyAtDateTime: {job.ExecuteExactlyAtDateTime.Value} Cron Expression: {job.CronExpression} - Job ItemId:{job.ItemId}", this);
                return false;
            }

            if (job.ExecuteExactlyAtDateTime.Value != DateTime.MinValue && job.ExecuteExactlyAtDateTime.Value.ToUniversalTime() < DateTime.Now.ToUniversalTime())
            {
                Log.Info($"SiteCron - Job Not Loaded - ExecuteExactlyAtDateTime is in the past: Job Source: {job.JobSource} - {job.Name} ExecuteExactlyAtDateTime: {job.ExecuteExactlyAtDateTime.Value} Cron Expression: {job.CronExpression} - Job ItemId:{job.ItemId}", this);
                return false;
            }

            if (job.Disable)
            {
                Log.Info($"SiteCron - Job Not Loaded - Job Disabled: Job Source: {job.JobSource} - {job.Name} Type: {job.JobTypeSignature} Cron Expression: {job.CronExpression} - Job ItemId:{job.ItemId}", this);
                return false;
            }

            //if its a Run As Sitecore Job, validate fields
            if (job.TemplateId == SitecronConstants.Templates.SitecronRunAsSitecoreJobTemplateID && (string.IsNullOrEmpty(job.SitecoreJobCategory) || string.IsNullOrEmpty(job.SitecoreJobMethod) || string.IsNullOrEmpty(job.SitecoreJobName) || string.IsNullOrEmpty(job.SitecoreJobSiteName) || string.IsNullOrEmpty(job.SitecoreJobType) || string.IsNullOrEmpty(job.SitecoreJobPriority)))
            {
                Log.Info($"SiteCron - RunAsSitecoreJob - Job Not Loaded - Job Disabled: Job Source: {job.JobSource} - {job.Name} Type: {job.JobTypeSignature} Cron Expression: {job.CronExpression} - Job ItemId:{job.ItemId}", this);
                return false;
            }

            //if its a SiteCron Sitecore Schedule Command Job, validate fields
            if (job.TemplateId == SitecronConstants.Templates.SiteCronSitecoreScheduleCommandJobTemplateID && (string.IsNullOrEmpty(job.SitecoreJobCategory) || string.IsNullOrEmpty(job.SitecoreJobName) || string.IsNullOrEmpty(job.SitecoreJobSiteName) || string.IsNullOrEmpty(job.SitecoreJobPriority) || string.IsNullOrEmpty(job.SitecoreScheduleJob)))
            {
                Log.Info($"SiteCron - SiteCron Sitecore Schedule Command Job - Job Not Loaded - Job Disabled: Job Source: {job.JobSource} - {job.Name} Type: {job.JobTypeSignature} Cron Expression: {job.CronExpression} - Job ItemId:{job.ItemId}", this);
                return false;
            }
            return true;
        }
    }
}