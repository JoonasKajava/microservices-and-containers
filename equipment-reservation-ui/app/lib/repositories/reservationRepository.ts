import type { Dayjs } from "dayjs"
import type { Reservation } from "~/lib/types"

const ReservationRepository = () => ({
  postReservation: async (data: {
    equipmentId: string
    startTime: Dayjs
    endTime: Dayjs
  }) => {
    const response = await fetch("/api/v1/reservations", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    })
    if (!response.ok) throw await response.text()

    return await response.json()
  },
  readReservations: async (equipmentId: string): Promise<Reservation[]> => {
    let params = new URLSearchParams()
    params.append("equipmentId", equipmentId)

    const response = await fetch("/api/v1/reservations?" + params.toString())

    if (!response.ok) throw await response.json()

    return await response.json()
  },
  deleteReservation: async (id: string) => {
    const response = await fetch(`/api/v1/reservations/${id}`, {
      method: "DELETE",
    })
    if (!response.ok) throw await response.text()
  },
})

const reservationRepository = ReservationRepository()

export default reservationRepository
