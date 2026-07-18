import { useAsync } from "react-use"
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
import { useEffect } from "react"
import { Button } from "../ui/button"

export default function EquipmentList() {
  const equipmentReadState = useAsync(inventoryRepository.readEquipment)

  if (equipmentReadState.loading) {
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
  } else if (equipmentReadState.value && equipmentReadState.value.length < 1) {
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
  } else if (equipmentReadState.error !== undefined) {
    return (
      <Empty>
        <EmptyHeader>
          <EmptyMedia variant="icon">
            <BookDashed />
          </EmptyMedia>
          <EmptyTitle>Issue getting equipment list</EmptyTitle>
          <EmptyDescription>
            {equipmentReadState.error.toString()}
          </EmptyDescription>
        </EmptyHeader>
      </Empty>
    )
  } else {
    return (
      <section className="flex flex-col gap-4">
        {equipmentReadState.value?.map((value) => (
          <Item key={value.name} variant="outline">
            <ItemContent>
              <ItemTitle>{value.name}</ItemTitle>
              <ItemDescription className="flex flex-col gap-2">
                {value.description}
              </ItemDescription>
            </ItemContent>
            <ItemActions>
              <Button>Reserve</Button>
            </ItemActions>
          </Item>
        ))}
      </section>
    )
  }
}
