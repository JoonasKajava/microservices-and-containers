import React from "react"
import { Avatar, Button, Card } from "antd"
import { UserOutlined } from "@ant-design/icons"
import { useAuth } from "react-oidc-context"

const Profile = () => {
  const auth = useAuth()
  console.log("auth.user", auth.user)
  return (
    <div className="flex">
      <Card
        actions={[
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
