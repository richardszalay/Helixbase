using NUnit.Framework;

namespace Helixbase.Feature.Analytics.Tests
{

    public class Tests
    {
        private IAnalyticsService _analyticsService;

        private AnalyticsController _analyticsController;
        [SetUp]
        public void Setup()
        {
            _analyticsService = Substitute.For<IAnalyticsService>();
            _analyticsController = new AnalyticsController(_analyticsService);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}