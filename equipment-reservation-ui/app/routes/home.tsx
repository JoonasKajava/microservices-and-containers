import { NavLink } from "react-router"
import { Button } from "~/components/ui/button"
import Container from "~/components/ui/container"
import EquipmentList from "~/components/widgets/equipment-list"

export default function Home() {

  return (
    <Container>
      <h1>Equipment Reservation</h1>
      <NavLink to="/add-equipment">
        <Button className="cursor-pointer">Add Equipment</Button>
      </NavLink>

      <EquipmentList />
      
    </Container>
  )
}
