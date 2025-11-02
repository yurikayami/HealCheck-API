# HealCheck API - Setup and Run Guide

## Project Overview
This .NET Core Web API analyzes nutritional components from food images using the Gemini API and stores results in SQL Server.

## Prerequisites
- .NET SDK 9.0 or later
- SQL Server (Express or higher)
- Gemini API Key (Get from: https://makersuite.google.com/app/apikey)
- Visual Studio Code or Visual Studio

## Project Structure
```
HealCheckAPI/
├── Controllers/          # API Controllers
│   ├── UsersController.cs
│   └── ImagesController.cs
├── Data/                # Database Context
│   └── ApplicationDbContext.cs
├── DTOs/                # Data Transfer Objects
│   ├── UserDtos.cs
│   ├── ImageDtos.cs
│   └── GeminiDtos.cs
├── Models/              # Database Models
│   ├── User.cs
│   ├── Image.cs
│   ├── Nutrient.cs
│   └── Analysis.cs
├── Services/            # Business Logic
│   ├── IUserService.cs
│   ├── UserService.cs
│   ├── IImageService.cs
│   ├── ImageService.cs
│   ├── IGeminiService.cs
│   └── GeminiService.cs
├── Migrations/          # EF Core Migrations
├── Program.cs           # Application Entry Point
└── appsettings.json     # Configuration
```

## Setup Instructions

### 1. Configure Database Connection
Open `appsettings.json` and update the connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=nutrition_app_db;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 2. Configure Gemini API Key
In `appsettings.json`, add your Gemini API key:
```json
"GeminiAPI": {
  "ApiKey": "YOUR_GEMINI_API_KEY_HERE"
}
```

### 3. Create Database
Run the following command to create the database:
```bash
dotnet ef database update
```

This will:
- Create the `nutrition_app_db` database
- Create all tables (Users, Images, Nutrients, Analysis)
- Seed initial nutrient data

### 4. Run the Application
```bash
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`
- Swagger UI: `https://localhost:5001/swagger`

## API Endpoints

### User Management

#### Register User
```http
POST /api/users/register
Content-Type: application/json

{
  "username": "testuser",
  "password": "password123",
  "email": "test@example.com"
}
```

#### Login User
```http
POST /api/users/login
Content-Type: application/json

{
  "username": "testuser",
  "password": "password123"
}
```

### Image Analysis

#### Upload Image for Analysis
```http
POST /api/images/upload
Content-Type: multipart/form-data

userId: 1
image: [file]
```

#### Get Image Analysis Result
```http
GET /api/images/{id}
```

## Testing with Swagger

1. Start the application
2. Navigate to `https://localhost:5001/swagger`
3. Test endpoints directly from the Swagger UI

## Testing with cURL

### Register a User
```bash
curl -X POST "https://localhost:5001/api/users/register" ^
  -H "Content-Type: application/json" ^
  -d "{\"username\":\"testuser\",\"password\":\"password123\",\"email\":\"test@example.com\"}"
```

### Login
```bash
curl -X POST "https://localhost:5001/api/users/login" ^
  -H "Content-Type: application/json" ^
  -d "{\"username\":\"testuser\",\"password\":\"password123\"}"
```

### Upload Image
```bash
curl -X POST "https://localhost:5001/api/images/upload" ^
  -F "userId=1" ^
  -F "image=@path/to/your/image.jpg"
```

## Database Schema

The application uses the following tables:

- **Users**: Store user information
- **Images**: Store uploaded image metadata
- **Nutrients**: Store nutrient types (Calories, Protein, Fat, Carbohydrate)
- **Analysis**: Store AI analysis results linking images and nutrients

## Features

✅ User registration and authentication
✅ Image upload with validation
✅ Gemini AI integration for nutritional analysis
✅ Automatic nutrient extraction and storage
✅ RESTful API endpoints
✅ Swagger documentation
✅ CORS enabled
✅ Entity Framework Core with SQL Server
✅ Seed data for nutrients

## Configuration Options

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your SQL Server connection string"
  },
  "GeminiAPI": {
    "ApiKey": "Your Gemini API key"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string in `appsettings.json`
- Ensure Windows Authentication or SQL authentication is configured

### Migration Issues
If you need to reset the database:
```bash
dotnet ef database drop
dotnet ef database update
```

### Gemini API Issues
- Verify API key is correct
- Check internet connection
- Ensure you haven't exceeded API quota

## Development Commands

```bash
# Build the project
dotnet build

# Run the project
dotnet run

# Create a new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove

# Restore packages
dotnet restore
```

## Security Notes

⚠️ **Important Security Considerations:**
- The current password hashing uses SHA256 (for simplicity). Consider using BCrypt or ASP.NET Core Identity for production.
- Add JWT authentication for securing endpoints
- Store sensitive configuration in User Secrets or Azure Key Vault
- Implement rate limiting for API endpoints
- Add input validation and sanitization

## Next Steps

1. ✅ Set up the database
2. ✅ Configure Gemini API key
3. ✅ Test user registration
4. ✅ Test image upload
5. 📝 Implement JWT authentication (recommended)
6. 📝 Add logging and monitoring
7. 📝 Deploy to production

## Support

For issues or questions:
- Check the logs in the console output
- Review Swagger documentation
- Verify database connectivity
- Check Gemini API status

## License
MIT License
