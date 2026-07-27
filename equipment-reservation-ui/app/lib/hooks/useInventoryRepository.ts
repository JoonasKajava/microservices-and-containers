import { useMemo } from "react"
import { InventoryRepository } from "~/lib/repositories/inventoryRepository"

export const useInventoryRepository = (accessToken: string) => {
  return useMemo(() => {
    return InventoryRepository(accessToken)
  }, [accessToken])
}
