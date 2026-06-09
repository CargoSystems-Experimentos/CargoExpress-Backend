@wip
Feature: TS06 - Vehicle Data Record
    As a logistics management entrepreneur, I want to register my vehicles' data to have a record of the shipments they make.

    @TS06 @API @Vehicles @POST
    Scenario: Register a new vehicle successfully
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        When the entrepreneur sends a POST request with the vehicle data to the API
            | Endpoint         | Method | Body                                                                                                                                          |
            | /api/v1/vehicles | POST   | {"name": "Volvo Truck", "model": "Volvo FH16", "plate": "ABC123", "tractorPlate": "TRC456", "maxLoad": 20000, "volume": 80, "entrepreneurId": 1} |
        Then the API responds with status code 200
        And the vehicle is correctly added to the database

    @TS06 @API @Vehicles @GET
    Scenario: Get registered vehicle information
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there is a registered vehicle with ID 1
        When the entrepreneur sends a GET request to obtain the information of a specific vehicle
            | Endpoint           | Method |
            | /api/v1/vehicles/1 | GET    |
        Then the API responds with status code 200
        And returns the requested vehicle data

    @TS06 @API @Vehicles @Error
    Scenario: Error registering vehicle with invalid data
        Given the entrepreneur has access to the API documentation
        When the entrepreneur sends a POST request with invalid vehicle data
            | Endpoint         | Method | Body                                                        |
            | /api/v1/vehicles | POST   | {"name": "", "model": "", "plate": "", "entrepreneurId": 0} |
        Then the API responds with status code 400
        And returns a validation error message
