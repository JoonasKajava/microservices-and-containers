{
  pkgs,
  lib,
  config,
  inputs,
  ...
}: rec {
  # https://devenv.sh/basics/
  # env.GREET = "devenv";

  env.INVENTORY_ADDR = "http://localhost:3000";
  env.VITE_INVENTORY_ADDR = env.INVENTORY_ADDR;

  env.VITE_OIDC_AUTHORITY = "https://auth.equipment.localhost";
  env.VITE_OIDC_CLIENT_ID = "420a32cb-5100-4bee-b770-e273e059e326";
  env.VITE_OIDC_REDIRECT_URI = "https://equipment.localhost";
  env.DATABASE_URL = "postgres://admin@localhost/equipment_reservation";

  # https://devenv.sh/packages/
  packages = with pkgs; [
    git
    openssl
    cargo-tarpaulin
    shadcn
    sea-orm-cli
  ];

  # https://devenv.sh/languages/
  languages = {
    rust = {
      enable = true;
      channel = "stable";
      targets = ["x86_64-unknown-linux-gnu" "x86_64-unknown-linux-musl"];
    };
  };

  process.manager.implementation = "process-compose";

  processes = {
    ui = {
      exec = "npm run dev";
      cwd = "./equipment-reservation-ui/";
    };
  };

  # https://devenv.sh/processes/
  # processes.dev.exec = "${lib.getExe pkgs.watchexec} -n -- ls -la";

  # https://devenv.sh/services/
  services = {
    postgres = {
      listen_addresses = "localhost";
      enable = true;
      initialDatabases = [
        {
          name = "inventory";
          user = "admin";
        }
        {
          name = "reservation";
          user = "admin";
        }
      ];
    };
    caddy = {
      enable = true;
      config = ''
        {
            http_port 8080
            https_port 8443
        }
      '';
      virtualHosts = {
        "localhost:8080" = {
          extraConfig = ''
            handle /api/v1/equipment* {
              reverse_proxy localhost:5058
            }

            handle /api/v1/reservation* {
              reverse_proxy localhost:5264
            }
            handle {
              reverse_proxy localhost:5173
            }
          '';
        };
      };
    };
  };

  # https://devenv.sh/scripts/
  # scripts.hello.exec = ''
  #   echo hello from $GREET
  # '';

  # https://devenv.sh/basics/
  # enterShell = ''
  #   hello         # Run scripts directly
  #   git --version # Use packages
  # '';

  # https://devenv.sh/tasks/
  # tasks = {
  #   "myproj:setup".exec = "mytool build";
  #   "devenv:enterShell".after = [ "myproj:setup" ];
  # };

  # https://devenv.sh/tests/
  # enterTest = ''
  #   echo "Running tests"
  #   git --version | grep --color=auto "${pkgs.git.version}"
  # '';

  # https://devenv.sh/git-hooks/
  # git-hooks.hooks.shellcheck.enable = true;

  # See full reference at https://devenv.sh/reference/options/
}
