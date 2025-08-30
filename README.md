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
## Architecture Overview

The app is a .NET MAUI (net9.0) Shell application with XAML front-end, MVVM via CommunityToolkit, and an offline-first data model backed by SQLite. When the API is available, services use HttpClient to perform CRUD; when offline, data is read/written locally and synced later.

- UI: XAML pages and Shell routes under `Views/*`
- ViewModels: CommunityToolkit MVVM in `ViewModels/*`
- Services: `ApiClient` (REST), `CustomerService`, `InvoiceService`, `PaymentService`, `DatabaseService`
- Storage: SQLite via `sqlite-net-pcl`; tokens via `TokenStorage` (Preferences on Mac Catalyst)
- Auth: Local user table with PBKDF2 hashing (offline); wire to Identity API when available

## API Map (planned/consumed)

Base URL: `Constants.BaseApiUrl` (configure per environment).

- Auth
  - POST `auth/register` → create account (Identity)
  - POST `auth/login` → JWT + refresh
  - POST `auth/refresh` → refresh JWT
- Customers
  - GET `Customers` / `Customers/{id}`
  - POST `Customers` / PUT `Customers/{id}` / DELETE `Customers/{id}`
- Invoices
  - GET `Invoices` / `Invoices/{id}`
  - POST `Invoices` / PUT `Invoices/{id}` / DELETE `Invoices/{id}`
- Payments
  - GET `Payments` / `Payments/{id}`
  - POST `Payments` / PUT `Payments/{id}` / DELETE `Payments/{id}`

## SQLite Schema

Tables in `credibill.db3` (see `Services/DatabaseService`):

- `Customer` (Id PK AUTOINC, Name, Email, Phone)
- `Invoice` (Id PK AUTOINC, CustomerId NOT NULL Indexed, Reference, Amount, DateIssued)
- `Payment` (Id PK AUTOINC, InvoiceId NOT NULL Indexed, Reference, Amount, DatePaid)
- `User` (Id PK AUTOINC, Name UNIQUE, Email UNIQUE, PasswordSalt, PasswordHash)

Relationships: One `Customer` → many `Invoice`; One `Invoice` → many `Payment`. Foreign keys enforced (`PRAGMA foreign_keys = ON`).

## Offline/Online Flow

- Read: Services try API first; on failure, fall back to SQLite.
- Write: Create/Update/Delete applied locally; API attempt is made when online (best-effort).
- Auth: Local (offline) PBKDF2 hashing; planned to use Identity tokens from API with refresh-on-401.

## Demo Steps

1) Launch app → Welcome screen. Sign up or Login.
2) After login → hamburger shows Privacy and Sign out.
3) Customers tab → list, search, open details, add/edit customers.
4) Invoices tab → list with search + Customer filter (Picker at top). Add invoice → pick a customer, set amount, save.
5) Payments tab → add payment → pick an invoice, set reference/amount, save.
6) Privacy Policy → Delete my data (clears DB and tokens). Relaunch to reseed.
7) Optional: Switch to dark mode to verify contrast.

## Maintenance Notes

- Configure API: set `Constants.BaseApiUrl`; toggle `Constants.UseApiAuth` when the Identity API is ready.
- DB location (Mac Catalyst): `~/Library/Containers/<bundle id>/Data/Library/Application Support/credibill.db3`
- To reset DB manually, delete the file or use the Privacy page.
