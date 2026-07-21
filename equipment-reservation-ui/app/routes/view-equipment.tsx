import React, { useMemo } from "react"
import Container from "~/components/ui/container"
import { useParams } from "react-router"
import { useQuery } from "@tanstack/react-query"
import inventoryRepository from "~/lib/repositories/inventoryRepository"
import { Descriptions, Empty, Skeleton } from "antd"
import type { DescriptionsItemType } from "antd/lib/descriptions"
import type { ApiError, Equipment } from "~/lib/types"
import { Title } from "~/components/ui/Typography"

const ViewEquipment = () => {
  const { equipmentId } = useParams<{ equipmentId: string }>()

  const equipmentQuery = useQuery<Equipment, ApiError>({
    queryKey: ["equipment", equipmentId],
    queryFn: () => inventoryRepository.readEquipmentById(equipmentId!),
  })

  const details: DescriptionsItemType[] = useMemo(() => {
    const dataOrSkeleton = (key: keyof Equipment) => {
      if (equipmentQuery.isSuccess) {
        return <p>{equipmentQuery.data[key]}</p>
      } else {
        return <Skeleton />
      }
    }
    return [
      {
        label: "Id",
        children: dataOrSkeleton("equipmentId"),
      },
      {
        label: "Name",
        children: dataOrSkeleton("name"),
      },
      {
        label: "Description",
        children: dataOrSkeleton("description"),
      },
    ]
  }, [equipmentQuery.isSuccess, equipmentQuery.data])

  return (
    <Container>
      <Title>View Equipment</Title>
      {(equipmentQuery.isLoading || equipmentQuery.isSuccess) && (
        <Descriptions items={details} />
      )}
      {equipmentQuery.isError && equipmentQuery.error.status === 404 && (
        <Empty />
      )}
    </Container>
  )
}

export default ViewEquipment
