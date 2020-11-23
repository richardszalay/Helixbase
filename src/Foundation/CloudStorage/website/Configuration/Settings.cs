﻿using Helixbase.Foundation.CloudStorage.Constants;
using Sitecore;
using SC = Sitecore.Configuration;

namespace Helixbase.Foundation.CloudStorage.Configuration
{
    public class Settings
    {
        public static string CloudMediaUrl
        {
            get
            {
                return StringUtil.EnsurePostfix('/', SC.Settings.GetSetting(ConfigSettings.MediaLinkCdnServerUrl));
            }
        }

        public static string MediaThumbnailCacheFolder
        {
            get
            {
                return StringUtil.EnsurePostfix('/', SC.Settings.GetSetting(ConfigSettings.MediaThumbnailCacheFolder, "/App_Data/MediaThumbnailCache"));
            }
        }

        public static bool AlwaysIncludeCdnServerUrl
        {
            get
            {
                return SC.Settings.GetBoolSetting(ConfigSettings.AlwaysIncludeCdnServerUrl, false);
            }
        }
    }
}
