@wip
Feature: TS05 - Driver data record
    As a logistics management entrepreneur, I want to register my drivers' data to assign them appropriately to each shipment.

    @TS05 @API @Drivers @POST
    Scenario: Register a new driver successfully
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        When the entrepreneur sends a POST request with the driver data to the API
            | Endpoint        | Method | Body                                                                                                             |
            | /api/v1/drivers | POST   | {"name": "Carlos Perez", "dni": "12345678", "license": "A1B2C3D4E5", "contactNumber": "987654321", "entrepreneurId": 1} |
        Then the API responds with status code 200
        And the driver is correctly added to the database

    @TS05 @API @Drivers @GET
    Scenario: Get registered driver information
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there is a registered driver with ID 1
        When the entrepreneur sends a GET request to obtain the information of a specific driver
            | Endpoint          | Method |
            | /api/v1/drivers/1 | GET    |
        Then the API responds with status code 200
        And returns the requested driver data

    @TS05 @API @Drivers @Error
    Scenario: Error registering driver with invalid data
        Given the entrepreneur has access to the API documentation
        When the entrepreneur sends a POST request with invalid driver data
            | Endpoint        | Method | Body                                                        |
            | /api/v1/drivers | POST   | {"name": "", "dni": "", "license": "", "entrepreneurId": 0} |
        Then the API responds with status code 400
        And returns a validation error message
