import React from "react"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { App, Button, Dropdown, Table } from "antd"
import { type Reservation, ReservationStatus } from "~/lib/types"
import { useAuth } from "react-oidc-context"
import { useReservationRepository } from "~/lib/hooks/useReservationRepository"
import { DownOutlined } from "@ant-design/icons"

const ListReservations = (props: { equipmentId: string }) => {
  const { notification } = App.useApp()

  const auth = useAuth()
  const reservationRepository = useReservationRepository(
    auth.user?.access_token!
  )

  const queryClient = useQueryClient()

  const equipmentQuery = useQuery({
    queryKey: ["reservations", props.equipmentId],
    queryFn: () => reservationRepository.readReservations(props.equipmentId),
  })

  const updateReservationMutation = useMutation({
    mutationFn: (variables: {
      id: string
      action: "cancel" | "return" | "start"
    }) => {
      if (variables.action === "cancel")
        return reservationRepository.deleteReservation(variables.id)
      return reservationRepository.changeReservationStatus(
        variables.id,
        variables.action === "start"
          ? ReservationStatus.Started
          : ReservationStatus.Returned
      )
    },
    onSuccess: () => {
      notification.success({
        title: "Reservation status changed successfully",
        placement: "top",
        duration: 5,
      })
      queryClient
        .invalidateQueries({
          queryKey: ["reservations", props.equipmentId],
        })
        .catch(console.error)

      queryClient
        .invalidateQueries({
          queryKey: ["equipment", props.equipmentId],
        })
        .catch(console.error)
    },
    onError: () => {
      notification.error({
        title: "Reservation status change failed",
        placement: "top",
        duration: 5,
      })
    },
  })

  const columns = [
    {
      title: "Status",
      dataIndex: "status",
      key: "status",

      render: (_: any, record: Reservation) =>
        ReservationStatus[record.status]
    },
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
      title: "User",
      dataIndex: "reservedBy",
      key: "reservedBy",
    },
    {
      title: "Actions",
      key: "actions",
      render: (_: any, record: Reservation) => (
        <Dropdown
          menu={{
            items: [
              {
                label: "Cancel",
                key: "cancel",
              },
              {
                label: "Start",
                key: "start",
                disabled: record.status !== ReservationStatus.Reserved,
              },
              {
                label: "Return",
                key: "return",
                disabled: record.status !== ReservationStatus.Started,
              },
            ],
            onClick: (e) => {
              updateReservationMutation.mutate({
                id: record.reservationId,
                action: e.key as "cancel" | "return" | "start",
              })
            },
          }}
        >
          <Button icon={<DownOutlined />} iconPlacement="end">
            Change Status
          </Button>
        </Dropdown>
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
