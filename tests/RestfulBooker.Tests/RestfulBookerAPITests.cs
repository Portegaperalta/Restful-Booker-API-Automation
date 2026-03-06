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

    var authRequest = new AuthPostRequest
    {
      Username = "admin",
      Password = "password123"
    };

    var request = CreatePostRequest("auth");
    request.AddJsonBody(authRequest);

    //Act
    var response = await client.ExecuteAsync<AuthPostResponse>(request);

    //Assert
    var data = response.Data;

    data.Should().NotBeNull();
    data.Should().BeOfType<AuthPostResponse>();
  }

  [Fact]
  public async Task CreateToken_ReturnsStatusCode200_WhenCredentialsAreValid()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);

    var authRequest = new AuthPostRequest
    {
      Username = "admin",
      Password = "password123"
    };

    var request = CreatePostRequest("auth");
    request.AddJsonBody(authRequest);

    // Act
    var response = await client.ExecuteAsync<AuthPostResponse>(request);

    // Assert
    response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
  }

  [Fact]
  public async Task CreateToken_ReturnsStatusCode401_WhenCredentialsAreInvalid()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);

    var authRequest = new AuthPostRequest
    {
      Username = "invalidUser",
      Password = "WrongPassword123"
    };

    var request = CreatePostRequest("auth");
    request.AddJsonBody(authRequest);

    // Act
    var response = await client.ExecuteAsync<AuthPostResponse>(request);

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
    var postRequest = CreatePostRequest("booking");

    var postRequestBody = new BookingPostRequest
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
    var postResponse = await client.ExecuteAsync<BookingPostResponse>(postRequest);

    postResponse.Data.Should().NotBeNull();
    var bookingId = postResponse.Data.BookingId.ToString();

    //Act
    var getRequest = CreateGetRequest("booking/{id}", bookingId);

    // Act
    var response = await client.ExecuteAsync<Booking>(getRequest);

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
    var postRequest = CreatePostRequest("booking");

    var postRequestBody = new BookingPostRequest
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
    var postResponse = await client.ExecuteAsync<BookingPostResponse>(postRequest);

    postResponse.Data.Should().NotBeNull();
    var bookingId = postResponse.Data.BookingId.ToString();

    //Act
    var getRequest = CreateGetRequest("booking/{id}", bookingId);

    // Act
    var response = await client.ExecuteAsync<Booking>(getRequest);

    // Assert
    response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
  }

  [Fact]
  public async Task GetBookingById_ReturnsStatusCode404_WhenBookingDoesNotExist()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var request = CreateGetRequest("booking/{id}", "80000");

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

    var postRequestBody = new BookingPostRequest
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
    var response = await client.ExecuteAsync<BookingPostResponse>(postRequest);

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

    var creationRequestBody = new BookingPostRequest
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
    var response = await client.ExecuteAsync<BookingPostResponse>(request);

    //Assert
    response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
  }

  [Fact]
  public async Task CreateBooking_ReturnsStatusCode400_WhenRequestHasMissingFields()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var request = CreatePostRequest("booking");

    var creationRequestBody = new InvalidBookingPostRequest
    {
      LastName = "Brown",
      TotalPrice = 111,
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

  // Booking - UpdateBooking Tests
  [Fact]
  public async Task UpdateBooking_UpdatesPersistedData()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var token = await CreateToken(client);
    client.AddDefaultHeader("Cookie", $"token={token}");

    var postRequest = CreatePostRequest("booking");

    var postRequestBody = new BookingPostRequest
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

    var postResponse = await client.ExecuteAsync<BookingPostResponse>(postRequest);
    postResponse.Data.Should().NotBeNull();

    var bookingId = postResponse.Data.BookingId;
    var putRequest = CreatePutRequest("booking/{id}", bookingId.ToString());

    var putRequestBody = new BookingPutRequest
    {
      FirstName = "James",
      LastName = "Brown",
      TotalPrice = 111,
      DepositPaid = true,
      BookingDates =
      {
        CheckIn = "2018-01-01",
        CheckOut = "2019-01-01"
      },
      AdditionalNeeds = "Breakfast"
    };

    putRequest.AddJsonBody(putRequestBody);

    // Act
    await client.ExecuteAsync<BookingPutResponse>(putRequest);

    // Assert
    var getRequest = CreateGetRequest("booking/{id}", bookingId.ToString());
    var getRequestResponse = await client.ExecuteAsync<Booking>(getRequest);
    var updatedBookingData = getRequestResponse.Data;

    updatedBookingData.Should().NotBeNull();
    updatedBookingData.FirstName.Should().Be(putRequestBody.FirstName);
    updatedBookingData.LastName.Should().Be(putRequestBody.LastName);
    updatedBookingData.TotalPrice.Should().Be(putRequestBody.TotalPrice);
    updatedBookingData.DepositPaid.Should().Be(putRequestBody.DepositPaid);
    updatedBookingData.BookingDates.CheckIn.Should().Be(putRequestBody.BookingDates.CheckIn);
    updatedBookingData.BookingDates.CheckOut.Should().Be(putRequestBody.BookingDates.CheckOut);
    updatedBookingData.AdditionalNeeds.Should().Be(putRequestBody.AdditionalNeeds);
  }

  [Fact]
  public async Task UpdateBooking_ReturnsStatusCode200()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var token = await CreateToken(client);
    client.AddDefaultHeader("Cookie", $"token={token}");

    var postRequest = CreatePostRequest("booking");

    var postRequestBody = new BookingPostRequest
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

    var postResponse = await client.ExecuteAsync<BookingPostResponse>(postRequest);
    postResponse.Data.Should().NotBeNull();

    var bookingId = postResponse.Data.BookingId;

    var putRequest = CreatePutRequest("booking/{id}", bookingId.ToString());

    var putRequestBody = new BookingPutRequest
    {
      FirstName = "James",
      LastName = "Brown",
      TotalPrice = 111,
      DepositPaid = true,
      BookingDates =
      {
        CheckIn = "2018-01-01",
        CheckOut = "2019-01-01"
      },
      AdditionalNeeds = "Breakfast"
    };

    putRequest.AddJsonBody(putRequestBody);

    // Act
    var putResponse = await client.ExecuteAsync<BookingPutResponse>(putRequest);

    // Assert
    putResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
  }

  [Fact]
  public async Task UpdateBooking_ReturnsStatusCode400_WhenRequestBodyHasMissingFields()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var token = await CreateToken(client);
    client.AddDefaultHeader("Cookie", $"token={token}");

    var postRequest = CreatePostRequest("booking");

    var postRequestBody = new BookingPostRequest
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

    var postResponse = await client.ExecuteAsync<BookingPostResponse>(postRequest);
    postResponse.Data.Should().NotBeNull();

    var bookingId = postResponse.Data.BookingId;

    var putRequest = CreatePutRequest("booking/{id}", bookingId.ToString());

    var putRequestBody = new InvalidBookingPutRequest
    {
      LastName = "Brown",
      TotalPrice = 111,
      AdditionalNeeds = "Breakfast"
    };

    putRequest.AddJsonBody(putRequestBody);

    // Act
    var putResponse = await client.ExecuteAsync<BookingPutResponse>(putRequest);

    // Assert
    putResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
  }

  // Booking - PartialUpdateBooking Tests
  [Fact]
  public async Task PartialUpdateBooking_UpdatesPersistedData()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var token = await CreateToken(client);
    client.AddDefaultHeader("Cookie", $"token={token}");

    var postRequest = CreatePostRequest("booking");

    var postRequestBody = new BookingPostRequest
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

    var postResponse = await client.ExecuteAsync<BookingPostResponse>(postRequest);
    postResponse.Data.Should().NotBeNull();

    var bookingId = postResponse.Data.BookingId;
    var patchRequest = CreatePatchRequest("booking/{id}", bookingId.ToString());

    var patchRequestBody = new BookingPatchRequest
    {
      FirstName = "James",
      LastName = "Brown",
    };

    patchRequest.AddJsonBody(patchRequestBody);

    // Act
    await client.ExecuteAsync(patchRequest);

    // Assert
    var getRequest = CreateGetRequest("booking/{id}", bookingId.ToString());
    var getRequestResponse = await client.ExecuteAsync<Booking>(getRequest);

    var updatedBookingData = getRequestResponse.Data;

    updatedBookingData.Should().NotBeNull();
    updatedBookingData.FirstName.Should().Be(patchRequestBody.FirstName);
    updatedBookingData.LastName.Should().Be(patchRequestBody.LastName);
  }

  [Fact]
  public async Task PartialUpdateBooking_ReturnsStatusCode200()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var token = await CreateToken(client);
    client.AddDefaultHeader("Cookie", $"token={token}");

    var postRequest = CreatePostRequest("booking");

    var postRequestBody = new BookingPostRequest
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

    var postResponse = await client.ExecuteAsync<BookingPostResponse>(postRequest);
    postResponse.Data.Should().NotBeNull();

    var bookingId = postResponse.Data.BookingId;
    var patchRequest = CreatePatchRequest("booking/{id}", bookingId.ToString());

    var patchRequestBody = new BookingPatchRequest
    {
      FirstName = "James",
      LastName = "Brown",
    };

    patchRequest.AddJsonBody(patchRequestBody);

    // Act
    var patchResponse = await client.ExecuteAsync(patchRequest);

    // Assert
    patchResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
  }

  [Fact]
  public async Task
  PartialUpdateBooking_ReturnsStatusCode404_WhenBookingDoesNotExist()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var token = await CreateToken(client);
    client.AddDefaultHeader("Cookie", $"token={token}");

    var patchRequest = CreatePatchRequest("booking/{id}", "8000000");

    var patchRequestBody = new BookingPatchRequest
    {
      FirstName = "James",
      LastName = "Brown",
    };

    patchRequest.AddJsonBody(patchRequestBody);

    // Act
    var patchResponse = await client.ExecuteAsync<BookingPatchResponse>(patchRequest);

    // Assert
    patchResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task
  PartialUpdateBooking_ReturnsStatusCode400_WhenRequestHasInvalidFields()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var token = await CreateToken(client);
    client.AddDefaultHeader("Cookie", $"token={token}");

    var postRequest = CreatePostRequest("booking");

    var postRequestBody = new BookingPostRequest
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

    var postResponse = await client.ExecuteAsync<BookingPostResponse>(postRequest);
    postResponse.Data.Should().NotBeNull();

    var bookingId = postResponse.Data.BookingId;
    var patchRequest = CreatePatchRequest("booking/{id}", bookingId.ToString());

    var invalidPatchRequestBody = new InvalidBookingPatchRequest { Age = 30 };

    patchRequest.AddJsonBody(invalidPatchRequestBody);

    // Act
    var patchResponse = await client.ExecuteAsync<BookingPatchResponse>(patchRequest);

    // Assert
    patchResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
  }

  // Booking - DeleteBooking Tests
  [Fact]
  public async Task DeleteBooking_DeletesPersistedData()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var token = await CreateToken(client);
    client.AddDefaultHeader("Cookie", $"token={token}");

    var postRequest = CreatePostRequest("booking");

    var postRequestBody = new BookingPostRequest
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

    var postResponse = await client.ExecuteAsync<BookingPostResponse>(postRequest);
    postResponse.Data.Should().NotBeNull();

    var bookingId = postResponse.Data.BookingId.ToString();

    var deleteRequest = CreateDeleteRequest("booking/{id}", bookingId);

    //Act
    await client.ExecuteAsync(deleteRequest);

    //Assert
    var getRequest = CreateGetRequest("booking/{id}", bookingId);
    var getResponse = await client.ExecuteAsync<Booking>(getRequest);

    getResponse.Data.Should().BeNull();
  }

  [Fact]
  public async Task DeleteBooking_ReturnsStatusCode204()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var token = await CreateToken(client);
    client.AddDefaultHeader("Cookie", $"token={token}");

    var postRequest = CreatePostRequest("booking");

    var postRequestBody = new BookingPostRequest
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

    var postResponse = await client.ExecuteAsync<BookingPostResponse>(postRequest);
    postResponse.Data.Should().NotBeNull();

    var bookingId = postResponse.Data.BookingId.ToString();

    var deleteRequest = CreateDeleteRequest("booking/{id}", bookingId);

    //Act
    var deleteResponse = await client.ExecuteAsync(deleteRequest);

    //Assert
    deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
  }

  [Fact]
  public async Task DeleteBooking_ReturnsStatusCode404_WhenBookingDoesNotExist()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);
    var token = await CreateToken(client);
    client.AddDefaultHeader("Cookie", $"token={token}");

    var deleteRequest = CreateDeleteRequest("booking/{id}", "80000");

    //Act
    var deleteResponse = await client.ExecuteAsync(deleteRequest);

    //Assert
    deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task DeleteBooking_ReturnsStatusCode401_WhenUserDoesNotHaveAuthToken()
  {
    // Arrange
    var client = CreateRestClient(_baseUrl);

    var postRequest = CreatePostRequest("booking");

    var postRequestBody = new BookingPostRequest
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

    var postResponse = await client.ExecuteAsync<BookingPostResponse>(postRequest);
    postResponse.Data.Should().NotBeNull();

    var bookingId = postResponse.Data.BookingId.ToString();

    var deleteRequest = CreateDeleteRequest("booking/{id}", bookingId);

    //Act
    var deleteResponse = await client.ExecuteAsync(deleteRequest);

    //Assert
    deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
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

  private RestRequest CreatePatchRequest(string endpoint, string resourceId)
  {
    var request = new RestRequest(endpoint, Method.Patch);
    request.AddUrlSegment("id", resourceId);

    return request;
  }

  private RestRequest CreateDeleteRequest(string endpoint, string resourceId)
  {
    var request = new RestRequest(endpoint, Method.Delete);
    request.AddUrlSegment("id", resourceId);

    return request;
  }

  private async Task<string> CreateToken(RestClient client)
  {
    var authRequest = new AuthPostRequest
    {
      Username = "admin",
      Password = "password123"
    };

    var request = CreatePostRequest("auth");
    request.AddJsonBody(authRequest);

    var response = await client.ExecuteAsync<AuthPostResponse>(request);

    if (response.Data is null)
    {
      throw new Exception("Token creation failed");
    }

    return response.Data.Token;
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