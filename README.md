# SHCA

## Improvements and Fixes
### Error Handling:
- [x] Ensure proper error handling and logging for API calls and other operations.
- [x] Fixes asynchronous and non- async mixed code
- [x] Avoid using .GetAwaiter().GetResult() to prevent blocking the main thread.
### HttpClient Usage:
- [x] Use a single instance of HttpClient to avoid socket exhaustion.
### Separation of Concerns:
- [x] Refactor code to separate business logic, data access, and API calls.
### Configuration Management:
- [x] Use configuration files or environment variables for URLs and other settings.
### Architecture Optimization
- [x] Create repository pattern using DDD / Clean code standards.
### Dependency Injection:
- [x] Use dependency injection for better testability and maintainability.
### Code Readability:
- [x] Improve code readability with meaningful variable names and comments.
### Performance Optimization:
- [x] Optimize loops and data processing for better performance.
### Security
- [x] Included auth implementation and integration with stubbed Azure configuration assets.
### Testing Setup
- [x] Mock tests covering both the data access and business logic calls.


## Architecture Overview
1.	Azure Function: Acts as the entry point for the scheduled task.
2.	Service Layer: Contains services responsible for specific tasks such as token acquisition and order processing.
3.	HttpClient: Used for making HTTP requests to external APIs.

## Modules
### 1. Azure Function
- Class: ScheduledNotificationCaller
  - Purpose: This is the Azure Function that gets triggered based on a schedule (every 5 minutes in this case).
  - Responsibilities:
    - Logs the start of the function execution.
    - Initializes the TokenService and OrderProcessor.
    - Acquires an access token using TokenService.
    - Processes orders using OrderProcessor.
    - Logs any errors that occur during the process.
### 2. Service Layer
- Class: TokenService
  - Purpose: Handles the acquisition of access tokens from Azure AD.
  - Responsibilities:
    - Uses Microsoft Identity Client (MSAL) to acquire tokens.
    - Handles exceptions related to token acquisition and logs errors.
- Class: OrderProcessor
  - Purpose: Handles the processing of orders by making HTTP requests to an external API.
  - Responsibilities:
    - Sets the authorization header with the acquired token.
    - Makes an HTTP POST request to the orders processing API.
    - Handles HTTP request exceptions and logs errors.
3. HttpClient
•	HttpClient Instance
•	Purpose: Used for making HTTP requests to external APIs.
•	Responsibilities:
•	Shared instance of HttpClient used by OrderProcessor to make API calls.
Interaction Flow
1.	Trigger: The Azure Function ScheduledNotificationCaller is triggered by a timer (every 5 minutes).
2.	Logging Start: Logs the start of the function execution.
3.	Token Acquisition:
•	TokenService is called to acquire an access token from Azure AD.
•	Handles and logs any exceptions during token acquisition.
4.	Order Processing:
•	OrderProcessor is called to process orders by making an HTTP POST request to the orders processing API.
•	Sets the authorization header with the acquired token.
•	Handles and logs any exceptions during the HTTP request.
5.	Logging End: Logs the success or failure of the order processing.
Benefits of This Architecture
•	Separation of Concerns: Each class has a single responsibility, making the code easier to understand and maintain.
•	Reusability: Services like TokenService and OrderProcessor can be reused in other parts of the application.
•	Testability: The modular design makes it easier to write unit tests for each component.
•	Scalability: The architecture can be easily extended with additional services or functionality without affecting existing code.
