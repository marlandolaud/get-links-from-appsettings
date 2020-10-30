using Project1;
using NUnit.Framework;

//https://stackoverflow.com/questions/39791634/read-appsettings-json-values-in-net-core-test-project
namespace UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            UriNameHelper.CleanupUriName("");
        }
    }
}