namespace RestfulBooker.Tests;

using RestSharp;
using Xunit;
using FluentAssertions;
using RestSharp.Authenticators;

public class RestfulBookerAPITests
{
  private readonly string baseUrl = "https://restful-booker.herokuapp.com";
  private readonly string testUsername = "admin";
  private readonly string testUserPassword = "password123";
  private readonly RestClientOptions _options;

  public RestfulBookerAPITests()
  {
    _options = new RestClientOptions(baseUrl)
    {
      Authenticator = new HttpBasicAuthenticator(testUsername, testUserPassword)
    };
  }

  [Fact]
  public void CreateToken_ReturnsValidAuthToken()
  {

  }
}