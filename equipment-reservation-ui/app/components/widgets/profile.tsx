import React from "react"
import { Avatar, Button, Card } from "antd"
import { UserOutlined } from "@ant-design/icons"
import { useAuth } from "react-oidc-context"
import { NavLink } from "react-router"

const Profile = () => {
  const auth = useAuth()
  console.log("auth.user", auth.user)
  return (
    <div className="flex">
      <Card
        actions={[
          <NavLink className="block" to={"/"}>
            <Button>Home</Button>
          </NavLink>,
          <Button onClick={() => auth.signoutRedirect()}>Logout</Button>,
        ]}
      >
        <Card.Meta
          avatar={<Avatar icon={<UserOutlined />} />}
          title={auth.user?.profile.name}
          description={auth.user?.profile.sub}
        />
      </Card>
    </div>
  )
}

export default Profile
