export interface Equipment {
  equipmentId: string
  name: string
  description: string,
  creator: string,
  availability: EquipmentAvailability
}

export interface Reservation {
  status: ReservationStatus,
  equipmentId: string,
  reservationId: string,
  startTime: string,
  endTime: string,
  reservedBy: string
}

export enum ReservationStatus {
  Reserved = 0,
  Started = 1,
  Returned = 2,
}

export interface ApiError {
  status: number,
  title: string,
  traceId: string,
  type: string
}

export enum EquipmentAvailability
{
  Available = 0,
  InUse = 1
}
