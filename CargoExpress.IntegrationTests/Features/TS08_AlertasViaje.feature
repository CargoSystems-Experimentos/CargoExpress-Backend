@wip
Feature: TS08 - Recording and viewing travel alerts
    As a logistics management entrepreneur, I want to receive an alert about any important event that may affect delivery to minimize any impact on my operation.

    @TS08 @API @Alerts @POST
    Scenario: Record a travel alert successfully
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        When the entrepreneur sends a POST request with the alert data to the API
            | Endpoint       | Method | Body                                                                                                                   |
            | /api/v1/alerts | POST   | {"title": "Demora en ruta", "type": "WARNING", "description": "Trafico en la via", "date": "2024-01-01T10:00:00", "tripId": 1} |
        Then the API responds with status code 200
        And the alert is correctly added to the database

    @TS08 @API @Alerts @GET
    Scenario: View all alerts for a trip
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there is a registered alert for trip with ID 1
        When the entrepreneur sends a GET request to obtain all alerts for a trip
            | Endpoint               | Method |
            | /api/v1/trips/1/alerts | GET    |
        Then the API responds with status code 200
        And returns the list of alerts for the trip

    @TS08 @API @Alerts @Error
    Scenario: Error recording alert with invalid data
        Given the entrepreneur has access to the API documentation
        When the entrepreneur sends a POST request with invalid alert data
            | Endpoint       | Method | Body                                         |
            | /api/v1/alerts | POST   | {"title": "", "type": "", "tripId": 0}        |
        Then the API responds with status code 400
        And returns a validation error message
