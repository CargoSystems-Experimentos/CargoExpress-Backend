@wip
Feature: TS12 - Viewing expenses incurred during the trip
    As a logistics company client, I want to view the expenses made on each trip to know the reasons for the total amount for the service.

    @TS12 @API @Expenses @GET
    Scenario: View expenses for a specific trip
        Given the client has access to the API documentation and the necessary credentials for integration
        And there is a registered expense for trip with ID 1
        When the client sends a GET request to obtain the expenses for a trip
            | Endpoint                 | Method |
            | /api/v1/trips/1/expenses | GET    |
        Then the API responds with status code 200
        And returns the list of expenses for the trip

    @TS12 @API @Expenses @GET
    Scenario: Get expense details by expense ID
        Given the client has access to the API documentation and the necessary credentials for integration
        And there is a registered expense with ID 1
        When the client sends a GET request to obtain the details of a specific expense
            | Endpoint           | Method |
            | /api/v1/expenses/1 | GET    |
        Then the API responds with status code 200
        And returns the requested expense data

    @TS12 @API @Expenses @Error
    Scenario: Error getting expenses for a nonexistent trip
        Given the client has access to the API documentation
        When the client sends a GET request for expenses of a nonexistent trip
            | Endpoint                   | Method |
            | /api/v1/trips/999/expenses | GET    |
        Then the API responds with status code 200
        And returns an empty list of expenses
