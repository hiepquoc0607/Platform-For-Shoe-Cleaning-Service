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

**1.1 Install Sql Server**<br>
[Instructor](https://learn.microsoft.com/en-us/sql/database-engine/install-windows/install-sql-server?view=sql-server-ver17)<br>

**1.2 Create thhe database (run schema script)**<br>

In the `TP4SCS.Database` folder run the `TP4SCS_DatabaseScript.sql` script to create the database schema used by the project.<br>

**1.3 (Optional) Import sample data**<br>

If you want sample/test data for API testing, run the `TP4SCS_SampleDataScript.sql` script.<br>

### 2. Setup Project<br>

**2.1 Install Visual Studio 2022**<br>
[Instructor](https://learn.microsoft.com/en-us/visualstudio/install/install-visual-studio?view=vs-2022)<br>

**2.2 Install Git**<br>
[Instructor](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git)<br>

**2.3 Clone the project to your computer**<br>

**2.4 Open and run the porject solution file**<br>

In the `TP4SCS.BE`, open the `TP4SCS.Solution.sln` file with Visual Studio, the project solution.<br>

**2.5 Configure application settings**<br>

Open `appsettings.json` and set the external integrations and environment-specific values.<br>

***2.5.1 Configure the JWT setting***<br>

Find the Jwt section and fill required elements bellow<br>

![Jwt Section](https://github.com/user-attachments/assets/202b6b27-c6f5-4f4c-8c73-916c541df4ba)

***2.5.2 Configure the Database Connection String setting***<br>

Find the DefaultConnection in the Database ConnectionStrings section and fill required elements bellow<br>

![Database Section](https://github.com/user-attachments/assets/00845524-0df5-4016-9463-973ef5212ff4)

***2.5.3 Configure the Email SMTP Server setting***<br>

Find the EmailSettings section and fill required elements bellow<br>

![Email Section](https://github.com/user-attachments/assets/b13befaf-6095-4a70-b53c-171f2538ebab)

***2.5.4 Configure the Google Identity Authentication setting***<br>

Find the GoogleAuthSettings section and fill required elements bellow<br>

![Google Auth Section](https://github.com/user-attachments/assets/f3060f28-3220-4efc-82c6-7febe186f2ec)

***2.5.5 Configure the GiaoHangNhanh API Key setting***<br>

Find the GHN_API section and fill required elements bellow<br>

![GNH Section](https://github.com/user-attachments/assets/45bcae0a-61b8-4598-872f-b845e447cb0a)

***2.5.6 Configure the OpenAI API Key setting***<br>

Find the OpenAIKey section and fill required elements bellow<br>

![OpenAI Section](https://github.com/user-attachments/assets/e7df77ea-5d81-49a9-b7aa-5911dcb8cb3e)

***2.5.7 Configure the VnPay API Key setting***<br>

Find the VnPAY section and fill required elements bellow<br>

![VnPay Section](https://github.com/user-attachments/assets/0f0c8dd8-13cc-4837-88e2-9d3af4d67833)

***2.5.8 Configure the MoMo API Key setting***<br>

Find the Momo section and fill required elements bellow<br>

![MoMo Section](https://github.com/user-attachments/assets/9fb35a0a-6616-4102-9b45-9352ffdb55dc)

***2.5.9 Configure the Firebase setting***<br>

Find the FilePath in the Firebase section and fill required elements bellow<br>

![Firebase Section](https://github.com/user-attachments/assets/f151778c-dbe9-4f39-9ccf-bb07941d8c67)

**2.6 Build and run the project**<br>

Build the solution (Build → Build Solution).<br>

Run (F5) or launch without debugging (Ctrl+F5).<br>