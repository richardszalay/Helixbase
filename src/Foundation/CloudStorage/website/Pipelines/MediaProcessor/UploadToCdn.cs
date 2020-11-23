﻿using System.Diagnostics;
using System.Linq;
using Helixbase.Foundation.CloudStorage.Constants;
using Helixbase.Foundation.CloudStorage.Interface;
using Helixbase.Foundation.CloudStorage.Provider;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.IO;
using Sitecore.SecurityModel;

namespace Helixbase.Foundation.CloudStorage.Pipelines.MediaProcessor
{
    /// <summary>
    /// Uploads media item to azure cloud storage
    /// </summary>
    public class UploadToCdn
    {
        ICloudStorageProvider cloudStorage;

        public UploadToCdn()
        {
            cloudStorage = new CloudStorageProvider();
        }

        public void Process(MediaProcessorArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Log.Debug("Processing file upload to CDN", this);
            var sw = new Stopwatch();
            sw.Start();

            foreach (Item file in args.UploadedItems.Where(file => file.Paths.IsMediaItem))
            {
                /* NOTE: We don't deal with versioned files, should prepend file.Language and file.Version... */

                // delete if previously uploaded
                if (MainUtil.GetBool(file[FieldNameConstants.MediaItem.UploadedToCloud], false))
                    cloudStorage.Delete(file);

                // upload to CDN
                string filename = cloudStorage.Put(file);

                // delete the existing file from disk
                FileUtil.Delete(StringUtil.EnsurePrefix('/', file[FieldNameConstants.MediaItem.FilePath]));

                // update the item file location to CDN
                using (new EditContext(file, SecurityCheck.Disable))
                {
                    file[FieldNameConstants.MediaItem.FilePath] = filename;
                    file[FieldNameConstants.MediaItem.UploadedToCloud] = "1";
                }
            }

            sw.Stop();
            Log.Debug("File Upload process to CDN complete: " + sw.Elapsed, this);
        }
    }
}
