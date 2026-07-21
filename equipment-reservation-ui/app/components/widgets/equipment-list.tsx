import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import inventoryRepository from "~/lib/repositories/inventoryRepository"
import {
  Item,
  ItemActions,
  ItemContent,
  ItemDescription,
  ItemTitle,
} from "../ui/item"
import { Skeleton } from "../ui/skeleton"
import {
  Empty,
  EmptyDescription,
  EmptyHeader,
  EmptyMedia,
  EmptyTitle,
} from "../ui/empty"
import { BookDashed } from "lucide-react"
import { Button } from "../ui/button"
import { NavLink } from "react-router"
import { toast } from "sonner"
import SimpleError from "~/components/widgets/SimpleError"

export default function EquipmentList() {
  const queryClient = useQueryClient()

  const equipmentQuery = useQuery({
    queryKey: ["equipment"],
    queryFn: inventoryRepository.readEquipment,
  })

  const deleteEquipmentMutation = useMutation({
    mutationFn: inventoryRepository.deleteEquipment,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["equipment"] })
    },
  })

  if (equipmentQuery.isPending) {
    return (
      <section className="flex flex-col gap-4">
        {[...Array(3)].map((_, i) => (
          <Item key={i} variant="outline">
            <ItemContent>
              <ItemTitle>
                <Skeleton className="h-6 w-[250px]" />
              </ItemTitle>
              <div className="flex flex-col gap-2">
                <Skeleton className="h-3 w-[300px]" />
                <Skeleton className="h-3 w-[300px]" />
                <Skeleton className="h-3 w-[100px]" />
              </div>
            </ItemContent>
            <ItemActions>
              <Skeleton className="h-6 w-[50px]" />
            </ItemActions>
          </Item>
        ))}
      </section>
    )
  } else if (equipmentQuery.data && equipmentQuery.data.length < 1) {
    return (
      <Empty>
        <EmptyHeader>
          <EmptyMedia variant="icon">
            <BookDashed />
          </EmptyMedia>
          <EmptyTitle>No data</EmptyTitle>
          <EmptyDescription>No data found</EmptyDescription>
        </EmptyHeader>
      </Empty>
    )
  } else if (equipmentQuery.isError) {
    return <SimpleError title="Issue getting equipment list" description={equipmentQuery.error.message} />
  } else {
    return (
      <section className="flex flex-col gap-4">
        {equipmentQuery.data.map((value) => (
          <Item key={value.equipmentId} variant="outline">
            <ItemContent>
              <NavLink to={`/view-equipment/${value.equipmentId}`}>
                <ItemTitle>{value.name}</ItemTitle>
                <ItemDescription className="flex flex-col gap-2">
                  {value.description}
                </ItemDescription>
              </NavLink>
            </ItemContent>
            <ItemActions>
              <Button disabled={deleteEquipmentMutation.isPending}>
                Reserve
              </Button>
              <Button
                disabled={deleteEquipmentMutation.isPending}
                onClick={() =>
                  deleteEquipmentMutation.mutate(value.equipmentId)
                }
              >
                Delete
              </Button>
            </ItemActions>
          </Item>
        ))}
      </section>
    )
  }
}
