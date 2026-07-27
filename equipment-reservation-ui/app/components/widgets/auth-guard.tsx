import React from "react"
import { useAuth } from "react-oidc-context"
import { Button, Result } from "antd"
import { LoginOutlined } from "@ant-design/icons"

const AuthGuard = ({ children }: { children: React.ReactNode }) => {
  const auth = useAuth()

  if (auth.isAuthenticated) return children

  return (
    <Result
      icon={<LoginOutlined />}
      title="You must be logged in to access this site."
      extra={
        <Button onClick={() => auth.signinRedirect()} type="primary">
          Login
        </Button>
      }
    />
  )
}

export default AuthGuard
