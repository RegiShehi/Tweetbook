using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TweetBook.Contracts.V1;
using TweetBook.Domain;

namespace TweetBook.Tests
{
    [TestFixture]
    public class PostsControllerTests : IntegrationTest
    {
        [Test]
        public async Task GetAll_WithoutAnyPosts_ReturnsEmptyResponse()
        {
            //arrange
            await AutheticateAsync();

            //act
            var response = await TestClient.GetAsync(ApiRoutes.Posts.GetAll);

            //assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            CollectionAssert.IsEmpty(await response.Content.ReadAsAsync<List<Post>>());
        }
    }
}
