export interface Equipment {
  equipmentId: string
  name: string
  description: string
}

export interface ApiError {
  status: number,
  title: string,
  traceId: string,
  type: string
}