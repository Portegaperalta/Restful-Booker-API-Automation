# Test Plan: Restful Booker API Automation

---

## 1. Overview

This test plan establishes a range of test cases designed to verify the correct functionality and data consistency of the Restful Booker API.

The primary focus of these tests is to validate the interaction between the client and the RESTful service layer, ensuring that CRUD operations (Create, Read, Update, Delete) satisfy expected requirements, maintain data persistence, and handle authentication via JWT tokens securely.

---

## 2. Test Scope:

### 2.1 In-Scope:

*   **Authentication & Session Management:** Validation of secure token generation.

*   **CRUD Operations:** Full lifecycle validation of booking records.

*   **Data Persistence:** Ensuring `POST` and `PUT` requests accurately modify the underlying data store.

*   **Contract Validation:** Verification of HTTP status codes, response headers, and JSON schema accuracy.

*   **Negative Testing:** Validating system resilience against invalid dates, empty payloads, and expired credentials.

### 2.2 Out of Scope:

*   **UI/Frontend:** Any interaction with the web browser or user interface components.
*   **Non-Functional Testing:** Performance, Load, and Stress testing.
*   **Security Testing:** Deep-level penetration testing or vulnerability scanning.

---

## 3. Test Strategy

### 3.1 Testing levels

*   **API Integration Testing:** Validating communication protocols between the test client and the service layer.
*   **End-to-End (E2E) Flow Testing:** Simulating complete user journeys (e.g., *Authenticate -> Create -> Update -> Verify -> Delete*).
*   **Negative Testing:** Ensuring graceful error handling for malformed requests and unauthorized access.

### 3.2 Automation Approach
*   **Tech Stack:** C# (.NET 10), RestSharp (HTTP Client), and xUnit (Test Runner).
*   **Design Pattern:** **Request/Response Models** using C# POCOs for strong typing and schema safety.
*   **Assertions:** Utilizing **FluentAssertions** for human-readable, expressive validation logic.

### 3.3 Tooling and Infrastructure
*   **Version Control:** Git/GitHub for source code management.
*   **CI/CD Pipeline:** **GitHub Actions** to execute the full suite on every `push` and `pull_request` to the main branch.
*   **Reporting:** Console-based xUnit logs and GitHub Action execution summaries.

---

## 4. Test Deliverables
*   **Automated Test Suite:** C# source code located in the `/tests` directory.
*   **Requirement Traceability Matrix (RTM):** Detailed mapping of test cases to API requirements.
*   **Execution Reports:** Available via the GitHub Actions "Actions" tab.