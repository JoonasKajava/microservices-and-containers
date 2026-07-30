import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { NavLink } from "react-router"
import SimpleError from "~/components/widgets/SimpleError"
import { Button, Empty, List } from "antd"
import { useInventoryRepository } from "~/lib/hooks/useInventoryRepository"
import { useAuth } from "react-oidc-context"

export default function EquipmentList() {
  const queryClient = useQueryClient()

  const auth = useAuth()

  const inventoryRepository = useInventoryRepository(auth.user?.access_token!)

  const equipmentQuery = useQuery({
    queryKey: ["equipment"],
    queryFn: inventoryRepository.readEquipment,
  })

  const deleteEquipmentMutation = useMutation({
    mutationFn: inventoryRepository.deleteEquipment,
    onSuccess: () => {
      queryClient
        .invalidateQueries({ queryKey: ["equipment"] })
        .catch((e) => console.error(e))
    },
  })

  if (equipmentQuery.data && equipmentQuery.data.length < 1) {
    return <Empty />
  } else if (equipmentQuery.isError) {
    return (
      <SimpleError
        title="Issue getting equipment list"
        description={equipmentQuery.error.message}
      />
    )
  } else {
    return (
      <section className="flex flex-col gap-4">
        <List
          loading={equipmentQuery.isPending}
          itemLayout="horizontal"
          dataSource={equipmentQuery.data}
          renderItem={(item) => (
            <List.Item
              actions={[
                <NavLink to={`/view-equipment/${item.equipmentId}`}>
                  <Button>View</Button>
                </NavLink>,
                <Button
                  disabled={deleteEquipmentMutation.isPending}
                  onClick={() =>
                    deleteEquipmentMutation.mutate(item.equipmentId)
                  }
                >
                  Delete
                </Button>,
              ]}
            >
              <List.Item.Meta
                title={item.name}
                description={item.description}
              />
            </List.Item>
          )}
        />
      </section>
    )
  }
}
