# The Platform Provide Shoe Cleaning Service (TP4SCS)

An easy-to-use platform that connects customers with shoe-cleaning service providers.  
Customers place orders through a simple UI; providers manage orders, subscriptions and promotions; admins and owners monitor and control the platform.

---

## Key Goals
- Provide a clean, discoverable marketplace for shoe cleaning businesses and customers.
- Let providers advertise services and sell packages/subscriptions.
- Support order management, notifications, chat and feedback.
- Give admins/owners dashboards for statistics and platform control.

---

## Highlight Features 
- **Authentication**: Register, Login, Reset password  
- **Orders**: Create, update status, manage pickup/delivery  
- **Packages & Subscriptions**: Promotions / featured packages for providers  
- **Payments**: Integrated with external payment gateways  
- **Communication**: Notifications & Chat between parties  
- **Support**: Feedback, reviews, and support tickets  
- **Dashboards**: Business & platform statistics  
- **External Integrations**: Shipping provider, Payment system, AI Censorship System

---

## Technologies
- **Backend**: ASP.NET Core 8 Web API + EF Core  
- **Database**: SQL Server
- **Integrations**: [Google Identity](https://developers.google.com/identity), [GiaoHangNhanh](https://ghn.vn/), [OpenAI](https://openai.com/api/), [VnPay](https://vnpay.vn/), [MoMo](https://www.momo.vn/), [Firebase](https://firebase.google.com/)

---

## Installation (For Local Development)

### 1. Setup Database
[**1.1 Install Sql Server**](https://learn.microsoft.com/en-us/sql/database-engine/install-windows/install-sql-server?view=sql-server-ver17)
**1.2 Create thhe database (run schema script)**
In the `TP4SCS.Database` folder run the `TP4SCS_DatabaseScript.sql` script to create the database schema used by the project.
**1.3 (Optional) Import sample data**
If you want sample/test data for API testing, run the `TP4SCS_SampleDataScript.sql` script.

### 2. Setup Project
[**2.1 Install Visual Studio 2022**](https://learn.microsoft.com/en-us/visualstudio/install/install-visual-studio?view=vs-2022)
[**2.2 Install Git**](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git)
**2.3 Clone the project to your computer**
**2.4 Open and run the porject solution file**
In the TP4SCS.BE, open the TP4SCS.Solution.sln file with Visual Studio, the project solution.
**2.5 Configure application settings**
Open appsettings.json and set the external integrations and environment-specific values.
***2.5.1 Configure the JWT setting***
Find the Jwt section and fill required elements bellow
***2.5.2 Configure the Database Connection String setting***
Find the DefaultConnection in the ConnectionStrings section and fill required elements bellow
***2.5.3 Configure the Email SMTP Server setting***
Find the EmailSettings section and fill required elements bellow
***2.5.4 Configure the Google Identity Authentication setting***
Find the GoogleAuthSettings section and fill required elements bellow
***2.5.5 Configure the GiaoHangNhanh API Key setting***
Find the GHN_API section and fill required elements bellow
***2.5.6 Configure the OpenAI API Key setting***
Find the OpenAIKey section and fill required elements bellow
***2.5.7 Configure the VnPay API Key setting***
Find the VnPAY section and fill required elements bellow
***2.5.8 Configure the MoMo API Key setting***
Find the Momo section and fill required elements bellow
***2.5.9 Configure the Firebase setting***
Find the FilePath in the Firebase section and fill required elements bellow
**2.6 Build and run the project**
Build the solution (Build → Build Solution).
Run (F5) or launch without debugging (Ctrl+F5).