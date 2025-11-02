# Nutrition Analysis API

This project is a .NET Core Web API that allows users to upload images of food, and the API will use the Gemini API to analyze the nutritional components of the food in the image. The analysis results are then stored in a SQL Server database.

## Features

*   User registration and authentication.
*   Image upload.
*   Nutritional analysis of food images using the Gemini API.
*   Store and retrieve nutritional analysis results.

## Technologies Used

*   .NET Core
*   ASP.NET Core Web API
*   Entity Framework Core
*   SQL Server
*   Gemini API
*   Visual Studio Code

## Prerequisites

*   [.NET SDK](https://dotnet.microsoft.com/download)
*   [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
*   [Visual Studio Code](https://code.visualstudio.com/)
*   [Git](https://git-scm.com/)

## Getting Started

### 1. Clone the repository

```bash
git clone <repository-url>
cd <repository-name>
```

### 2. Database Setup

*   Open SQL Server Management Studio or any other SQL Server client.
*   Execute the SQL script below to create the database and tables.

### 3. Configuration

*   Open the `appsettings.json` file.
```cshape
"ConnectionStrings": {
  "DefaultConnection": "Server=DESKTOP-YURI\\SQLEXPRESS;Database=nutrition_app_db;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
*   Update the `ConnectionStrings` section with your SQL Server connection string.
*   Add your Gemini API key to the configuration.

### 4. Running the application

*   Open the project in Visual Studio Code.
*   Open the terminal and run the following command:

```bash
dotnet run
```

The API will be running at `https://localhost:5001` (or a similar port).

## API Endpoints

*   `POST /api/users/register`: Register a new user.
*   `POST /api/users/login`: Login a user.
*   `POST /api/images/upload`: Upload an image for analysis.
*   `GET /api/images/{id}`: Get the analysis result for an image.

## Database Schema

```sql
-- Create database
CREATE DATABASE nutrition_app_db;
USE nutrition_app_db;

-- Users table: Stores user information
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    [Password] NVARCHAR(255) NOT NULL,  -- Should store hashed password
    Email NVARCHAR(100) UNIQUE,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Images table: Stores image information
CREATE TABLE Images (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    [Path] NVARCHAR(255) NOT NULL,  -- Path to the image file
    Kcal DECIMAL(10,2),
    Gam DECIMAL(10,2),
    AiSuggestion NVARCHAR(MAX),     -- Suggestion from AI
    CreatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Images_Users FOREIGN KEY (UserId)
        REFERENCES Users(Id) ON DELETE CASCADE
);
GO

-- Nutrients table: Stores nutrient details
CREATE TABLE Nutrients (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL,   -- Example: Protein, Fat, Carb
    [Unit] NVARCHAR(20) NOT NULL,   -- Example: gram, kcal
    [Description] NVARCHAR(MAX)
);
GO

-- Analysis table: Links AI analysis results with images
CREATE TABLE Analysis (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ImageId INT NOT NULL,
    NutrientId INT NOT NULL,
    [Value] DECIMAL(10,2) NOT NULL,  -- Value (e.g., 20.5)
    Confidence DECIMAL(5,2),         -- AI confidence (e.g., 0.95)
    CreatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Analysis_Images FOREIGN KEY (ImageId)
        REFERENCES Images(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Analysis_Nutrients FOREIGN KEY (NutrientId)
        REFERENCES Nutrients(Id) ON DELETE NO ACTION
);
GO

-- Sample data for Nutrients table
INSERT INTO Nutrients ([Name], [Unit], [Description]) VALUES
(N'Calories', N'kcal', N'Total energy'),
(N'Protein', N'gram', N'Protein'),
(N'Fat', N'gram', N'Fat'),
(N'Carbohydrate', N'gram', N'Carbohydrate');
GO
```

## Contributing

Contributions are welcome! Please feel free to submit a pull request.

## License

This project is licensed under the MIT License.
