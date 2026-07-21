import Container from "~/components/ui/container"
import { useNavigate } from "react-router"
import inventoryRepository from "~/lib/repositories/inventoryRepository"
import { useMutation } from "@tanstack/react-query"
import Title from "antd/lib/typography/Title"
import { Button, Form, Input, notification, Space } from "antd"
import TextArea from "antd/lib/input/TextArea"

export default function AddEquipment() {

  const [api, contextHolder] = notification.useNotification();

  const [form] = Form.useForm()

  const createEquipmentMutation = useMutation({
    mutationFn: inventoryRepository.postEquipment,
    onSuccess: () => {
      api.success({
        title: "Equipment created successfully",
        placement: "top",
        duration: 5,
      });
      navigate(-1)
    },
    onError: (error) => {
      api.error({
        title: "Equipment creation failed",
        placement: "top",
        duration: 5,
      });
    },
  })

  let navigate = useNavigate()

  const onSubmit = (data) => {
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
          <TextArea placeholder="Ergonomic chair..." />
        </Form.Item>
        ,
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
