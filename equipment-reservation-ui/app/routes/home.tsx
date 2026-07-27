import { NavLink } from "react-router"
import EquipmentList from "~/components/widgets/equipment-list"
import { Button } from "antd"
import { Title } from "~/components/ui/Typography"

export default function Home() {
  return (
    <>
      <Title>Equipment Reservation</Title>
      <NavLink to="/add-equipment">
        <Button className="cursor-pointer">Add Equipment</Button>
      </NavLink>
      <EquipmentList />
    </>
  )
}
