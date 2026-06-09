@wip
Feature: TS03 - Modification of trip data
    As a logistics management entrepreneur, I want to modify the data of a trip to correct erroneous data that was recorded.

    @TS03 @API @Trips @PUT
    Scenario: Update trip details successfully
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there is a registered trip with ID 1
        When the entrepreneur sends a PUT request to update the trip details
            | Endpoint                | Method | Body                                                                                             |
            | /api/v1/trips/1/details | PUT    | {"name": "Updated Trip", "type": "Food", "weight": 150, "addressFrom": "Av. Javier Prado 456"} |
        Then the API responds with status code 200
        And the trip details are correctly updated in the database

    @TS03 @API @Trips @PUT
    Scenario: Update trip schedule successfully
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there is a registered trip with ID 1
        When the entrepreneur sends a PUT request to update the trip schedule
            | Endpoint                 | Method | Body                                                                                                                         |
            | /api/v1/trips/1/schedule | PUT    | {"scheduledDepartureDate": "2024-06-01T08:00:00", "addressTo": "Av. Peru 789", "scheduledArrivalDate": "2024-06-01T14:00:00"} |
        Then the API responds with status code 200
        And the trip schedule is correctly updated in the database

    @TS03 @API @Trips @Error
    Scenario: Error updating trip with nonexistent ID
        Given the entrepreneur has access to the API documentation
        When the entrepreneur sends a PUT request to update a trip that does not exist
            | Endpoint                  | Method | Body                                                            |
            | /api/v1/trips/999/details | PUT    | {"name": "Ghost Trip", "type": "None", "weight": 0, "addressFrom": "Nowhere"} |
        Then the API responds with status code 404
        And returns a not found error message
