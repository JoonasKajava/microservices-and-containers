import { useMemo } from "react"
import { ReservationRepository } from "~/lib/repositories/reservationRepository"

export const useReservationRepository = (accessToken: string) => {
  return useMemo(() => {
    return ReservationRepository(accessToken)
  }, [accessToken])
}
