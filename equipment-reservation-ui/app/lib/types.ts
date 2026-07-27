export interface Equipment {
  equipmentId: string
  name: string
  description: string,
  creator: string
}

export interface Reservation {
  equipmentId: string,
  reservationId: string,
  startTime: string,
  endTime: string,
  reservedBy: string
}

export interface ApiError {
  status: number,
  title: string,
  traceId: string,
  type: string
}