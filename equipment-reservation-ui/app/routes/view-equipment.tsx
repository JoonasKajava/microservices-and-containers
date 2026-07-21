import React, { useCallback } from "react"
import Container from "~/components/ui/container"
import { useParams } from "react-router"
import { useQuery } from "@tanstack/react-query"
import inventoryRepository from "~/lib/repositories/inventoryRepository"
import { Skeleton } from "~/components/ui/skeleton"
import SimpleError from "~/components/widgets/SimpleError"
import { FieldGroup, FieldLegend, FieldSet } from "~/components/ui/field"

const ViewEquipment = () => {
  const { equipmentId } = useParams<{ equipmentId: string }>()

  const equipmentQuery = useQuery({
    queryKey: ["equipment", equipmentId],
    queryFn: () => inventoryRepository.readEquipmentById(equipmentId!),
  })

  const property = useCallback(
    (value: string) => {
      if (equipmentQuery.isPending) {
        return <Skeleton className="h-4 w-62.5" />
      }
      if (!equipmentQuery.isSuccess) return <></>

      return value
    },
    [equipmentQuery.isPending, equipmentQuery.isSuccess]
  )

  return (
    <Container>
      <FieldSet>
        <FieldLegend>View Equipment</FieldLegend>
        <FieldGroup>
          <FieldLegend>Equipment Id</FieldLegend>
          <p>{equipmentId}</p>
        </FieldGroup>
      </FieldSet>
      {equipmentQuery.isError && (
        <SimpleError
          title="Issue getting equipment details"
          description={equipmentQuery.error.message}
        />
      )}

      {(equipmentQuery.isPending || equipmentQuery.isSuccess) && (
        <>
          <p>Equipment Name: {property(equipmentQuery.data?.name!)}</p>
          <p>
            Equipment Description: {property(equipmentQuery.data?.description!)}
          </p>
        </>
      )}
    </Container>
  )
}

export default ViewEquipment
