namespace RestfulBooker.Tests;

using RestSharp;
using Xunit;
using FluentAssertions;
using Models;
using RestSharp.Authenticators;

public class RestfulBookerAPITests
{
  private readonly string _baseUrl = "https://restful-booker.herokuapp.com";
  private readonly string _authUrl;
  private readonly string _bookingUrl;
  private readonly string _testUsername = "admin";
  private readonly string _testUserPassword = "password123";

  public RestfulBookerAPITests()
  {
    _authUrl = $"{_baseUrl}/auth";
    _bookingUrl = $"{_baseUrl}/booking";
  }

  [Fact]
  public async Task CreateToken_ReturnsValidAuthToken()
  {
    //Arrange
    var client = CreateRestClient(_baseUrl);

    var authRequest = new AuthRequest
    {
      Username = "admin",
      Password = "password123"
    };

    var request = CreatePostRequest("auth");
    request.AddJsonBody(authRequest);

    //Act
    var response = await client.ExecuteAsync<AuthResponse>(request);

    //Assert
    var data = response.Data;

    data.Should().NotBeNull();
    data.Should().BeOfType<AuthResponse>();
  }

  [Fact]
  public async Task CreateToken_ReturnsStatusCode200_WhenCredentialsAreValid()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);

    var authRequest = new AuthRequest
    {
      Username = "admin",
      Password = "password123"
    };

    var request = CreatePostRequest("auth");
    request.AddJsonBody(authRequest);

    // Act
    var response = await client.ExecuteAsync<AuthResponse>(request);

    // Assert
    response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
  }

  [Fact]
  public async Task CreateToken_ReturnsStatusCode401_WhenCredentialsAreInvalid()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);

    var authRequest = new AuthRequest
    {
      Username = "invalidUser",
      Password = "WrongPassword123"
    };

    var request = CreatePostRequest("auth");
    request.AddJsonBody(authRequest);

    // Act
    var response = await client.ExecuteAsync<AuthResponse>(request);

    // Assert
    response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
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