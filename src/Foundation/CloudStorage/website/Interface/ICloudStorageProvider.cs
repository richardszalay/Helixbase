using Sitecore.Data.Items;

namespace Helixbase.Foundation.CloudStorage.Interface
{
    interface ICloudStorageProvider
    {
        string Put(MediaItem media);
        string Update(MediaItem media);
        bool Delete(MediaItem media);
    }
}
