import React from "react"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import reservationRepository from "~/lib/repositories/reservationRepository"
import { App, Button, Table } from "antd"
import type { Reservation } from "~/lib/types"

const ListReservations = (props: { equipmentId: string }) => {
  const { notification } = App.useApp()

  const queryClient = useQueryClient()

  const equipmentQuery = useQuery({
    queryKey: ["reservations", props.equipmentId],
    queryFn: () => reservationRepository.readReservations(props.equipmentId),
  })

  const cancelReservationMutation = useMutation({
    mutationFn: reservationRepository.deleteReservation,
    onSuccess: () => {
      notification.success({
        title: "Reservation canceled successfully",
        placement: "top",
        duration: 5,
      })
      queryClient
        .invalidateQueries({
          queryKey: ["reservations", props.equipmentId],
        })
        .catch(console.error)
    },
    onError: () => {
      notification.error({
        title: "Reservation cancellation failed",
        placement: "top",
        duration: 5,
      })
    },
  })

  const columns = [
    {
      title: "Start Time",
      dataIndex: "startTime",
      key: "startTime",
      render: (_: any, record: Reservation) =>
        new Date(record.startTime).toLocaleString(),
    },
    {
      title: "End Time",
      dataIndex: "endTime",
      key: "endTime",
      render: (_: any, record: Reservation) =>
        new Date(record.endTime).toLocaleString(),
    },
    {
      title: "Action",
      key: "action",
      render: (_: any, record: Reservation) => (
        <Button
          onClick={() => cancelReservationMutation.mutate(record.reservationId)}
          danger
        >
          Cancel
        </Button>
      ),
    },
  ]

  return (
    <Table<Reservation>
      rowKey={(record) => record.reservationId}
      columns={columns}
      dataSource={equipmentQuery.data}
    />
  )
}

export default ListReservations
