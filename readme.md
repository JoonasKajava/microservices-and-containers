# Equipment Reservation System

## TODO:
- Aspire dashboard
- OpenID for Inventory and reservation

## Diagram

```mermaid
sequenceDiagram
  participant reservation
  participant inventory

  reservation->>inventory: Availability Request
  inventory->>reservation: Availability Response
```
