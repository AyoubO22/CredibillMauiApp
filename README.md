# CredibillMauiApp

## How to Run
- Install .NET 9 SDK and MAUI workload
- Restore NuGet packages
- Build and run on Android/iOS/MacCatalyst/Windows

## Local DB Structure
- SQLite tables: Customers, Invoices, Payments
- Data is stored locally for offline use
- Seeding logic adds demo data on first launch

## API Endpoints Used
- `/api/customers` (GET, POST, PUT, DELETE)
- `/api/invoices` (GET, POST, PUT, DELETE)
- `/api/payments` (GET, POST, PUT, DELETE)
- `/api/auth/register` (POST)
- `/api/auth/login` (POST)
- `/api/auth/refresh` (POST)

## Login/Identity
- Uses ASP.NET Identity endpoints
- JWT and refresh token stored securely with SecureStorage
- Automatic re-login if token expires

## GDPR/Privacy
- All data is stored locally and securely
- PrivacyPolicyPage allows users to delete all local data
- No data is shared with third parties
