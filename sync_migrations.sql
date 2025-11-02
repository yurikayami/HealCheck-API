-- Đồng bộ migration history với database hiện tại
-- Chạy script này trong SQL Server Management Studio hoặc Azure Data Studio

USE nutrition_app_db;
GO

-- Tạo bảng __EFMigrationsHistory nếu chưa có
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__EFMigrationsHistory]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[__EFMigrationsHistory](
        [MigrationId] [nvarchar](150) NOT NULL,
        [ProductVersion] [nvarchar](32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC)
    );
END
GO

-- Thêm migration InitialCreate vào history (vì database đã được tạo thủ công)
IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20251031152114_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20251031152114_InitialCreate', '9.0.10');
END
GO

-- Kiểm tra xem cột FoodName đã tồn tại chưa, nếu chưa thì thêm
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Images]') AND name = 'FoodName')
BEGIN
    ALTER TABLE [dbo].[Images]
    ADD [FoodName] NVARCHAR(255) NULL;
    PRINT 'Đã thêm cột FoodName vào bảng Images';
END
ELSE
BEGIN
    PRINT 'Cột FoodName đã tồn tại trong bảng Images';
END
GO

-- Thêm migration AddFoodNameToImages vào history
IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20251102171856_AddFoodNameToImages')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20251102171856_AddFoodNameToImages', '9.0.10');
    PRINT 'Đã thêm migration AddFoodNameToImages vào history';
END
ELSE
BEGIN
    PRINT 'Migration AddFoodNameToImages đã tồn tại trong history';
END
GO

-- Kiểm tra kết quả
SELECT * FROM [__EFMigrationsHistory];
GO

PRINT 'Hoàn tất đồng bộ migrations!';
GO
