const InventoryRepository = () => ({
  postEquipment: async (data: { name: string; description: string }) => {
    const response = await fetch("/api/v1/equipment", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    })
    if (!response.ok) throw await response.text();

    return await response.json()
  },
})

const inventoryRepository = InventoryRepository()

export default inventoryRepository
