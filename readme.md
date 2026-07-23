# Equipment Reservation System

## TODO:
- Aspire dashboard
- OpenID for Inventory and reservation
- Show current user with Card and force login
- Save sub to database for equipment and reservation

## Diagram

```mermaid
sequenceDiagram
  participant reservation
  participant inventory

  reservation->>inventory: Availability Request
  inventory->>reservation: Availability Response
```
