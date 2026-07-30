# Equipment Reservation System

## Links

- [Data communication structures](./Data-communication-structures.md)
- [DDD Documentation](./DDD-Document.md)
- [Testing Plan](./Testing-plan.md)

## Demo

https://github.com/user-attachments/assets/1a767f00-1b7d-4fe3-a85e-e9118604100e

## Diagram

```mermaid
classDiagram

  namespace InternalNetwork {
    class Reservation {
  
    }
    class Inventory {
  
    }
  
    class PocketId {
      OpenID Connect Provider
    }
  
    class Aspire {
      Structured logs
      Traces
      Metrics
    }

    namespace Postgres {
      class InventoryDB {
        
      }
      class ReservationDB
    }
  }


  class UI {

  }
  class Caddy {

  }

  UI --> Caddy : Api Requests

  Caddy --> Reservation : REST API
  Caddy --> Inventory : REST API
  Caddy --> PocketId : Authentication
  Caddy --> Aspire : Dashboard

  Reservation --> Aspire : OpenTelemetry
  Inventory --> Aspire : OpenTelemetry

  Inventory --> Reservation : Delete Event
  Reservation --> Inventory : Request Equipment Info
  Reservation --> Inventory : Status Change Events 

  InventoryDB --> Inventory
  ReservationDB --> Reservation
  
```
