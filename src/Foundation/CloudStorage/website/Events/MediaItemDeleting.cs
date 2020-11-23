﻿using System;
using Helixbase.Foundation.CloudStorage.Interface;
using Helixbase.Foundation.CloudStorage.Provider;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;

namespace Helixbase.Foundation.CloudStorage.Events
{
    public class MediaItemDeleting
    {
        ICloudStorageProvider cloudStorage;

        public MediaItemDeleting()
        {
            cloudStorage = new CloudStorageProvider();
        }

        public void OnItemDeleting(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            Item item = Event.ExtractParameter(args, 0) as Item;
            DeleteAzureMediaBlob(item);
        }

        private void DeleteAzureMediaBlob(Item item)
        {
            if (!item.Paths.IsMediaItem)
                return;

            var media = new MediaItem(item);
            if (media.FileBased)
            {
                cloudStorage.Delete(item);
            }
        }
    }
}
