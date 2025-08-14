using Demo.Tests.Base;
using Demo.Tests.Metadata;
using System.Net;
using System.Net.Http;
using Unicorn.Backend.Matchers;
using Unicorn.Backend.Services.RestService;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Demo.Tests.Scenarios.RestService
{
    /// <summary>
    /// Rest web service test suite example.
    /// The test suite should have <see cref="SuiteAttribute"/>
    /// It's possible to specify any number of suite tags and metadata.
    /// </summary>
    [Suite("Dummy Rest Api tests")]
    [Tag(Platforms.Api), Tag(Apps.DummyApi)]
    [Metadata(key: "Description", value: "Tests for dummy rest api functionality")]
    [Metadata(key: "Api link", value: "http://dummy.restapiexample.com")]
    public class DummyRestApiTests : BaseTestSuite
    {
        [Test("Get user by ID")]
        [Author(Authors.ASmithee)]
        public void GetUserByIdTest()
        {
            RestResponse userResponse = Do.DummyRestApi.GetUser(2);

            Do.Assertion.StartAssertionsChain()
                .VerifyThat(userResponse, ApiResponse.HasStatusCode(HttpStatusCode.OK))
                .VerifyThat(userResponse, ApiResponse.Rest.HasTokenWithValue("$.data.id", 2))
                .AssertChain();
        }

        [Test("Get users list")]
        [Author(Authors.ASmithee)]
        public void GetUsersTest()
        {
            RestResponse userResponse = Do.DummyRestApi.GetUsersPage(2);

            Do.Assertion.StartAssertionsChain()
                .VerifyThat(userResponse, ApiResponse.Rest.HasStatusCode(HttpStatusCode.OK))
                .VerifyThat(userResponse, ApiResponse.Rest.HasTokenWithValue("$.page", 2))
                .VerifyThat(userResponse, ApiResponse.Rest.HasTokenWithValue("$.per_page", 6))
                .VerifyThat(userResponse, ApiResponse.Rest.HasTokenWithValue("$.total", 12))
                .VerifyThat(userResponse, ApiResponse.Rest.HasTokenWithValue("$.total_pages", 2))
                .VerifyThat(userResponse, ApiResponse.Rest.HasTokensCount("$.data[*]", 6))
                .AssertChain();
        }

        [Test("Check call to non-existing endpoint returns BadRequest")]
        [Author(Authors.ASmithee)]
        public void SendRequestToNonExistingEndpoint()
        {
            RestResponse response = Do.DummyRestApi
                .SendGeneralRequest(HttpMethod.Post, "/non-existing-endpoint", "data");

            Do.Assertion.AssertThat(
                response,
                ApiResponse.HasStatusCode(HttpStatusCode.BadRequest));
        }
    }
}
