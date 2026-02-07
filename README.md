# Mercator -- Local Development Guide

This repository contains **Mercator**, a **modular monolith** built with
**.NET 10** and orchestrated locally using **.NET Aspire**.

The goal of this setup is simple:

> **One command → fully working local environment**\
> *(API + database + frontend + observability)*

This guide explains how to run the project on **Windows** and **macOS**.

------------------------------------------------------------------------

## What Aspire Does for This Project

Aspire acts as the *local orchestrator* for Mercator:

-   Starts the backend API
-   Starts PostgreSQL
-   Starts the React frontend (Vite)
-   Wires connection strings automatically
-   Wires frontend → backend networking
-   Provides a local dashboard (logs, traces, health)

You do **not** need Docker Compose files or manual env configuration to
get started.

------------------------------------------------------------------------

## Project Structure (Simplified)

    mercator/
    ├─ src/
    │  ├─ Mercator.AppHost        # Aspire AppHost (entry point)
    │  ├─ Mercator.Bootstrapper   # Backend API
    │  └─ web/                    # React + Vite frontend

------------------------------------------------------------------------

## Prerequisites (Both Windows & macOS)

### Required

-   **.NET SDK 10.0**
-   **.NET Aspire workload**
-   **Node.js (LTS, v24.13.0)**
-   **Docker Desktop** (required for PostgreSQL)

### Recommended

-   A modern IDE (Visual Studio / Rider / VS Code)

------------------------------------------------------------------------

## Installing Prerequisites

### 1. Install .NET 10 SDK

Download from:\
https://dotnet.microsoft.com/download

Verify installation:

``` bash
dotnet --version
```

------------------------------------------------------------------------

### 2. Install .NET Aspire Workload

``` bash
dotnet workload install aspire
```

Verify:

``` bash
dotnet workload list
```

------------------------------------------------------------------------

### 3. Install Node.js

Download Node.js LTS from:\
https://nodejs.org

Verify:

``` bash
node --version
npm --version
```

------------------------------------------------------------------------

## Running Mercator

### 0. Start Docker

Ensure Docker Desktop is running.

Verify:

``` bash
docker version
```

------------------------------------------------------------------------

### 1. Clone the repository

``` bash
git clone https://github.com/maitre-forgeron/mercator.git
cd mercator
```

------------------------------------------------------------------------

### 2. Restore dependencies

``` bash
dotnet restore
```

------------------------------------------------------------------------

### 3. Run everything with Aspire

``` bash
aspire run
```

Equivalent:

``` bash
dotnet run --project src/Mercator.AppHost
```

This starts:

-   PostgreSQL
-   Backend API
-   Frontend (React + Vite)
-   Aspire dashboard

------------------------------------------------------------------------

## Aspire Dashboard

Usually available at:

    https://localhost:18888

From the dashboard you can:

-   View logs
-   Inspect traces
-   Check service health
-   Open frontend & API endpoints

------------------------------------------------------------------------

## Frontend (React + Vite)

The frontend runs via Vite and is managed by Aspire.

Access it using the URL shown in the Aspire dashboard, for example:

    http://localhost:60xxx

> The port is dynamically assigned.

### Frontend → API communication

-   Frontend calls APIs using relative paths:

    ``` ts
    fetch("/health");
    ```

-   Vite proxies `/api/*` requests to the backend

-   Everything is same-origin → **no CORS required**

------------------------------------------------------------------------

## API Access

API URL is printed by Aspire, for example:

    http://localhost:5xxx

Swagger (if enabled):

    http://localhost:5xxx/swagger

------------------------------------------------------------------------

## Database

-   PostgreSQL runs in Docker
-   Connection strings are injected automatically
-   No local DB setup required

------------------------------------------------------------------------

## Common Commands

Stop everything:

    Ctrl + C

Clean & rebuild:

``` bash
dotnet clean
dotnet build
dotnet run --project src/Mercator.AppHost
```

------------------------------------------------------------------------

## Troubleshooting

### Aspire workload not found

``` bash
dotnet workload install aspire
```

### Frontend does not start

-   Ensure Node.js is installed
-   Check frontend logs in Aspire dashboard

### Port conflicts

Restart the AppHost.

### Database errors

Check Aspire dashboard logs.
