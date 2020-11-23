using Newtonsoft.Json;

namespace Helixbase.Feature.Akamai.Akamai.Utilities
{
    public enum AkamaiNetwork
    {
        staging,
        production
    }

    public enum AkamaiRequestMethod
    {
        POST,
        GET
    }

    public class AkamaiCCURequest
    {
        public AkamaiCCURequest()
        {
            //network = AkamaiNetwork.staging;
            method = AkamaiRequestMethod.POST;
        }

        public string[] objects { get; set; }

        [JsonIgnore]
        public AkamaiNetwork network { get; set; }

        [JsonIgnore]
        public AkamaiRequestMethod method { get; set; }
    }
}