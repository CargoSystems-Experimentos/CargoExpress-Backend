@wip
Feature: TS29 - Audit of changes in trips, vehicles and drivers
    As a logistics management entrepreneur, I want to view the audit log of my trips, vehicles, and drivers to have a traceable record of all changes made and ensure transparency with my clients.

    @TS29 @API @AuditLogs @GET
    Scenario: Get all audit logs for an entrepreneur
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there are registered audit logs associated with entrepreneur ID 1
        When the entrepreneur sends a GET request to obtain all audit logs
            | Endpoint                          | Method |
            | /api/v1/audit-logs/entrepreneur/1 | GET    |
        Then the API responds with status code 200
        And returns the list of audit logs for the entrepreneur

    @TS29 @API @AuditLogs @GET
    Scenario: Get trip audit logs for an entrepreneur
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there are registered trip audit logs for entrepreneur ID 1
        When the entrepreneur sends a GET request to obtain trip-specific audit logs
            | Endpoint                                | Method |
            | /api/v1/audit-logs/entrepreneur/trips/1 | GET    |
        Then the API responds with status code 200
        And returns the list of trip audit logs for the entrepreneur

    @TS29 @API @AuditLogs @Error
    Scenario: Error getting audit logs for a nonexistent entrepreneur
        Given the entrepreneur has access to the API documentation
        When the entrepreneur sends a GET request for audit logs of a nonexistent entrepreneur
            | Endpoint                              | Method |
            | /api/v1/audit-logs/entrepreneur/999   | GET    |
        Then the API responds with status code 200
        And returns an empty list of audit logs
