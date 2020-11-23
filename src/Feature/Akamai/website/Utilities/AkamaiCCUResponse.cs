namespace Helixbase.Feature.Akamai.Akamai.Utilities
{
    public class AkamaiCCUResponse
    {
        public string describedBy { get; set; }

        public string detail { get; set; }

        public int estimatedSeconds { get; set; }

        public int httpStatus { get; set; }

        public string purgeId { get; set; }

        public string supportId { get; set; }

        public string title { get; set; }
    }
}