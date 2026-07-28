# Domain

Equipment reservation domain

# Subdomains

- Equipment domain
- Reservation domain
- Account domain

# Bounded Contexts

Same as subdomains:

- Equipment context
- Reservation context
- Account context

# Value Objects

- Time span
- Equipment status

# Entities

- Equipment
- Reservation
- Employee

# Aggregates

![alt](https://yqintl.alicdn.com/b49cdbdd00bd745ea18d8256ef7ebbf6e229ae96.png)

```mermaid
classDiagram
    namespace ReservationAggregate {
        class Reservation {
            <<Aggregate Root>>
            +ReservationId
            +EquipmentId
            +TimeSpan
            +ReservedBy
        }
    }

    namespace EquipmentAggregate {
        class Equipment {
            <<Aggregate Root>>
            +EquipmentId
            +Name
            +Description
            +Creator
        }
    }

    namespace UserAggregate {
        class User {
            <<Aggregate Root>>
            +Name
            +Email
            +Sub
            ..etc
        }
    }

    Reservation --> User
    Equipment --> User
```