viết 1 tài liệu hướng dẫn để cho backend .net api call gemini api  nhận dạng thành phần di dương cho github copiot để code, tôi dùng vscode 

```sql
-- Tạo cơ sở dữ liệu
CREATE DATABASE nutrition_app_db;
USE nutrition_app_db;

-- Bảng Users: Lưu thông tin người dùng
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    [Password] NVARCHAR(255) NOT NULL,  -- Nên lưu password đã hash
    Email NVARCHAR(100) UNIQUE,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Bảng Images: Lưu thông tin hình ảnh
CREATE TABLE Images (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    [Path] NVARCHAR(255) NOT NULL,  -- Đường dẫn file hình ảnh
    Kcal DECIMAL(10,2),
    Gam DECIMAL(10,2),
    AiSuggestion NVARCHAR(MAX),     -- Gợi ý từ AI
    CreatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Images_Users FOREIGN KEY (UserId)
        REFERENCES Users(Id) ON DELETE CASCADE
);
GO

-- Bảng Nutrients: Lưu chi tiết dinh dưỡng
CREATE TABLE Nutrients (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL,   -- Ví dụ: Protein, Fat, Carb
    [Unit] NVARCHAR(20) NOT NULL,   -- Ví dụ: gram, kcal
    [Description] NVARCHAR(MAX)
);
GO

-- Bảng Analysis: Liên kết kết quả phân tích AI với hình ảnh
CREATE TABLE Analysis (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ImageId INT NOT NULL,
    NutrientId INT NOT NULL,
    [Value] DECIMAL(10,2) NOT NULL,  -- Giá trị (ví dụ: 20.5)
    Confidence DECIMAL(5,2),         -- Độ tin cậy AI (ví dụ: 0.95)
    CreatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Analysis_Images FOREIGN KEY (ImageId)
        REFERENCES Images(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Analysis_Nutrients FOREIGN KEY (NutrientId)
        REFERENCES Nutrients(Id) ON DELETE NO ACTION
);
GO

-- Dữ liệu mẫu cho bảng Nutrients
INSERT INTO Nutrients ([Name], [Unit], [Description]) VALUES
(N'Calories', N'kcal', N'Tổng năng lượng'),
(N'Protein', N'gram', N'Chất đạm'),
(N'Fat', N'gram', N'Chất béo'),
(N'Carbohydrate', N'gram', N'Tinh bột');
GO
```


Update Table

```sql
ALTER TABLE Images
ADD DishName NVARCHAR(255) NULL;
GO
```


