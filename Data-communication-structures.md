# Workflow

The system uses choreography and orchestration workflow patterns.


```mermaid
sequenceDiagram
    participant UI
    participant Reservation as Reservation Service
    participant Inventory as Inventory Service
    Note over UI, Inventory: Reservation Request with synchronous request-response process
    UI ->> Reservation: Reserve Request
    Reservation ->> Inventory: Get Equipment Request
    Inventory ->> Reservation: Get Equipment Response
    Reservation ->> UI: Reserve Response
    Note over Reservation, Inventory: Events between services using pub/sub pattern
    Inventory ->> Reservation: EquipmentDeleteEvent
    Reservation ->> Inventory: ReservationCreatedEvent
    Reservation ->> Inventory: ReservationCanceledEvent
    Reservation ->> Inventory: ReservationStartedEvent
    Reservation ->> Inventory: ReservationReturnedEvent

    note over UI,Inventory: All requests made from UI are authenticated by Account Service
```



# Events

## EquipmentDeleteEvent

Published by: Inventory Service

Description: Equipment has been deleted from the system.

Schema: 

```json
{{EquipmentId}}
```

## ReservationCreatedEvent

Published by: Reservation Service

Description: Reservation was created in the system.

Schema:
```json
{
    ReservationId: Guid,
    EquipmentId: Guid
}
```


## ReservationCanceledEvent

Published by: Reservation Service

Description: Reservation was canceled.

Schema:
```json
{
    ReservationId: Guid,
    EquipmentId: Guid
}
```

## ReservationStartedEvent

Published by: Reservation Service

Description: Reservation was started.

Schema:
```json
{
    ReservationId: Guid,
    EquipmentId: Guid
}
```

## ReservationReturnedEvent

Published by: Reservation Service

Description: Equipment was returned after being used, completes the reservation.

Schema:
```json
{
    ReservationId: Guid,
    EquipmentId: Guid
}
```

