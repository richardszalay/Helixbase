using Sitecore.Data.Items;

namespace Helixbase.Foundation.CloudStorage.Interface
{
    public interface ICloudStorage
    {
        string Put(MediaItem media);
        string Update(MediaItem media);
        bool Delete(string filename);
    }
}
