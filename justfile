dev-rebuild:
  sudo docker compose -f compose.yaml up --build -d

dev-config:
  sudo docker compose -f compose.prod.yaml -f compose.dev.yaml config
