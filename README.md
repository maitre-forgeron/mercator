# Mercator – Local Development Guide

This repository contains **Mercator**, a **modular monolith** built with **.NET 10** and orchestrated locally using **.NET Aspire**.

The goal of this setup is simple:

> **One command → fully working local environment** (API + database + observability).

This guide explains how to run the project on **Windows** and **macOS**.

---

## What Aspire Does for This Project

Aspire acts as the *local orchestrator* for Mercator:

- Starts the backend API
- Starts PostgreSQL
- Wires connection strings automatically
- Provides a local dashboard (logs, traces, health)

You do **not** need Docker Compose files or manual env configuration to get started.

---

## Prerequisites (Both Windows & macOS)

Make sure the following tools are installed:

### Required
- **.NET SDK 10.0**
- **.NET Aspire workload**

### Required
- Docker Desktop (required for PostgreSQL)

### Recommended
- A modern IDE (Visual Studio / Rider / VS Code)

---

## Installing Prerequisites

### 1. Install .NET 10 SDK

Download from:
https://dotnet.microsoft.com/download

Verify installation (should print **10.x**):

```bash
dotnet --version
```

---

### 2. Install .NET Aspire Workload

Run once on your machine:

```bash
dotnet workload install aspire
```

Verify:

```bash
dotnet workload list
```

You should see `aspire` in the list.

---

## Running Mercator (Windows & macOS)

### 0. Start Docker

Make sure Docker is running **before** you start the AppHost.

- **Windows:** Start **Docker Desktop** and wait until it shows “Running”.
- **macOS:** Start **Docker Desktop** and wait until it shows “Running”.

Verify from terminal:

```bash
docker version
```

If this prints client/server info, you’re good.

---

## Running Mercator (Windows & macOS)

### 1. Clone the repository

```bash
git clone <repository-url>
cd mercator
```

---

### 2. Restore dependencies

```bash
dotnet restore
```

---

### 3. Run with Aspire

From the repo root, run:

```bash
aspire run
```

This starts the **Mercator.AppHost**, which then orchestrates everything (API + PostgreSQL + dashboard).

> If you prefer the explicit .NET command, this is equivalent:
>
> ```bash
> dotnet run --project src/Mercator.AppHost
> ```

What happens next:

- PostgreSQL starts automatically
- Mercator API starts
- Database connection strings are injected
- Aspire Dashboard opens in your browser

---

## Aspire Dashboard

Once the AppHost is running, Aspire will print a local dashboard URL, usually:

```
https://localhost:18888
```

From the dashboard you can:
- View logs
- Inspect traces
- Check service health
- See environment variables and bindings

---

## API Access

After startup, the API will be available at a local address printed by Aspire, for example:

```
https://localhost:7xxx
```

Swagger (if enabled):

```
https://localhost:7xxx/swagger
```

The exact port may differ per machine — Aspire manages this automatically.

---

## Database

- Database: **PostgreSQL**
- Runs in a **Docker container** managed by Aspire
- Connection string is injected into the API at runtime

You do **not** need to:
- Install PostgreSQL locally
- Create the database manually
- Manually manage connection strings

You **do** need Docker running (see “Start Docker” above).

---

## Common Commands

### Stop everything

Press:

```
Ctrl + C
```

All Aspire-managed services will shut down cleanly.

---

### Clean & rebuild (if things get weird)

```bash
dotnet clean
dotnet build
dotnet run --project src/Mercator.AppHost
```

---

## Troubleshooting

### Aspire workload not found

```text
The workload 'aspire' was not found
```

Fix:

```bash
dotnet workload install aspire
```

---

### Port already in use

Aspire auto-selects ports, but if something conflicts:
- Stop other running services
- Restart the AppHost

---

### Database authentication errors

If PostgreSQL fails to start:

```bash
dotnet run --project src/Mercator.AppHost
```

Watch the dashboard logs — Aspire usually reports the root cause clearly.

---
