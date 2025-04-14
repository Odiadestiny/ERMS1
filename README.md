# Enterprise Resource Management System (ERMS)

**Author:** Destiny Odia (Odiadestiny)  
**Student ID:** 991715771  
**Institution:** Sheridan College  
**Course:** PROG36944 – Advanced .Net Server-side Technologies  
**Date:** April 2025

## Project Overview

The Enterprise Resource Management System (ERMS) is a comprehensive full‑stack web application designed to manage employees, projects, and tasks within an organization. Built using ASP.NET Core MVC, Web API, Entity Framework Core, and SQL Server, the application implements secure user authentication, role‑based access control, RESTful API integration, and robust data security features such as prevention of SQL Injection, CSRF, and XSS.

## Features

- **User Authentication & RBAC:**  
  Implements ASP.NET Core Identity with roles (Admin, Manager, Employee).  
  - Only Admin users can access certain functionalities, such as creating new employees, projects, and tasks.

- **CRUD Operations:**  
  - **Employee Management:** Manage employees with full Create, Read, Update, and Delete operations.  
  - **Project Management:** Create and monitor projects, including employee assignments.  
  - **Task Management:** Manage tasks with configurable priority and status levels.

- **RESTful API Integration:**  
  Exposes API endpoints to mirror MVC functionality and uses HttpClient for API consumption on the frontend.

- **Database Design & Reporting:**  
  - A normalized SQL Server database is implemented using Entity Framework Core.  
  - Stored procedures support data manipulation and custom reporting can be generated via SQL Server Reporting Services (SSRS) or Power BI Report Builder.

- **Data Security:**  
  - **SQL Injection Prevention:** Uses EF Core’s parameterized queries.  
  - **CSRF Protection:** Utilizes anti-forgery tokens in forms and `[ValidateAntiForgeryToken]` on POST actions.  
  - **XSS Protection:** Razor encoding is used by default, with an optional Content Security Policy (CSP) header.

- **Deployment & Monitoring:**  
  - **Deployment:** Published to Azure App Service (with alternative options for AWS or IIS available).  
  - **Logging & Monitoring:** Integrated with Application Insights and optionally NLog for runtime diagnostics.

- **Testing:**  
  Unit tests and integration tests are written using xUnit (or NUnit) with mock repositories to ensure robust code coverage (minimum 5%).

## Technology Stack

- **Backend:** ASP.NET Core MVC, ASP.NET Core Web API, Entity Framework Core  
- **Frontend:** Razor Views, HTML, CSS, JavaScript  
- **Database:** SQL Server  
- **Authentication/Authorization:** ASP.NET Core Identity  
- **Logging/Monitoring:** Application Insights (or NLog as an alternative)  
- **Testing:** xUnit / NUnit  
- **Deployment:** Azure App Service (or AWS/IIS)  
- **Version Control:** Git (hosted on GitHub)

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- SQL Server (or Docker container with SQL Server)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) for deploying to Azure
- [Visual Studio Code](https://code.visualstudio.com/) (or your preferred IDE)

### Setup

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/Odiadestiny/ERMS.git
   cd ERMS
Restore NuGet Packages:


dotnet restore
Apply Database Migrations:


dotnet ef database update
Run the Application Locally:


dotnet run
The application will be available at:
https://localhost:5242/ (or as specified in your launchSettings.json)

Testing
Run the tests with code coverage collection:


dotnet test --collect:"XPlat Code Coverage"
After running tests, you can generate an HTML report using ReportGenerator (ensure it’s installed and added to your PATH):


reportgenerator "-reports:TestResults/**/*.xml" "-targetdir:coveragereport" -reporttypes:Html
Deployment
Deploying to Azure App Service
Publish the Application:


dotnet publish -c Release -o ./publish
Zip the Publish Folder:

cd publish
zip -r ../publish.zip .
cd ..
Log in to Azure:


az login
Create a Resource Group:


az group create --name ERMSResourceGroup --location eastus
Create an App Service Plan (Windows):


az appservice plan create --name ERMSAppServicePlan --resource-group ERMSResourceGroup --sku B1
Create a Web App:


az webapp create --name destinemsapp --resource-group ERMSResourceGroup --plan ERMSAppServicePlan
Deploy the Application:


az webapp deploy --resource-group ERMSResourceGroup --name youruniqueappname --src-path publish.zip
Configure Production Settings:

In the Azure Portal, configure application settings (e.g., set ASPNETCORE_ENVIRONMENT to Production and update connection strings).

Access Your Application:

Open your browser and navigate to:
https://destinemsapp.azurewebsites.net/
