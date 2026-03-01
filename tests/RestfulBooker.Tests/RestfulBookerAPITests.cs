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
  public void CreateToken_ReturnsValidAuthToken()
  {

  }

  // Helper Functions

  private RestClient CreateRestClient()
  {
    return new RestClient(_options);
  }

  private RestRequest CreateRestRequest(string url)
  {
    return new RestRequest(url);
  }
}