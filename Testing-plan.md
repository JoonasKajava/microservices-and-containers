# Testing Plan

## Integration Tests

Integration tests are performed by `Inventory.Tests`.

It tests equipment REST APIs.

## Mocks and stubs

Database and OIDC providers are not available so they are mocked during testing.

Database access is handled using repository pattern and during testing this repository is mocked.

Mocked database has configured to return stubbed data when requesting equipment by test id.

Since REST API requires authentication they a mock authentication scheme is registered which will always succeed.

## Example of integration test with mocked database and authentication

```mermaid

sequenceDiagram
    participant test as TestController
    participant API
    participant Auth as Auth Mock
    participant Database as Database Mock

    test ->> API: Get Equipment By Id
    API ->> Auth: Authenticate JWT
    Auth ->> API: Always success

    API ->> Database: Get Equipment By id
    note over API, Database: If equipment id is {{TestEquipmentId}}

    Database ->> API: Return "Laptop"
    API ->> test: Return equipment info
    note over test: Assert status code is 200
```