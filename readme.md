# Equipment Reservation System

## TODO:
- Aspire dashboard
- Jwt autentication for reservation
- Save sub to database for equipment and reservation
- Finish docs
- ask tutor about grading

## Diagram

```mermaid
sequenceDiagram
  participant reservation
  participant inventory

  reservation->>inventory: Availability Request
  inventory->>reservation: Availability Response
```
