# 🧩 Clean Architectured Blogging Platform — Full System Documentation

This is a full-stack Blogging Platform built with:

- **Backend:** .NET 8 Web API + EF Core + Identity + Clean Architecture  
- **Frontend:** Next.js 14 + React 19 + TailwindCSS  
- **Deployments:**
  - Option 1: GitHub → IIS
  - Option 2: Azure DevOps → Azure App Service  
- **Multi-Repo Support:** GitHub + Azure DevOps  
- **CI/CD Pipelines:** Automated build + deploy for backend & frontend  

---

# 🌍 Deployed Applications

### 🔵 Backend API  
https://bloggingplatformappservice-gre7fyhtancwfge6.canadacentral-01.azurewebsites.net/swagger/index.html  

### 🟣 Frontend Website  
https://bloggingplatformfrontendwebapp-byauatf8bqf4h6ga.canadacentral-01.azurewebsites.net/  

---

# 📐 Architecture Overview

This solution follows **Clean Architecture**, separating the system into independent layers with clear responsibilities.  
The structure ensures scalability, maintainability, and testability.

---

### 🟦 Domain (Core)
Contains the **business model**:
- Entities, value objects, enums  
- Domain logic & rules  
- Shared domain primitives  
This layer is pure and has **no external dependencies**.

### 🟩 Application
Implements the system's **use cases** using **CQRS**:
- Commands, Queries, and Handlers (via MediatR)  
- Validation (FluentValidation)  
- Application models & helpers  
- Repository and service interfaces  
This layer coordinates domain operations without knowing infrastructure details.

### 🟧 Infrastructure
Provides **actual implementations** of Application interfaces:
- EF Core DbContext, migrations, repository implementations  
- External service integrations  
- Infrastructure-level services  
This layer depends on Application, never the reverse.

### 🟥 Presentation (API Layer)
Exposes the application to the outside world:
- Controllers  
- DTOs & Validators  
- AutoMapper profiles  
- Middleware, JWT, CORS, Swagger  
This layer handles HTTP requests and delegates all logic to the Application layer.

---

### ⭐ Key Highlights
- **CQRS** cleanly separates read and write operations  
- **MediatR** decouples controllers from business logic  
- **Repositories & Services** follow interface-driven design  
- **Infrastructure is replaceable** without touching business logic  
- **High testability**, **clean separation**, and **scalable structure**
  
---

# 🧰 Design Patterns & Techniques Used

### ⚡ Clean Architecture
A layered architecture ensuring:
- Inner layers do **not** depend on outer layers  
- Business rules remain isolated from infrastructure  
- High maintainability and testability over time  

---

### ⚡ CQRS (Command–Query Responsibility Segregation)
Separates operations into:
- **Commands** → state-changing actions  
- **Queries** → read-only operations  

This increases scalability, performance, and clarity of intent.

---

### ⚡ Mediator Pattern (via MediatR)
Implements loose coupling between controllers and business logic:
- Controllers send commands/queries to **Mediator**  
- Handlers execute the use case logic  
- Eliminates direct dependencies on application logic  

---

### ⚡ Validation Pipeline (FluentValidation)
Centralized validation using:
- FluentValidation rules for each command/query  
- MediatR pipeline behavior that validates before execution  

Ensures consistent input validation across the entire system.

---

### ⚡ Dependency Injection
All services, repositories, handlers, validators, and mappers are registered using the built-in .NET DI container, improving flexibility and testability.

---

### ⚡ Repository / Data Access Abstraction
- Repository interfaces exist in the **Application layer**  
- EF Core and data access implementations live in the **Persistence layer**  

This keeps infrastructure code separate from business logic and allows easy replacement or unit testing.

---

# 🏗️ Technologies & Frameworks

| Technology | Usage |
|-----------|--------|
| **.NET 8** | Backend |
| **ASP.NET Web API** | HTTP Endpoints |
| **EF Core** | ORM |
| **SQL Server** | Database |
| **Next.js 14 / React 19** | Frontend |
| **TailwindCSS** | Styling |
| **GitHub Actions** | CI/CD for IIS |
| **Azure DevOps Pipelines** | CI/CD for Azure |
| **Azure App Service** | Cloud Hosting |
| **IIS** | On-prem Deployment |

---

# 🔀 Multi-Repository Workflow (GitHub + Azure DevOps)

Each project supports **Two Remotes**:

### GitHub  
https://github.com/ErinyAdel/CleanArchitectured_BloggingPlatform/

### Azure DevOps  
https://dev.azure.com/renyadel7/CleanArchitectured_BloggingPlatform/_git/CleanArchitecturedBloggingPlatform
https://dev.azure.com/renyadel7/CleanArchitectured_BloggingPlatform/_git/BloggingPlatformFrontend

### Push Examples

**GitHub only**
~~~
git push github main
~~~

**Azure only**
~~~
git push azure main
~~~

**Both**
~~~
git push github main
git push azure main
~~~

---

# ⚙️ Backend Deployment Overview

Backend is deployed using:

✔ **GitHub Actions → Self-Hosted Runner → IIS**  
✔ **Azure DevOps → Azure App Service (Windwos)**  

---

# ⚙️ Frontend Deployment Overview

Frontend is deployed using:

✔ **Azure DevOps → Azure App Service (Linux)**  

---

# 🖥️ Backend Deployment — GitHub → IIS

## 🟦 Step 1 — Configure Self-Hosted Runner

~~~
mkdir "D:\GitHubRunner"
cd "D:\GitHubRunner"
~~~

Configure:

~~~
.\config.cmd --url https://github.com/... --token <TOKEN>
~~~

Start:

~~~
.\run.cmd
~~~

---

## 🟩 Step 2 — IIS Setup

- Install **.NET Hosting Bundle**  
- Create Website directory  
- AppPool: **No Managed Code**  
- Assign file permissions  

---

## 🟨 Step 3 — GitHub Actions Pipeline (deploy.yml)

~~~yaml
name: Build and Deploy .NET App

on:
  push:
    branches:
      - main

jobs:
  deploy-to-eriny:
    runs-on: [self-hosted, ErinyDevice]
    
    steps:
      - uses: actions/checkout@v4

      - name: Restore dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build --configuration Release --no-restore

      - name: Publish
        run: dotnet publish -c Release -o "./published"

      - name: Deploy to IIS (ERINY)
        shell: powershell
        run: |
          $source = "$(pwd)\published"
          $destination = "C:\Work\SoftwareDevelopment\Deployment\PublishedCleanArchitectured_BloggingPlatform"
          Copy-Item "$source\*" $destination -Recurse -Force
          Write-Host "Deployment done for ERINY runner"
          
  build-and-deploy:
    runs-on: self-hosted

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Check .NET version
        run: dotnet --version
        
      - name: Restore dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build --configuration Release --no-restore

      - name: Publish
        run: dotnet publish -c Release -o "./published"

      - name: Deploy to IIS
        shell: powershell
        run: |
          $source = "$(pwd)\published"
          $destination = "D:\Company\Software Development\PublishedProject\PublishedBloggingPlatform"
          Copy-Item -Path "$source\*" -Destination $destination -Recurse -Force
          Write-Host "Deployment is Completed Without IIS Reset"
~~~

### 🔹 GitHub Actions Multi-Machine Deployment

The `deploy.yml` workflow uses **two jobs**, each tagged with its own runner labels:

- `deploy-to-eriny` → runs on `[self-hosted, ErinyDevice]`  
- `build-and-deploy` → runs on the default `[self-hosted]` runner (company machine)

Each job publishes the API and copies it to a **different IIS folder**, enabling automated deployment to **two separate IIS servers** from the same pipeline.

---

# ☁️ Backend Deployment — Azure DevOps → Azure App Service (azure-pipelines.yml)

~~~yaml
trigger:
- main

pool:
  vmImage: 'windows-latest'

steps:

# Install specific .NET version
- task: UseDotNet@2
  displayName: "Install .NET"
  inputs:
    version: '8.x'
    packageType: 'sdk'

# Restore packages
- script: dotnet restore
  displayName: "Restore"

# Build solution
- script: dotnet build --configuration Release --no-restore
  displayName: "Build"

# Publish the API
- script: dotnet publish -c Release -o $(Build.ArtifactStagingDirectory)/publish
  displayName: "Publish"

# Deploy to Azure Web App
- task: AzureWebApp@1
  displayName: "Deploy to Azure Web App"
  inputs:
    azureSubscription: 'BloggingPlatformServiceConnection'   # Service Connection Name
    appName: 'BloggingPlatformAppService'   # Azure Web App Name
    package: '$(Build.ArtifactStagingDirectory)/publish'
~~~

---

# 🌐 Frontend — Next.js SSR Deployment to Azure App Service

## Azure App Service Configuration

| Setting | Value |
|--------|--------|
| Runtime | Node 22 LTS |
| Platform | Linux |
| Startup Command | npm start |
| Build During Deploy | true |

## package.json

~~~json
"scripts": {
  "dev": "next dev",
  "build": "next build",
  "start": "next start -p $PORT",
  "start:win": "next start -p 3000"
}
~~~

---

# 🚀 Frontend CI/CD — Azure DevOps (azure-pipeline.yml)

~~~yaml
trigger:
  - main

pool:
  vmImage: 'ubuntu-latest'

variables:
  NODE_VERSION: '22.x'

steps:
  - checkout: self
    persistCredentials: true

  - task: NodeTool@0
    inputs:
      versionSpec: '$(NODE_VERSION)'
    displayName: "Use Node.js"

  - script: npm ci
    displayName: "Install dependencies"

  - script: npm run build
    displayName: "Build Next.js app"

  - task: ArchiveFiles@2
    displayName: "Archive site"
    inputs:
      rootFolderOrFile: '$(Build.SourcesDirectory)/'
      archiveType: 'zip'
      archiveFile: '$(Build.ArtifactStagingDirectory)/site.zip'
      includeRootFolder: false
      replaceExistingArchive: true

  - task: PublishBuildArtifacts@1
    inputs:
      pathToPublish: '$(Build.ArtifactStagingDirectory)/site.zip'
    displayName: "Publish Artifact"

  - task: AzureWebApp@1
    displayName: "Deploy to Azure App Service"
    inputs:
      azureSubscription: 'BloggingPlatformServiceConnection'
      appName: 'BloggingPlatformFrontendWebApp'
      package: '$(Build.ArtifactStagingDirectory)/site.zip'
~~~

---

# 🔐 App Settings Configuration on Azure App Service (Tokens, Keys & Secrets)

Both the **Backend (.NET API)** and **Frontend (Next.js)** require secure configuration values such as:
- JWT Signing Keys  
- Issuer & Audience  
- Connection Strings  
- Third-party service keys  
- API URLs  

Azure App Service provides a secure way to store these using **Application Settings**, which overrides local configuration at runtime.

---

# 🟦 1. Backend — App Settings on Azure (Connection Strings + Secrets)

Open:

**Azure Portal → App Service → Configuration → Application Settings**

Add the following keys under **Application Settings**:

### 🔑 Required Backend Keys

| Name | Example Value |
|------|----------------|
| `JWT_Param:Key` | c2VjdXKlS2V5Tq9ySldURm9yQ29ycmVjdEF9dGhLYXk= |
| `JWT_Param:Audience` | SecureApiUser |
| `JWT_Param:Issuer` | SecureApi |
| `WEBSITE_ENABLE_SYNC_UPDATE_SITE` | true |
| `WEBSITE_RUN_FROM_PACKAGE` | 1 |

---

## 🗄️ Connection String (Backend)

Azure requires placing EF Core connection strings under the **Connection Strings** tab.

Go to:

**Azure App Service → Environment Variables → Connection Strings**

Add:

| Name | Type | Example |
|------|------|---------|
| `DefaultConnection` | SQLAzure | `Server=eriny-azure-aql-server.database.windows.net,1433;Initial Catalog=BloggingPlatformDB;User=....;Password=....;Encrypt=True;TrustServerCertificate=True;` |

.NET reads it automatically using:

```csharp
builder.Configuration.GetConnectionString("DefaultConnection");
```

# 🟦 2. Frontend — App Settings & Stack Settings on Azure

Open:

**Azure Portal → App Service → Environment Variables → Application Settings**

### 🔑 Required Backend Keys

| Name | Example Value |
|------|----------------|
| `NEXT_PUBLIC_API_BASE_URL` | https://bloggingplatformappservice-gre7fyhtancwfge6.canadacentral-01.azurewebsites.net/api |
| `PORT` | 3000 |
| `SCM_BASIC_AUTHORIZATION_ENABLED` | true |
| `SCM_DO_BUILD_DURING_DEPLOYMENT` | true |
| `WEBSITE_NODE_DEFAULT_VERSION` | 22 |

**Azure Portal → App Service → Configuration → Stack Settings**

### 🔑 Startup Command: 
~~~
npm run start
~~~

---

# 🧪 Local Development

### Backend

~~~
dotnet build
dotnet run
~~~

### Frontend

~~~
npm install
npm run dev
~~~

---

# 🏁 Summary

This system includes:

- Clean Architecture backend  
- Identity + JWT authentication  
- Next.js SSR frontend  
- Two CI/CD pipelines  
- Azure hosting (backend + frontend)  
- IIS on-prem deployment  
- Multi-repo support  
- Cookie-based authentication  
- Fully automated deployments  
