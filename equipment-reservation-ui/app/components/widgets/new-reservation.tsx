import React, { useCallback } from "react"
import { App, Button, DatePicker, Form } from "antd"
import { useMutation } from "@tanstack/react-query"
import reservationRepository from "~/lib/repositories/reservationRepository"
import dayjs from "dayjs"

const NewReservation = (props: { equipmentId: string }) => {
  const [form] = Form.useForm()
  const { notification } = App.useApp()

  const createReservationMutation = useMutation({
    mutationFn: reservationRepository.postReservation,
    onSuccess: () => {
      notification.success({
        title: "Reservation created successfully",
        placement: "top",
        duration: 5,
      })
      form.resetFields()
    },
    onError: () => {
      notification.error({
        title: "Reservation creation failed",
        placement: "top",
        duration: 5,
      })
    },
  })

  const onFinish = useCallback(
    (values: any) => {
      createReservationMutation.mutate({
        equipmentId: props.equipmentId,
        startTime: values.timespan[0],
        endTime: values.timespan[1],
      })
    },
    [createReservationMutation, props.equipmentId]
  )

  return (
    <Form form={form} onFinish={onFinish}>
      <Form.Item
        name="timespan"
        label="Reservation Timespan"
        rules={[{ required: true }]}
      >
        <DatePicker.RangePicker minDate={dayjs()} showTime />
      </Form.Item>
      <Form.Item>
        <Button
          disabled={createReservationMutation.isPending}
          type="primary"
          htmlType="submit"
        >
          Submit
        </Button>
      </Form.Item>
    </Form>
  )
}

export default NewReservation
