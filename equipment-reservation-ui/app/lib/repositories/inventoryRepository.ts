import type { Equipment } from "~/lib/types"

export const InventoryRepository = (accessToken: string) => ({
  postEquipment: async (data: { name: string; description: string }) => {
    const response = await fetch("/api/v1/equipment", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${accessToken}`,
      },
      body: JSON.stringify(data),
    })
    if (!response.ok) throw await response.text()

    return await response.json()
  },
  readEquipment: async (): Promise<
    [{ equipmentId: string; name: string; description: string }]
  > => {
    const response = await fetch("/api/v1/equipment", {
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    })

    if (!response.ok) throw await response.text()

    return await response.json()
  },
  readEquipmentById: async (id: string): Promise<Equipment> => {
    const response = await fetch("/api/v1/equipment/" + id, {
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    })

    if (!response.ok) throw await response.json()

    return await response.json()
  },
  deleteEquipment: async (id: string) => {
    const response = await fetch(`/api/v1/equipment/${id}`, {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    })
    if (!response.ok) throw await response.text()
  },
})
