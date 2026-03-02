namespace RestfulBooker.Tests;

using RestSharp;
using Xunit;
using FluentAssertions;
using RestSharp.Authenticators;

public class RestfulBookerAPITests
{
  private readonly string _baseUrl = "https://restful-booker.herokuapp.com";
  private readonly string _authUrl;
  private readonly string _bookingUrl;
  private readonly string _testUsername = "admin";
  private readonly string _testUserPassword = "password123";
  private readonly RestClientOptions _options;

  public RestfulBookerAPITests()
  {
    _options = new RestClientOptions(_baseUrl)
    {
      Authenticator = new HttpBasicAuthenticator(_testUsername, _testUserPassword)
    };

    _authUrl = $"{_baseUrl}/auth";
    _bookingUrl = $"{_baseUrl}/booking";
  }

  [Fact]
  public async Task CreateToken_ReturnsValidAuthToken()
  {

  }

  // Helper Functions
  private RestRequest CreateGetRequest(string endpoint)
  {
    var request = new RestRequest(endpoint, Method.Get);
    return request;
  }

  private RestRequest CreatePostRequest(string endpoint)
  {
    var request = new RestRequest(endpoint, Method.Post);
    return request;
  }

  private RestRequest CreatePutRequest(string endpoint)
  {
    var request = new RestRequest(endpoint, Method.Put);
    return request;
  }

  private RestRequest CreatePatchRequest(string endpoint)
  {
    var request = new RestRequest(endpoint, Method.Patch);
    return request;
  }

  private RestRequest CreateDeleteRequest(string endpoint)
  {
    var request = new RestRequest(endpoint, Method.Delete);
    return request;
  }

  private RestClient CreateRestClient(string baseUrl)
  {
    var options = CreateRestClientOptions(baseUrl);
    return new RestClient(options);
  }

  private RestClientOptions CreateRestClientOptions(string baseUrl)
  {
    return new RestClientOptions(baseUrl);
  }
}