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
  private readonly ITestOutputHelper _outputHelper;

  public RestfulBookerAPITests(ITestOutputHelper outputHelper)
  {
    _outputHelper = outputHelper;
  }

  // Auth - CreateToken Tests
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

  // Booking - GetBookingIds Tests
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

  // Booking - GetBooking Tests
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

  // Booking - CreateBooking Tests
  [Fact]
  public async Task CreateBooking_PersistsDataInDatabase()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var postRequest = CreatePostRequest("booking");

    var postRequestBody = new BookingCreationRequest
    {
      FirstName = "Jim",
      LastName = "Brown",
      TotalPrice = 111,
      DepositPaid = true,
      BookingDates = new BookingDates
      {
        CheckIn = "2018-01-01",
        CheckOut = "2019-01-01"
      },
      AdditionalNeeds = "Breakfast"
    };

    postRequest.AddJsonBody(postRequestBody);

    //Act
    var response = await client.ExecuteAsync<BookingCreationResponse>(postRequest);

    //Assert
    var postRequestData = response.Data;
    postRequestData.Should().NotBeNull();

    var getRequest = CreateGetRequest("booking/{id}", postRequestData.BookingId.ToString());
    var getRequestResponse = await client.ExecuteAsync<Booking>(getRequest);
    var bookingData = getRequestResponse.Data;

    bookingData.Should().NotBeNull();
    bookingData.FirstName.Should().Be(postRequestBody.FirstName);
    bookingData.LastName.Should().Be(postRequestBody.LastName);
    bookingData.DepositPaid.Should().Be(postRequestBody.DepositPaid);
    bookingData.AdditionalNeeds.Should().Be(postRequestBody.AdditionalNeeds);
  }

  [Fact]
  public async Task CreateBooking_ReturnsStatusCode200()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var request = CreatePostRequest("booking");

    var creationRequestBody = new BookingCreationRequest
    {
      FirstName = "Jim",
      LastName = "Brown",
      TotalPrice = 111,
      DepositPaid = true,
      BookingDates = new BookingDates
      {
        CheckIn = "2018-01-01",
        CheckOut = "2019-01-01"
      },
      AdditionalNeeds = "Breakfast"
    };

    request.AddJsonBody(creationRequestBody);

    //Act
    var response = await client.ExecuteAsync<BookingCreationResponse>(request);

    //Assert
    response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
  }

  [Fact]
  public async Task CreateBooking_ReturnsStatusCode400_WhenRequestHasMissingFields()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var request = CreatePostRequest("booking");

    var creationRequestBody = new BookingCreationRequest
    {
      LastName = "Brown",
      TotalPrice = 111,
      DepositPaid = true,
      AdditionalNeeds = "Breakfast"
    };

    request.AddJsonBody(creationRequestBody);

    //Act
    var response = await client.ExecuteAsync(request);

    //Assert
    response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
  }

  [Fact]
  public async Task CreateBooking_ReturnsStatusCode400_WhenRequestBodyIsEmpty()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var request = CreatePostRequest("booking");

    var creationRequestBody = new object { };

    request.AddJsonBody(creationRequestBody);

    //Act
    var response = await client.ExecuteAsync(request);

    //Assert
    response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
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

  private RestRequest CreatePutRequest(string endpoint, string resourceId)
  {
    var request = new RestRequest(endpoint, Method.Put);
    request.AddUrlSegment("id", resourceId);

    return request;
  }

  private RestRequest CreatePatchRequest(string endpoint)
  {
    var request = new RestRequest(endpoint, Method.Patch);
    return request;
  }

  private RestRequest CreateDeleteRequest(string endpoint, string resourceId)
  {
    var request = new RestRequest(endpoint, Method.Delete);
    request.AddUrlSegment("id", resourceId);

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