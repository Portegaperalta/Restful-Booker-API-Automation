# Project Description
API Automation test suite made with XUnit, RestShap, & FluentAssertions for the [Restful Booker API](https://restful-booker.herokuapp.com/apidoc/index.html#api-Booking-PartialUpdateBooking). The project is designed to demonstrate industry-standard API testing, robust validation, and integration into a CI/CD pipeline.

Includes GitHub Actions CI, Auth, and STLC deliverables. 

### Note: The Restful Booker API is a "playground" API that contains intentional bugs and non-standard behaviors for testing practice.
### If the GitHub Actions build is "Failing," please check the Issues tab where these specific API bugs are documented as "Known Issues."

# Tech Stack: 
### - Framework: .NET 10
### - Test Runner: XUnit
### - HTTP Client: RestSharp
### - Assertions: FluentAssertions 
### - CI/CD: GitHub Actions

# Proyect Structure
```text
Restful-Booker-API-Automation/
├── .github/
│   └── workflows/          # GitHub Actions CI 
├── docs/                   # STLC Deliverables (Test Plan, Test Matrix, Passed Tests Evidence)
├── src/
│   ├── Models/             # Request/Response POCO classes
├── tests/
│   └── RestfulBooker.Tests/# xUnit Test Suite
├── RestfulBooker.sln       # Visual Studio Solution File
├── LICENSE
└── README.md
```
# Defect Management
### Any failed tests or identified bugs are tracked via the GitHub Issues tab. Each issue includes:

### - Steps to reproduce
### - Expected vs. Actual results
### - Traceability
### - Evidence / Screenshots

# Getting Started
### Prerequisites:
### .NET SDK 10.0+

### 1.Clone repository:
```bash
git clone https://github.com/Portegaperalta/Restful-Booker-API-Automation.git
```
## 2.Restore dependencies:
```bash
dotnet restore
```
## 3.Run the tests:
```bash
dotnet test
```
