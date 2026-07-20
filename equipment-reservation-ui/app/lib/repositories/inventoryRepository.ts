const InventoryRepository = () => ({
  postEquipment: async (data: { name: string; description: string }) => {
    const response = await fetch("/api/v1/equipment", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    })
    if (!response.ok) throw await response.text()

    return await response.json()
  },
  readEquipment: async (): Promise<
    [{ equipmentId: string; name: string; description: string }]
  > => {
    const response = await fetch("/api/v1/equipment")

    if (!response.ok) throw await response.text()

    return await response.json()
  },
  readEquipmentById: async (
    id: string
  ): Promise<{ equipmentId: string; name: string; description: string }> => {
    const response = await fetch("/api/v1/equipment/" + id)

    if (!response.ok) throw await response.text()

    return await response.json()
  },
  deleteEquipment: async (id: string) => {
    const response = await fetch(`/api/v1/equipment/${id}`, {
      method: "DELETE",
    })
    if (!response.ok) throw await response.text()
  },
})

const inventoryRepository = InventoryRepository()

export default inventoryRepository
