import { type RouteConfig, index, route } from "@react-router/dev/routes"

export default [
  index("routes/home.tsx"),
  route("add-equipment", "./routes/add-equipment.tsx"),
] satisfies RouteConfig
