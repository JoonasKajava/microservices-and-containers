import {
  isRouteErrorResponse,
  Links,
  Meta,
  NavLink,
  Outlet,
  Scripts,
  ScrollRestoration,
} from "react-router"

import { App as AntdApp, Button, Card, Space } from "antd"

import type { Route } from "./+types/root"
import "./app.css"
import { QueryClient, QueryClientProvider } from "@tanstack/react-query"
import Container from "~/components/ui/container"
import {
  AuthProvider,
  type AuthProviderProps,
  useAuth,
} from "react-oidc-context"
import { useEffect } from "react"

const queryClient = new QueryClient()

const oidcConfig = {
  authority: import.meta.env.VITE_OIDC_AUTHORITY,
  client_id: import.meta.env.VITE_OIDC_CLIENT_ID,
  redirect_uri: import.meta.env.VITE_OIDC_REDIRECT_URI,
  response_type: "code",
  scope: "openid profile email",
  disablePKCE: false
} satisfies AuthProviderProps

export function Layout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <head>
        <meta charSet="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <Meta />
        <Links />
      </head>
      <body>
        <AuthProvider {...oidcConfig}>
          <Wrapper>{children}</Wrapper>
        </AuthProvider>
      </body>
    </html>
  )
}

const Wrapper = ({ children }: { children: React.ReactNode }) => {
  const auth = useAuth()
  console.log(auth)
  console.log(oidcConfig)
  return (
    <AntdApp>
      <QueryClientProvider client={queryClient}>
        <Container>
          <Space>
            <Button onClick={() => auth.signinRedirect()}>Login</Button>
            <Button onClick={() => auth.signoutRedirect()}>Logout</Button>
          </Space>
          <NavLink className="block" to={"/"}>
            <Button>Home</Button>
          </NavLink>
          {children}
        </Container>
        <ScrollRestoration />
        <Scripts />
      </QueryClientProvider>
    </AntdApp>
  )
}

export default function App() {
  return <Outlet />
}

export function ErrorBoundary({ error }: Route.ErrorBoundaryProps) {
  let message = "Oops!"
  let details = "An unexpected error occurred."
  let stack: string | undefined

  if (isRouteErrorResponse(error)) {
    message = error.status === 404 ? "404" : "Error"
    details =
      error.status === 404
        ? "The requested page could not be found."
        : error.statusText || details
  } else if (import.meta.env.DEV && error && error instanceof Error) {
    details = error.message
    stack = error.stack
  }

  return (
    <main className="container mx-auto p-4 pt-16">
      <h1>{message}</h1>
      <p>{details}</p>
      {stack && (
        <pre className="w-full overflow-x-auto p-4">
          <code>{stack}</code>
        </pre>
      )}
    </main>
  )
}
