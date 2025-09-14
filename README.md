# 📰 News Website

A full-stack **ASP.NET MVC** news portal with a responsive frontend, clean architecture, and role-based admin features.  
The project demonstrates practical application of **SOLID principles**, **Repository Pattern**, and modern ASP.NET Core practices.

---

## 🚀 Features
- Display latest news articles with categories and tags  
- Responsive UI built with **Bootstrap 4** and **jQuery**  
- **Admin panel** for managing news and site settings  
- Authentication & authorization using **ASP.NET Core Identity**  
- Separation of concerns using the **Repository Pattern** (generic + extensions)  
- Dependency Injection for clean and testable design  
- Middleware & Filters applied at different levels of the request pipeline  

---

## 🛠 Tech Stack
- **Backend:** ASP.NET Core MVC, C#  
- **Frontend:** jQuery, Bootstrap 4, HTML5, CSS3  
- **Architecture & Patterns:** Repository Pattern, Dependency Injection, SOLID Principles  
- **Authentication:** ASP.NET Core Identity  
- **Database:** Entity Framework Core with Migrations (SQL Server by default, configurable)  

---

## 📐 Architecture Highlights
- **Repository Pattern**  
  Encapsulates data access logic with a **generic repository**, extended only when needed.  
- **Dependency Injection**  
  Applied **constructor injection** to avoid manual instantiation and follow **SOLID principles**.  
- **Service Lifetimes**  
  Practical use of ASP.NET Core lifetimes:  
  - **Singleton** – one instance for the whole app  
  - **Scoped** – one instance per HTTP request  
  - **Transient** – multiple instances per request  
- **Middleware vs Filters**  
  Learned their distinct roles:  
  - Middleware → global pipeline logic  
  - Filters → controller/action-specific concerns  
