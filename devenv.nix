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
  env.DATABASE_URL = "postgres://admin@localhost/equipment_reservation";

  # https://devenv.sh/packages/
  packages = with pkgs; [
    git
    openssl
    cargo-tarpaulin
    shadcn
    diesel-cli
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
    inventory = {
      exec = "cargo run";
      restart = {
        on = "always";
        max = null;
      };
      watch = {
        paths = [./inventory];
        extensions = ["rs" "toml"];
        ignore = ["target" "*.log"];
      };
      cwd = "./inventory/";
    };

    reservation = {
      exec = "cargo run";
      restart = {
        on = "always";
        max = null;
      };
      watch = {
        paths = [./reservation];
        extensions = ["rs" "toml"];
        ignore = ["target" "*.log"];
      };
      cwd = "./reservation/";
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
          name = "equipment_reservation";
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
            handle /api/* {
              reverse_proxy localhost:5058
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
