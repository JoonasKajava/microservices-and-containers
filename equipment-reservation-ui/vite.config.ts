import { reactRouter } from "@react-router/dev/vite"
import tailwindcss from "@tailwindcss/vite"
import { defineConfig } from "vite"

export default defineConfig({
  server: {
    host: "0.0.0.0",
    port: 3000,
    cors: {
      origin: ["https://equipment.localhost", "https://auth.equipment.localhost"],
      credentials: true
    }
  },
  resolve: { tsconfigPaths: true },
  plugins: [tailwindcss(), reactRouter()],
})
