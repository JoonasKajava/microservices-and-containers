dev-rebuild:
  sudo docker compose -f compose.prod.yaml -f compose.dev.yaml up --build -d

dev-config:
  sudo docker compose -f compose.prod.yaml -f compose.dev.yaml config
