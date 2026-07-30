import React from "react"
import { Alert } from "antd"

const SimpleError = (props: { title: string; description: string }) => {
  return (
    <Alert
      title={props.title}
      description={props.description}
      type="error"
      showIcon
    />
  )
}

export default SimpleError