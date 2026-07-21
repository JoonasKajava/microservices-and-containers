import Container from "~/components/ui/container"
import { useNavigate } from "react-router"
import inventoryRepository from "~/lib/repositories/inventoryRepository"
import { useMutation } from "@tanstack/react-query"
import { App, Button, Form, Input, Space } from "antd"
import { Title } from "~/components/ui/Typography"

export default function AddEquipment() {
  const { notification } = App.useApp()

  const [form] = Form.useForm()

  const createEquipmentMutation = useMutation({
    mutationFn: inventoryRepository.postEquipment,
    onSuccess: () => {
      notification.success({
        title: "Equipment created successfully",
        placement: "top",
        duration: 5,
      })
      navigate(-1)
    },
    onError: (error) => {
      notification.error({
        title: "Equipment creation failed",
        placement: "top",
        duration: 5,
      })
    },
  })

  let navigate = useNavigate()

  const onSubmit = (data: any) => {
    console.log(data)
    if (createEquipmentMutation.isPending) return

    createEquipmentMutation.mutate(data)
  }

  return (
    <Container>
      <Title>Add Equipment</Title>
      <Form form={form} onFinish={onSubmit}>
        <Form.Item
          name="name"
          label="Equipment Name"
          rules={[
            {
              required: true,
              message: "Equipment name is required",
            },
            {
              max: 255,
              message: "Max length for name is 255",
            },
          ]}
        >
          <Input placeholder="Office Chair..." />
        </Form.Item>
        <Form.Item
          name="Description"
          label="Equipment Description"
          rules={[
            {
              max: 1024,
              message: "Max length for description is 1024",
            },
          ]}
        >
          <Input.TextArea placeholder="Ergonomic chair..." />
        </Form.Item>
        <Form.Item>
          <Space>
            <Button
              disabled={createEquipmentMutation.isPending}
              type="primary"
              htmlType="submit"
            >
              Submit
            </Button>
            <Button
              disabled={createEquipmentMutation.isPending}
              htmlType="submit"
              onClick={() => navigate(-1)}
            >
              Cancel
            </Button>
          </Space>
        </Form.Item>
      </Form>
    </Container>
  )
}
