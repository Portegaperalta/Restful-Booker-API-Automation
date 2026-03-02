namespace RestfulBooker.Tests;

using RestSharp;
using Xunit;
using FluentAssertions;
using Models;
using RestSharp.Authenticators;
using models;
using Xunit.Abstractions;

public class RestfulBookerAPITests
{
  private readonly string _baseUrl = "https://restful-booker.herokuapp.com";
  private readonly string _authUrl;
  private readonly string _bookingUrl;
  private readonly string _testUsername = "admin";
  private readonly string _testUserPassword = "password123";
  private readonly ITestOutputHelper _outputHelper;

  public RestfulBookerAPITests(ITestOutputHelper outputHelper)
  {
    _authUrl = $"{_baseUrl}/auth";
    _bookingUrl = $"{_baseUrl}/booking";
    _outputHelper = outputHelper;
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

  [Fact]
  public async Task GetBookingIds_ReturnsListOfBookingIds()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var request = CreateGetRequest("booking");

    // Act
    var response = await client.ExecuteAsync<List<BookingId>>(request);

    // Assert
    var data = response.Data;

    data.Should().NotBeNull();
    data.Should().BeOfType<List<BookingId>>();
  }

  [Fact]
  public async Task GetBookingIds_ReturnsStatusCode200()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var request = CreateGetRequest("booking");

    // Act
    var response = await client.ExecuteAsync<List<BookingId>>(request);

    // Assert
    response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
  }

  [Fact]
  public async Task GetBookingById_ReturnsValidBooking()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var request = CreateGetRequest("booking/{id}", "816");

    // Act
    var response = await client.ExecuteAsync<Booking>(request);

    // Assert
    var data = response.Data;

    data.Should().NotBeNull();
    data.Should().BeOfType<Booking>();
  }

  [Fact]
  public async Task GetBookingById_ReturnsStatusCode200Ok()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var request = CreateGetRequest("booking/{id}", "816");

    // Act
    var response = await client.ExecuteAsync<Booking>(request);

    // Assert
    response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
  }

  [Fact]
  public async Task GetBookingById_ReturnsStatusCode404_WhenBookingDoesNotExist()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var request = CreateGetRequest("booking/{id}", "1");

    // Act
    var response = await client.ExecuteAsync<Booking>(request);

    // Assert
    response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
  }

  // Helper Functions
  private RestRequest CreateGetRequest(string endpoint, string resourceId = "")
  {
    var request = new RestRequest(endpoint, Method.Get);

    if (string.IsNullOrEmpty(resourceId) is not true)
    {
      request.AddUrlSegment("id", resourceId);
    }
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
    var client = new RestClient(options);

    client.AddDefaultHeader("Accept", "application/json");
    return client;
  }

  private RestClientOptions CreateRestClientOptions(string baseUrl)
  {
    return new RestClientOptions(baseUrl);
  }
}