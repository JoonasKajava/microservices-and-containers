# Equipment Reservation System

## Diagram

```mermaid
sequenceDiagram
  participant reservation
  participant inventory

  reservation->>inventory: Availability Request
  inventory->>reservation: Availability Response
```
