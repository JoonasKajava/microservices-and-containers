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

  const [submitState, doSubmit] = useAsyncFn(inventoryRepository.postEquipment, [])

  useEffect(() => {
    if (submitState.error && !submitState.loading) {
      toast.error(submitState.error.toString(), {position: "top-center", duration: 5000})
    }
  }, [submitState.error, submitState.loading])

  let navigate = useNavigate()

  const onSubmit: SubmitHandler<EquipmentInputs> = (data) => {
    if (submitState.loading) return

    doSubmit(data).catch(() => console.log("test"))
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
              {...register("name", { required: true })}
            />
            {errors.name && <FieldError>Equipment name is required</FieldError>}
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
