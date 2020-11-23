using Aliyun.OSS;
using Helixbase.Foundation.CloudStorage.Provider;
using Helixbase.Foundation.Logging.Repositories;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC = Sitecore.Configuration;

namespace Helixbase.Feature.AliStorage
{
    public class AliStorageProvider : CloudStorageBase
    {
        private static OssClient _client;
        private static readonly string _bucketName= SC.Settings.GetSetting("AliStorage.bucketName");
        private static readonly string _accessKeyId = SC.Settings.GetSetting("AliStorage.accessKeyId");
        private static readonly string _accessKeySecret= SC.Settings.GetSetting("AliStorage.accessKeySecret");
        private static readonly string _endpoint= SC.Settings.GetSetting("AliStorage.endpoint");
        private   ILogRepository _logRepository;
        #region ctor
        public AliStorageProvider(ILogRepository logRepository
           )
        {
            _logRepository = logRepository;
            
            this.Initialize();
        }

        private void Initialize()
        {
            _client = new OssClient(_endpoint, _accessKeyId, _accessKeySecret);
        }
        #endregion
        

        #region Implementation
        /// <summary>
        /// Uploads the media file into Azure Storage container
        /// </summary>
        /// <param name="media">Media Item to upload</param>
        /// <returns>Location of file in container</returns>
        public override string Put(MediaItem media)
        {
            string filename = base.ParseMediaFileName(media);
         
        



            using (Stream fileStream = media.GetMediaStream())
            {
                using (var metadata = _client.PutObject(_bucketName, filename, fileStream))
                {
                    var etag = metadata.ETag;
                 
                }
            }

            _logRepository.Info("File successfully uploaded to  Ali OSS Storage: " + filename);

            return filename;
        }

        /// <summary>
        /// Overrides the existing media item with this new one
        /// </summary>
        /// <param name="media">Media Item to upload</param>
        /// <returns>Location of file in container</returns>
        public override string Update(MediaItem media)
        {
            return Put(media);
        }

        /// <summary>
        /// Delete the associated media file from Azure storage
        /// </summary>
        /// <param name="filename">Location fo file to delete in storage</param>
        /// <returns>Bool indicating success</returns>
        public override bool Delete(string filename)
        {
               var result = _client.DeleteObject(_bucketName, filename);
            if (result.DeleteMarker)
            {
                _logRepository.Info("File successfully deleted from  Ali OSS Storage: " + filename);
                return true;
            }
            else
            {
                _logRepository.Info("File failed deleted from  Ali OSS Storage: " + filename);
                return false;
            }
        }
        #endregion
    }
}
