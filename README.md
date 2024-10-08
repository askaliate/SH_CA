# SHCA - Code Assesment Supplementary Materials

# Improvements and Fixes
- **Error Handling:**
  - Ensure proper error handling and logging for API calls and other operations.
  - Fix asynchronous and non-async mixed code.
  - Avoid using `.GetAwaiter().GetResult()` to prevent blocking the main thread.
- **HttpClient Usage:**
  - Use a single instance of `HttpClient` to avoid socket exhaustion.
- **Separation of Concerns:**
  - Refactor code to separate business logic, data access, and API calls. Each class has a single responsibility, making the code easier to understand and maintain.
- **Configuration Management:**
  - Use configuration files or environment variables for URLs and other settings.
- **Architecture Optimization:**
  - Create repository pattern using DDD / Clean code standards.
- **Dependency Injection:**
  - Use dependency injection for better testability and maintainability.
- **Code Readability:**
  - Improve code readability with meaningful variable names and comments.
- **Performance Optimization:**
  - Optimize loops and data processing for better performance.
- **Security:**
  - Include auth implementation and integration with stubbed Azure configuration assets.
- **Testability:**
  - Mock tests covering both the data access and business logic calls. The modular design makes it easier to write unit tests for each component.
- **Scalability:**
  - The architecture can be easily extended with additional services or functionality without affecting existing code.

# Solution Setup Guide and Architecture Summary

## Setup and Run Guide

### Prerequisites
1. **Development Environment**:
   - Visual Studio 2019 or later
   - .NET Core SDK 3.1 or later

2. **Azure Services**:
   - Azure Active Directory (Azure AD) for authentication
   - Azure Functions for running the scheduled tasks

3. **External Dependencies**:
   - Microsoft.Identity.Client (MSAL) for authentication
   - Newtonsoft.Json for JSON serialization/deserialization
   - Microsoft.Extensions.Logging for logging

### Setup Steps

1. **Clone the Repository**:
   - Clone the repository containing the solution to your local machine.

2. **Environment Variables**:
   - Set the following environment variables in your Azure Function App settings - create local.settings.json file with the following code
```
    {
      "IsEncrypted": false,
      "Values": {
          "AzureWebJobsStorage": "UseDevelopmentStorage=true",
          "FUNCTIONS_WORKER_RUNTIME": "dotnet",
          "AzureAd:Instance": "https://login.microsoftonline.com/",
          "AzureAd:ClientId": "client-id-from-Azure-config-not-local",
          "AzureAd:TenantId": "tenant-id-from-Azure-config-not-local",
          "AzureAd:ClientSecret": "client-secret-from-Azure-config-not-local"
    }
  }
```

3. **Install Dependencies**:
   - Open the solution in Visual Studio.
   - Restore NuGet packages to ensure all dependencies are installed:
     
	  ```dotnet restore```

4. **Build the Solution**:
   - Build the solution to ensure there are no compilation errors:

	  ```dotnet build```

## Architecture Overview
The solution is designed with a modular architecture to separate concerns, improve maintainability, and enhance scalability. The main components include Azure Functions, a service layer, and a shared HttpClient instance.

## Modules

### Project: SHCA.Functions.Order

#### Class: ScheduledNotificationCaller
- **Purpose**: Acts as the entry point for the scheduled task that processes orders.
- **Responsibilities**:
  - Triggered by a timer (every 5 minutes).
  - Logs the start and end of the function execution.
  - Initializes and uses `TokenService` to acquire an access token.
  - Initializes and uses `OrderProcessor` to process orders.
  - Handles and logs any errors that occur during the process.

### Project: SHCA.App.OrderProcessing.Monitor

#### Class: OrderServiceClient
- **Purpose**: Fetches medical equipment orders from an external API.
- **Responsibilities**:
  - Acquires an access token using `TokenHandler`.
  - Makes an HTTP GET request to fetch orders.
  - Handles HTTP request exceptions and logs errors.

#### Class: OrderStatusProcessorService
- **Purpose**: Processes fetched orders.
- **Responsibilities**:
  - Fetches orders using `OrderServiceClient`.
  - Processes orders using `OrderProcessor`.
  - Logs any errors that occur during the process.

#### Class: OrderUpdateService
- **Purpose**: Sends alerts and updates orders.
- **Responsibilities**:
  - Acquires an access token using `TokenHandler`.
  - Makes an HTTP POST request to update orders.
  - Handles HTTP request exceptions and logs errors.

### Project: SHCA.App.OrderProcessing.Monitor.Auth

#### Class: TokenHandler
- **Purpose**: Handles the acquisition of access tokens from Azure AD.
- **Responsibilities**:
  - Uses Microsoft Identity Client (MSAL) to acquire tokens.
  - Handles exceptions related to token acquisition and logs errors.

### Project: SHCA.Functions.Order.Services - NOT Production ready, included for architecture flow

#### Class: TokenService
- **Purpose**: Handles the acquisition of access tokens from Azure AD.
- **Responsibilities**:
  - Uses Microsoft Identity Client (MSAL) to acquire tokens.
  - Handles exceptions related to token acquisition and logs errors.

#### Class: OrderProcessor
- **Purpose**: Handles the processing of orders by making HTTP requests to an external API.
- **Responsibilities**:
  - Sets the authorization header with the acquired token.
  - Makes an HTTP POST request to the orders processing API.
  - Handles HTTP request exceptions and logs errors.

### Project: SHCA.Domain.Entities

#### Class: Order
- **Purpose**: Represents an order entity.
- **Responsibilities**:
  - Contains properties such as `OrderId`, `Type`, `Status`, `OrderDate`, `Items`, etc.
  - Used by various services to represent order data.

#### Class: OrderItem
- **Purpose**: Represents an order item entity.
- **Responsibilities**:
  - Contains properties such as `OrderItemId`, `Status`, `Description`, `DeliveryNotificationCount`, etc.
  - Used by various services to represent order item data.

### Project: SHCA.Infra.Repositories

#### Class: OrderRepository
- **Purpose**: Handles data access for orders.
- **Responsibilities**:
  - Contains methods to interact with the database for CRUD operations on orders.
  - Used by various services to persist and retrieve order data.

### Shared Components

#### HttpClient Instance
- **Purpose**: Used for making HTTP requests to external APIs.
- **Responsibilities**:
  - Shared instance of `HttpClient` used by various services to make API calls.

## Interaction Flow
1. **Trigger**: The Azure Function `ScheduledNotificationCaller` is triggered by a timer (every 5 minutes).
2. **Logging Start**: Logs the start of the function execution.
3. **Token Acquisition**: 
   - `TokenService` or `TokenHandler` is called to acquire an access token from Azure AD.
   - Handles and logs any exceptions during token acquisition.
4. **Order Processing**:
   - `OrderServiceClient` fetches orders by making an HTTP GET request.
   - `OrderStatusProcessorService` processes the fetched orders.
   - `OrderUpdateService` sends alerts and updates orders by making an HTTP POST request.
   - Handles and logs any exceptions during the HTTP requests.
5. **Logging End**: Logs the success or failure of the order processing.

## Benefits of This Architecture
- **Separation of Concerns**: Each class has a single responsibility, making the code easier to understand and maintain.
- **Reusability**: Services like `TokenService`, `OrderProcessor`, `OrderServiceClient`, `OrderStatusProcessorService`, and `OrderUpdateService` can be reused in other parts of the application.
- **Testability**: The modular design makes it easier to write unit tests for each component.
- **Scalability**: The architecture can be easily extended with additional services or functionality without affecting existing code.

## Summary of Full Classes
- **ScheduledNotificationCaller**: The Azure Function entry point that coordinates the token acquisition and order processing using the services.
- **TokenService**: Manages token acquisition from Azure AD.
- **OrderProcessor**: Manages the order processing logic by making HTTP requests to the external API.
- **OrderServiceClient**: Fetches medical equipment orders from an external API.
- **OrderStatusProcessorService**: Processes fetched orders.
- **OrderUpdateService**: Sends alerts and updates orders.
- **TokenHandler**: Handles the acquisition of access tokens from Azure AD.
- **Order**: Represents an order entity.
- **OrderItem**: Represents an order item entity.
- **OrderRepository**: Handles data access for orders.
- **HttpClient**: A shared instance used by various services for making API calls.
   
     
