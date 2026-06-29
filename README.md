# ASP.NET Security - Under The Hood

A comprehensive educational project demonstrating ASP.NET Core authentication and authorization patterns using Razor Pages, cookie-based authentication, and role-based access control.

## 📋 Project Overview

This project showcases practical implementations of ASP.NET Core security features including:
- **Cookie-based Authentication** - Secure user session management
- **Claims-based Authorization** - Fine-grained access control using custom claims
- **Policy-based Authorization** - Declarative authorization policies
- **Custom Authorization Requirements** - Complex authorization logic with custom requirement handlers
- **Access Control & Access Denied handling** - Proper authorization flow management

## 🏗️ Architecture

### Technology Stack
- **.NET 10** - Latest .NET runtime
- **ASP.NET Core Razor Pages** - Server-side web framework
- **Cookie Authentication** - Session-based user authentication
- **Bootstrap 5** - Frontend UI framework
- **jQuery** - Client-side scripting

### Project Structure

```
Security_UnderTheHood/
├── Pages/                          # Razor Pages
│   ├── Account/
│   │   ├── Login.cshtml(.cs)       # User login page with credential handling
│   │   ├── Logout.cshtml(.cs)      # User logout with session cleanup
│   │   └── AccessDenied.cshtml(.cs) # Access denied error page
│   ├── HumanResource.cshtml(.cs)   # HR Department page (requires HR department claim)
│   ├── HRManager.cshtml(.cs)       # HR Manager page (requires Manager claim + probation requirement)
│   ├── Index.cshtml(.cs)           # Home/landing page
│   ├── Privacy.cshtml(.cs)         # Privacy policy page
│   ├── Settings.cshtml(.cs)        # User settings page
│   ├── Error.cshtml(.cs)           # Global error page
│   └── Shared/
│       ├── _Layout.cshtml          # Master layout template
│       └── _LoginStatusPartial.cshtml # Login status display component
├── Authorization/
│   └── HRManagerProbationRequirement.cs # Custom authorization requirement & handler
├── wwwroot/                        # Static assets (CSS, JS, images)
├── Program.cs                      # Application configuration & startup
└── appsettings.json               # Configuration settings
```

## 🔐 Authentication & Authorization

### Authentication Scheme
- **Type**: Cookie-based authentication ("MyCookieAuth")
- **Session Duration**: 333 seconds (5.5 minutes)
- **Security**: HttpOnly and Secure flags enabled by default

### Authorization Policies

The application implements three distinct authorization policies:

#### 1. **AdminOnly Policy**
- **Requirement**: Must have the "Admin" claim
- **Usage**: Restricted admin-only operations
- **Example**: `[Authorize(Policy = "AdminOnly")]`

#### 2. **MustBelongToHRDepartment Policy**
- **Requirements**: Must have "Department" claim with value "HR"
- **Usage**: Access to HumanResource page
- **Example Page**: `HumanResource.cshtml`
- **Applied to**: `HumanResourceModel`

#### 3. **HRManagerOnly Policy** (Complex)
- **Requirements**:
  1. Must have "Department" claim with value "HR"
  2. Must have "Manager" claim
  3. Must pass custom `HRManagerProbationRequirement` (3-month employment minimum)
- **Usage**: HR Manager-specific operations
- **Example Page**: `HRManager.cshtml`
- **Applied to**: `HRManagerModel`

### Claims-Based Identity

Users are authenticated using custom claims that define their permissions:

```csharp
// Example claims structure
var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, userId),
    new Claim(ClaimTypes.Name, userName),
    new Claim("Department", "HR"),        // Department claim
    new Claim("Manager", "true"),         // Manager flag
    new Claim("EmploymentDate", "2022-01-15"), // For probation calculation
    new Claim("Admin", "true")            // Admin privilege
};
```

## 📄 Page Descriptions

### Public Pages (No Authorization Required)
- **Index** - Home page/landing page
- **Privacy** - Privacy policy information

### Account Pages
- **Login** - User authentication entry point
  - Accepts username/password credentials
  - Creates authenticated cookie session
  - Stores custom claims for authorization
- **Logout** - Session termination
  - Clears authentication cookie
  - Redirects to home page
- **AccessDenied** - Displayed when user lacks required permissions

### Protected Pages
- **Settings** - User preferences and account settings
- **HumanResource** - HR Department portal
  - Requires: `[Authorize(Policy = "MustBelongToHRDepartment")]`
  - Accessible by: HR department employees
- **HRManager** - HR Manager dashboard
  - Requires: `[Authorize(Policy = "HRManagerOnly")]`
  - Accessible by: HR managers with minimum 3-month employment

## 🚀 Getting Started

### Prerequisites
- .NET 10 SDK or later
- Visual Studio 2026 Community (or higher)
- PowerShell or Command Prompt

### Running the Application

1. **Clone the repository**
   ```bash
   git clone https://github.com/chnpgn/aspnet-cookie-auth.git
   cd "ASP.NET Security"
   ```

2. **Open the solution**
   ```bash
   dotnet sln open "ASP.NET Security.slnx"
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run --project Security_UnderTheHood/Security_UnderTheHood.csproj
   ```

5. **Access the application**
   - Navigate to `https://localhost:5001` (or the configured port)

### Default Test Credentials

The application includes test login functionality. Refer to the `Login.cshtml.cs` for available test accounts and their associated claims.

## 🔑 Key Features

### 1. Cookie-Based Session Management
- Secure cookie authentication with HttpOnly flag
- Automatic session expiration
- Custom cookie name and security settings

### 2. Claims-Based Authorization
- Fine-grained control over user permissions
- Multiple claims per user
- Policy-based authorization combining multiple claims

### 3. Custom Authorization Requirements
- `HRManagerProbationRequirement` enforces employment duration
- Custom handler validates employment date claims
- Extensible pattern for complex authorization logic

### 4. Access Control Flow
- Automatic redirection to login for unauthenticated requests
- Redirect to AccessDenied page for unauthorized requests
- Proper HTTP status codes (401 Unauthorized, 403 Forbidden)

### 5. Partial Views & Components
- `_LoginStatusPartial.cshtml` - Displays current user info and logout option
- Dynamic UI based on user's authenticated state

## 🧪 Testing the Authorization

1. **Login as HR Employee**
   - Access HumanResource page (should succeed)
   - Access HRManager page (should be denied)

2. **Login as HR Manager**
   - Access HumanResource page (should succeed)
   - Access HRManager page (should succeed if probation requirement met)

3. **Attempt Anonymous Access**
   - Try accessing protected pages without logging in
   - Should redirect to login page

## 📚 Security Concepts Demonstrated

### Authentication
- User identity verification
- Cookie-based session tokens
- Secure cookie attributes (HttpOnly, Secure, SameSite)

### Authorization
- Role-based access control (implicit via claims)
- Claims-based authorization
- Policy-based authorization patterns
- Custom authorization requirement handlers

### Best Practices
- Separation of concerns (Authentication vs Authorization)
- Declarative authorization using attributes
- Custom claim types for application-specific logic
- Proper error handling and user feedback

## 🛡️ Configuration

### appsettings.json
Contains application configuration including:
- Logging levels
- Connection strings (if applicable)
- Custom application settings

### Program.cs Configuration
Key configuration points:
```csharp
// 1. Authentication setup
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options => { ... });

// 2. Authorization policies
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", ...)
    .AddPolicy("MustBelongToHRDepartment", ...)
    .AddPolicy("HRManagerOnly", ...);

// 3. Custom authorization handler
builder.Services.AddSingleton<IAuthorizationHandler, 
    HRManagerProbationRequirementHandler>();

// 4. Middleware order (CRITICAL)
app.UseAuthentication();  // Must be before UseAuthorization
app.UseAuthorization();
```

## 🎓 Learning Outcomes

This project teaches:
- How ASP.NET Core authentication middleware works
- Building claims-based identities
- Implementing authorization policies
- Creating custom authorization requirement handlers
- Proper middleware ordering and configuration
- Securing Razor Pages with authorization attributes
- Handling access denied scenarios

## 📝 License

This project is educational and provided as-is.

## 🔗 Repository

- **GitHub**: https://github.com/chnpgn/aspnet-cookie-auth
- **Original Clone Location**: `C:\Users\MMXX\source\repos\ASP.NET Security`

## 📧 About

A hands-on learning resource for understanding ASP.NET Core security patterns and best practices.

---

**Last Updated**: 2025  
**Target Framework**: .NET 10  
**IDE**: Visual Studio 2026 Community Edition