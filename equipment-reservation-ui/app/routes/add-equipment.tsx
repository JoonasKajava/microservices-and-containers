import Container from "~/components/ui/container"
import { useForm, type SubmitHandler } from "react-hook-form"
import {
  Field,
  FieldError,
  FieldGroup,
  FieldLabel,
} from "~/components/ui/field"
import { Input } from "~/components/ui/input"
import { Button } from "~/components/ui/button"
import { useNavigate } from "react-router"
import inventoryRepository from "~/lib/repositories/inventoryRepository"
import { Spinner } from "~/components/ui/spinner"
import { toast } from "sonner"
import { useMutation } from "@tanstack/react-query"

type EquipmentInputs = {
  name: string
  description: string
}

export default function AddEquipment() {
  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm<EquipmentInputs>()

  const createEquipmentMutation = useMutation({
    mutationFn: inventoryRepository.postEquipment,
    onSuccess: () => {
      toast.success("Equipment Created", {
        position: "top-center",
      })
      navigate(-1)
    },
    onError: (error) => {
      toast.error("Equipment creation failed", {
        position: "top-center",
        duration: 5000,
      })
    },
  })

  let navigate = useNavigate()

  const onSubmit: SubmitHandler<EquipmentInputs> = (data) => {
    if (createEquipmentMutation.isPending) return

    createEquipmentMutation.mutate(data)

  }

  return (
    <Container>
      <h1>Add Equipment</h1>
      <form onSubmit={handleSubmit(onSubmit)}>
        <FieldGroup>
          <Field>
            <FieldLabel>Equipment Name</FieldLabel>
            <Input
              placeholder="Office Chair..."
              {...register("name", {
                required: {
                  value: true,
                  message: "Equipment name is required",
                },
                maxLength: {
                  value: 255,
                  message: "Max length for name is 255",
                },
              })}
            />
            {errors.name && <FieldError>{errors.name.message}</FieldError>}
          </Field>

          <Field>
            <FieldLabel>Description</FieldLabel>
            <Input
              placeholder="Ergonomic chair..."
              {...register("description")}
            />
          </Field>

          <Field orientation="horizontal">
            <Button
              disabled={createEquipmentMutation.isPending}
              className="cursor-pointer"
              type="submit"
            >
              {createEquipmentMutation.isPending ? (
                <>
                  <Spinner data-icon="inline-start" />
                  Loading...
                </>
              ) : (
                <>Submit</>
              )}
            </Button>
            <Button
              disabled={createEquipmentMutation.isPending}
              className="cursor-pointer"
              onClick={() => navigate(-1)}
              type="button"
              variant="outline"
            >
              Cancel
            </Button>
          </Field>
        </FieldGroup>
      </form>
    </Container>
  )
}
