# **FlexiPay**

## **Overview**

**FlexiPay** is a web application designed for managing user accounts, linking bank accounts, making payments, and viewing bank details. The platform allows users to link their FLOW accounts, manage their bank accounts, view recent transactions, and perform account-related actions like adding or managing bank accounts.

## **Features**

### 1. **Link Bank Accounts to FLOW Account**  
   Users can link their bank account to their FLOW account to make payments.

### 2. **View Bank Account Details**  
   Users can view their existing bank accounts, including account type, account number, and balance.

### 3. **Make Payments**  
   Users can make payments from their linked bank accounts to their FLOW accounts.

### 4. **Add Bank Accounts**  
   Users can add new bank accounts to their profile with account details and an initial balance.

### 5. **Manage Linked Accounts**  
   Users can manage their linked bank accounts, view recent transactions, and perform necessary actions on their accounts.

---

## **Technologies Used**

- **ASP.NET Web Forms (C#)**
- **SQL Server** (for database storage)
- **HTML/CSS** (for front-end styling)
- **JavaScript** (for client-side interactivity)
- **Bootstrap** (for responsive design)

---

## **Installation**

### **Prerequisites**

Before running **FlexiPay** locally, ensure you have the following installed:

- **Visual Studio** with **ASP.NET** and **C#** support
- **SQL Server** (localdb) for managing the database and storing user data

### **Steps to Run Locally**

#### 1. **Clone the Repository**

Clone the repository to your local machine


#### 2. **Open in Visual Studio**

- Open **Visual Studio**.
- Go to **File** > **Open** > **Project/Solution**.
- Navigate to the directory where you cloned the repository and open the **FlexiPay.sln** solution file.

**Note**: If you're prompted to restore NuGet packages, Visual Studio will do this automatically. However, if it doesn't, you can manually restore them by right-clicking on the solution in the **Solution Explorer** and selecting **Restore NuGet Packages**.

#### 3. **Run the Application**

- After ensuring that all project dependencies are restored, click **Start** (or press `F5`) to run the application in Debug mode.
- Visual Studio will build the solution, start the application, and open a browser window where you can interact with **FlexiPay** locally.

**Troubleshooting**: If the application doesn’t start as expected, check the **Output** and **Error List** windows in Visual Studio for any errors or missing configurations (e.g., connection strings or database setup).
