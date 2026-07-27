import React, { useMemo } from "react"
import { useParams } from "react-router"
import { useQuery } from "@tanstack/react-query"
import { Descriptions, Divider, Empty, Skeleton, Space } from "antd"
import type { DescriptionsItemType } from "antd/lib/descriptions"
import type { ApiError, Equipment } from "~/lib/types"
import { Title } from "~/components/ui/Typography"
import NewReservation from "~/components/widgets/new-reservation"
import ListReservations from "~/components/widgets/list-reservations"
import { useInventoryRepository } from "~/lib/hooks/useInventoryRepository"
import { useAuth } from "react-oidc-context"

const ViewEquipment = () => {
  const auth = useAuth()
  const inventoryRepository = useInventoryRepository(auth.user?.access_token!)

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
      {
        label: "Creator",
        children: dataOrSkeleton("creator"),
      },
    ]
  }, [equipmentQuery.isSuccess, equipmentQuery.data])

  return (
    <>
      <Space orientation="vertical">
        <Title>View Equipment</Title>
        {(equipmentQuery.isLoading || equipmentQuery.isSuccess) && (
          <Descriptions items={details} />
        )}
        {equipmentQuery.isError && equipmentQuery.error.status === 404 && (
          <Empty />
        )}
        <Divider />
        <NewReservation equipmentId={equipmentId!} />
        <Divider />
        <ListReservations equipmentId={equipmentId!} />
      </Space>
    </>
  )
}

export default ViewEquipment
