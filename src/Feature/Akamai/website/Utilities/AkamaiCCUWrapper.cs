using Newtonsoft.Json;
using Helixbase.Feature.Akamai.EdgeGridAuth;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;

namespace Helixbase.Feature.Akamai.Akamai.Utilities
{
    public class AkamaiCCUWrapper
    {
        private const int maxBodySize = 2048;
        private readonly static string _accessToken = Sitecore.Configuration.Settings.GetSetting("Akamai.AccessToken");
        private readonly static string _clientToken = Sitecore.Configuration.Settings.GetSetting("Akamai.ClientToken");
        private readonly static string _secret = Sitecore.Configuration.Settings.GetSetting("Akamai.Secret");
        private readonly static string _apiUrl = Sitecore.Configuration.Settings.GetSetting("Akamai.ApiUrl");

        #region Public Properties

        public static string clientToken { get; set; }

        public static string accessToken { get; set; }

        public static string secret { get; set; }

        public static string apiUrl { get; set; }

        public static string invalidateByUrlEndpoint { get; set; }

        public static string invalidateByCpCodeEndpoint { get; set; }

        public static string invalidateByCacheTagEndpoint { get; set; }

        public static string deleteByUrlEndpoint { get; set; }

        public static string deleteByCpCodeEndpoint { get; set; }

        public static string deleteByCacheTagEndpoint { get; set; }

        #endregion Public Properties

        #region Constructor

        public AkamaiCCUWrapper()
        {
            accessToken = _accessToken;
            clientToken = _clientToken;
            secret = _secret;
            apiUrl = _apiUrl;
            invalidateByUrlEndpoint = "/ccu/v3/invalidate/url/";
            invalidateByCpCodeEndpoint = "/ccu/v3/invalidate/cpcode/";
            invalidateByCacheTagEndpoint = "/ccu/v3/invalidate/tag/";
            deleteByUrlEndpoint = "/ccu/v3/delete/url/";
            deleteByCpCodeEndpoint = "/ccu/v3/delete/cpcode/";
            deleteByCacheTagEndpoint = "/ccu/v3/delete/tag/";
        }

        #endregion Constructor

        #region Invalidate Methods

        /// <summary>
        /// Invalidates content on the selected URL for the selected network. You should consider invalidating content by default. This keeps each object in cache until the version on your origin server is newer. Deletion retrieves the object regardless, which can dramatically increase the load on your origin server, and would prevent Akamai from serving the old content if your origin is unreachable.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <seealso cref="https://developer.akamai.com/api/purge/ccu/resources.html#postinvalidateurl"/>
        public AkamaiCCUResponse InvalidateByUrl(AkamaiCCUUrlRequest request)
        {
            return ExecuteRequest(request, invalidateByUrlEndpoint);
        }

        /// <summary>
        /// Invalidates content on the selected CP code for the selected network. You should consider invalidating content by default. This keeps each object in cache until the version on your origin server is newer. Deletion retrieves the object regardless, which can dramatically increase the load on your origin server, and would prevent Akamai from serving the old content if your origin is unreachable.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <seealso cref="https://developer.akamai.com/api/purge/ccu/resources.html#postinvalidatecpcode"/>
        public AkamaiCCUResponse InvalidateByCpCode(AkamaiCCURequest request)
        {
            return ExecuteRequest(request, invalidateByCpCodeEndpoint);
        }

        /// <summary>
        /// Invalidates content on the selected set of cache tags for the selected network. You should consider invalidating content by default. This keeps each object in cache until the version on your origin server is newer. Deletion retrieves the object regardless, which can dramatically increase the load on your origin server, and would prevent Akamai from serving the old content if your origin is unreachable. Invalidate by Cache Tag is available to select beta customers only.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <seealso cref="https://developer.akamai.com/api/purge/ccu/resources.html#postinvalidatebycachetag"/>
        public AkamaiCCUResponse InvalidateByCacheTag(AkamaiCCURequest request)
        {
            return ExecuteRequest(request, invalidateByCacheTagEndpoint);
        }

        #endregion Invalidate Methods

        #region Delete Methods

        /// <summary>
        /// Deletes content on the selected URL for the selected network. In most cases, you should invalidate rather than delete content.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <seealso cref="https://developer.akamai.com/api/purge/ccu/resources.html#postdeleteurl"/>
        public AkamaiCCUResponse DeleteByUrl(AkamaiCCUUrlRequest request)
        {
            return ExecuteRequest(request, deleteByUrlEndpoint);
        }

        /// <summary>
        /// Deletes content on the selected CP code for the selected network. In most cases, you should invalidate rather than delete content.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <seealso cref="https://developer.akamai.com/api/purge/ccu/resources.html#postdeletecpcode"/>
        public AkamaiCCUResponse DeleteByCpCode(AkamaiCCURequest request)
        {
            return ExecuteRequest(request, deleteByCpCodeEndpoint);
        }

        /// <summary>
        /// Deletes content on the selected set of cache tags for the selected network. In most cases, you should invalidate rather than delete content. Delete by Cache Tag is available to select beta customers only.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <seealso cref="https://developer.akamai.com/api/purge/ccu/resources.html#postdeletebycachetag"/>
        public AkamaiCCUResponse DeleteByCacheTag(AkamaiCCURequest request)
        {
            return ExecuteRequest(request, deleteByCacheTagEndpoint);
        }

        #endregion Delete Methods

        #region Utility Methods

        private AkamaiCCUResponse ExecuteRequest<T>(T request, string endpointUrl) where T : AkamaiCCURequest
        {
            EdgeGridV1Signer signer = new EdgeGridV1Signer(null, maxBodySize);
            ClientCredential credential = new ClientCredential(clientToken, accessToken, secret);
            var uri = new Uri(apiUrl + endpointUrl.TrimEnd('/') + "/" + request.network.ToString());
            var webRequest = WebRequest.Create(uri);
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });
            webRequest.ContentType = "application/json";
            webRequest.Method = request.method.ToString();
            using (var uploadStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request))))
            {
                using (var resultStream = signer.Execute(webRequest, credential, uploadStream))
                {
                    using (resultStream)
                    {
                        using (var reader = new StreamReader(resultStream))
                        {
                            return JsonConvert.DeserializeObject<AkamaiCCUResponse>(reader.ReadToEnd());
                        }
                    }
                }
            }
        }

        #endregion Utility Methods
    }
}