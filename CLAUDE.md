# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

PlasterSkull Framework is a multi-target Blazor/MAUI component library providing UI components, state management integration, error handling patterns, and cross-platform support. The solution targets .NET 9.0 (primary) with WebAssembly, Server-Side, and MAUI (Android, iOS, Mac Catalyst, Windows) deployments.

## Build Commands

### Frontend (TypeScript + SCSS)
```bash
npm install                  # Install dependencies
npm run build:Debug          # Webpack development build
npm run build:Release        # Webpack production build
npm run watch                # Watch mode during development
```

### .NET
```bash
dotnet tool restore                                          # Restore .NET tools
dotnet workload install maui                                 # Install MAUI workload (first time)
dotnet workload install wasm-tools                           # Install Wasm tools (first time)
dotnet restore build/nuget-packages.proj                     # Restore all packages
dotnet build build/nuget-packages.proj -c Release            # Build all NuGet packages
dotnet pack build/nuget-packages.proj -c Release             # Pack NuGet packages
```

### Demo App
```bash
dotnet publish ./samples/PlasterSkull.Framework.Blazor.Demo/PlasterSkull.Framework.Blazor.Demo.csproj -c Release
```

There are no test projects in this repository.

## Architecture

### Project Layout

```
src/
  Core/
    PlasterSkull.Framework.Core              # LINQ, string, task, enum, disposable extensions
    PlasterSkull.Framework.ErrorOr           # ErrorOr pattern extensions
  Blazor/
    PlasterSkull.Framework.Blazor            # Core Blazor components (context menus, icons, loaders, renderers)
    PlasterSkull.Framework.Blazor.Infrastructure  # DI services (ViewportService, ContextMenuService, BackButtonService)
    Wasm/   PlasterSkull.Framework.Blazor.Wasm
    ServerSide/ PlasterSkull.Framework.Blazor.ServerSide
    Maui/   PlasterSkull.Framework.Blazor.Maui
    Fluxor/ PlasterSkull.Framework.Blazor.Fluxor          # Actions, Effects, Subscriptions
    Fluxor/ PlasterSkull.Framework.Blazor.Fluxor.ErrorOr  # Fluxor + ErrorOr bridge
samples/
  PlasterSkull.Framework.Blazor.Demo        # Blazor WASM demo app
build/
  nuget-packages.proj                       # Traversal project for all NuGet outputs
  nuget-demo-packages.proj
  .github/workflows/                        # CI/CD (nuget-publish, nuget-demo-publish)
```

### Key Patterns

**Component Base**: All Razor components extend `PsComponentBase`, which provides CSS class building, styling helpers, and lifecycle management.

**Service Registration**: Each project exposes a `Configure.cs` with extension methods (e.g., `AddPlasterSkullBlazor()`) for DI setup. Always register services through these extension methods.

**State Management**: Fluxor is the state management layer. New state flows follow the Actions → Effects → Store pattern in `PlasterSkull.Framework.Blazor.Fluxor`.

**Error Handling**: Uses the `ErrorOr` result type (not exceptions) for service-layer returns. The `Fluxor.ErrorOr` project bridges these into Fluxor effects.

**Reactive Services**: `ViewportService`, `BackButtonService`, and navigation services use observer/subscriber patterns — subscribe in `OnInitializedAsync` and dispose subscriptions properly.

### Build Configuration

- `Directory.Build.props` — global MSBuild properties and NuGet metadata; package version is defined here
- `Directory.Packages.props` — centralized NuGet version pinning (all `<PackageVersion>` entries live here; projects use `<PackageReference>` without versions)
- `src/Directory.Build.props` — source-specific build overrides
- `global.json` — pins the .NET SDK version
- `tsconfig.json` — TypeScript compiler (ES5 output, ES6 module resolution)
- `webpack.config.js` — compiles `.ts` and `.scss` under `src/Blazor/PlasterSkull.Framework.Blazor/`

### Key Dependencies

| Package | Purpose |
|---|---|
| MudBlazor 8.x | Material Design component library |
| Fluxor 6.x | Redux-like state management |
| ActualLab.Fusion 11.x | Reactive distributed computing |
| ErrorOr 2.x | Result type for error handling |
| FluentValidation 11.x | Validation |
| MAUI 9.x | Cross-platform mobile targets |

### Adding a New Package Dependency

Add the version to `Directory.Packages.props`, then reference the package (without version) in the relevant `.csproj`. Never specify versions directly in `.csproj` files.
