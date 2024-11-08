{
  description = "Development flake for Unity and Neovim";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-24.05";
    nixpkgs-unstable.url = "github:NixOS/nixpkgs/nixos-unstable";
    utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, nixpkgs-unstable, utils }:
    utils.lib.eachDefaultSystem(system:
      let
        pkgs = import nixpkgs { 
          inherit system; 
          config.allowUnfree = true; # enable the use of proprietary packages
        };
        pkgs-unstable = import nixpkgs-unstable { 
          inherit system; 
        };
      in
      {
        devShell = with pkgs; mkShell {
          buildInputs = [
            (unityhub.override { extraLibs = { ... }: [ harfbuzz ]; })
            dotnetCorePackages.dotnet_8.sdk
            dotnetCorePackages.dotnet_8.runtime
            csharp-ls
            pkgs-unstable.neovim
            # (neovim.override { 
            #   configure.customRC = ''
            #     luafile ~/.config/nvim/init.lua
            #     lua require("lspconfig").csharp_ls.setup({ capabilities = require("cmp_nvim_lsp").default_capabilities() })
            #   ''; 
            # })
          ];
          shellHook = ''
            zsh
          '';
        };
      });
}
