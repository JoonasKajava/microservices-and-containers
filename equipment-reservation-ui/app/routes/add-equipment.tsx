import Container from "~/components/ui/container"
import { useAsyncFn } from "react-use"
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
import { useEffect, useState } from "react"
import { Spinner } from "~/components/ui/spinner"
import { toast } from "sonner"

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

  const [submitState, doSubmit] = useAsyncFn(
    inventoryRepository.postEquipment,
    []
  )

  useEffect(() => {
    if (submitState.error && !submitState.loading) {
      toast.error(submitState.error.toString(), {
        position: "top-center",
        duration: 5000,
      })
    }
  }, [submitState.error, submitState.loading])

  let navigate = useNavigate()

  const onSubmit: SubmitHandler<EquipmentInputs> = (data) => {
    if (submitState.loading) return

    doSubmit(data).catch(() => console.log("test"))
  }

  console.log(errors.name)
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
              disabled={submitState.loading}
              className="cursor-pointer"
              type="submit"
            >
              {submitState.loading ? (
                <>
                  <Spinner data-icon="inline-start" />
                  Loading...
                </>
              ) : (
                <>Submit</>
              )}
            </Button>
            <Button
              disabled={submitState.loading}
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
