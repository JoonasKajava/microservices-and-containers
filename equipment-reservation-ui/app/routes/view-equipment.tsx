import React, { useCallback } from "react"
import Container from "~/components/ui/container"
import { useParams } from "react-router"
import { useQuery } from "@tanstack/react-query"
import inventoryRepository from "~/lib/repositories/inventoryRepository"
import { Skeleton } from "~/components/ui/skeleton"

const ViewEquipment = () => {
  const { equipmentId } = useParams<{ equipmentId: string }>()

  const equipmentQuery = useQuery({
    queryKey: ["equipment", equipmentId],
    queryFn: () => inventoryRepository.readEquipmentById(equipmentId!),
  })

  const property = useCallback(
    (value: string) => {
      if (equipmentQuery.isPending) {
        return <Skeleton className="h-4 w-[250px]" />
      }
      if (!equipmentQuery.isSuccess) return <></>

      return value
    },
    [equipmentQuery.isPending, equipmentQuery.isSuccess]
  )

  return (
    <Container>
      <h1>View Equipment</h1>
      <p>Equipment Id: {equipmentId}</p>
      <p>Equipment Name: {property(equipmentQuery.data?.name!)}</p>
      <p>
        Equipment Description: {property(equipmentQuery.data?.description!)}
      </p>
    </Container>
  )
}

export default ViewEquipment
