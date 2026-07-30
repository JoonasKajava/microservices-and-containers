dev-rebuild:
  sudo docker compose -f compose.prod.yaml -f compose.dev.yaml up --build -d --remove-orphans

dev-config:
  sudo docker compose -f compose.prod.yaml -f compose.dev.yaml config
