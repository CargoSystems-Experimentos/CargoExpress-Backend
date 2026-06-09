@wip
Feature: TS04 - Travel Expense Record
    As a logistics management entrepreneur, I want to record expenses made during trips to maintain an accurate record and keep my clients informed about the costs associated with their services.

    @TS04 @API @Expenses @POST
    Scenario: Record an expense for a trip successfully
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        When the entrepreneur sends a POST request with the expense data to the API
            | Endpoint         | Method | Body                                                                                                                                                    |
            | /api/v1/expenses | POST   | {"fuelAmount": 200, "fuelDescription": "Gasolina", "viaticsAmount": 50, "viaticsDescription": "Viaticos dia", "tollsAmount": 30, "tollsDescription": "Peaje norte", "tripId": 1} |
        Then the API responds with status code 200
        And the expense is correctly added to the database

    @TS04 @API @Expenses @GET
    Scenario: Get expense information for a trip
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there is a registered expense for trip with ID 1
        When the entrepreneur sends a GET request to obtain the expenses for a trip
            | Endpoint                 | Method |
            | /api/v1/trips/1/expenses | GET    |
        Then the API responds with status code 200
        And returns the expense data for the trip

    @TS04 @API @Expenses @Error
    Scenario: Error recording expense with invalid data
        Given the entrepreneur has access to the API documentation
        When the entrepreneur sends a POST request with invalid expense data
            | Endpoint         | Method | Body                              |
            | /api/v1/expenses | POST   | {"fuelAmount": -1, "tripId": 0}   |
        Then the API responds with status code 400
        And returns a validation error message
