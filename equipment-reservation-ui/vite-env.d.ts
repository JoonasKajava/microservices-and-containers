interface ImportMetaEnv {
  readonly VITE_INVENTORY_ADDR: string
  readonly VITE_OIDC_AUTHORITY: string
  readonly VITE_OIDC_CLIENT_ID: string
  readonly VITE_OIDC_REDIRECT_URI: string
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}
