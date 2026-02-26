USE user15
GO


-- Создание таблицы пользователей для авторизации
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Login NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(100)
);

-- Таблица разработчиков
CREATE TABLE Developers (
    DeveloperID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

-- Таблица издателей
CREATE TABLE Publishers (
    PublisherID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

-- Таблица игровых платформ
CREATE TABLE Platforms (
    PlatformID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL
);

-- Основная таблица игр
CREATE TABLE Games (
    GameID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    DeveloperID INT FOREIGN KEY REFERENCES Developers(DeveloperID),
    PublisherID INT FOREIGN KEY REFERENCES Publishers(PublisherID),
    Mechanics NVARCHAR(255),
    Protagonist NVARCHAR(100),
    PlatformID INT FOREIGN KEY REFERENCES Platforms(PlatformID)
);