import type { Dayjs } from "dayjs"
import { type Reservation, ReservationStatus } from "~/lib/types"

export const ReservationRepository = (accessToken: string) => ({
  postReservation: async (data: {
    equipmentId: string
    startTime: Dayjs
    endTime: Dayjs
  }): Promise<Reservation> => {
    const response = await fetch("/api/v1/reservations", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${accessToken}`,
      },
      body: JSON.stringify(data),
    })
    if (!response.ok) throw await response.json()

    return await response.json()
  },
  readReservations: async (equipmentId: string): Promise<Reservation[]> => {
    let params = new URLSearchParams()
    params.append("equipmentId", equipmentId)

    const response = await fetch("/api/v1/reservations?" + params.toString(), {
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    })

    if (!response.ok) throw await response.json()

    return await response.json()
  },
  deleteReservation: async (id: string) => {
    const response = await fetch(`/api/v1/reservations/${id}`, {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    })
    if (!response.ok) throw await response.text()
  },

  changeReservationStatus: async (id: string, status: ReservationStatus) => {
    const response = await fetch(`/api/v1/reservations`, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${accessToken}`,
      },
      body: JSON.stringify({ id, status }),
    })
    if (!response.ok) throw await response.text()
  },
})
