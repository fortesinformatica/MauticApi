using MauticApi.Client.Models;
using NUnit.Framework;

namespace MauticApi.Client.Test
{
    [TestFixture]
    public class MauticClientConfigurationTest
    {
        [TestCase("http://test.com")]
        public void WhenCreateAnInstanceWithAPaththatDoesntEndsWithindex_phpAppendItToPath(string path)
        {
            var mauticConfig = new MauticClientConfiguration(path);
            StringAssert.EndsWith("/index.php", mauticConfig.BaseUrl);
        }

        [TestCase("http://test.com/index.php")]
        public void WhenCreateAnInstanceWithAPathThatEndsWith_index_phpDoesntAppendItToPath(string path)
        {
            var mauticConfig = new MauticClientConfiguration(path);
            StringAssert.EndsWith(path, mauticConfig.BaseUrl);
        }
    }
}
