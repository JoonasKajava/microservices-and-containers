import { type RouteConfig, index, route } from "@react-router/dev/routes"

export default [
  index("routes/home.tsx"),
  route("add-equipment", "./routes/add-equipment.tsx"),
  route("view-equipment/:equipmentId", "./routes/view-equipment.tsx"),
] satisfies RouteConfig
