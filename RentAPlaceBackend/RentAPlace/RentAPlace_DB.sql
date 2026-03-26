IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [FullName] nvarchar(100) NOT NULL,
    [Email] nvarchar(150) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [Role] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
);
GO

CREATE TABLE [Properties] (
    [PropertyId] int NOT NULL IDENTITY,
    [OwnerId] int NOT NULL,
    [Title] nvarchar(200) NOT NULL,
    [Description] nvarchar(2000) NULL,
    [Location] nvarchar(300) NOT NULL,
    [PropertyType] nvarchar(max) NOT NULL,
    [PricePerNight] decimal(10,2) NOT NULL,
    [Features] nvarchar(max) NULL,
    [ImageUrls] nvarchar(max) NULL,
    [IsActive] bit NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Properties] PRIMARY KEY ([PropertyId]),
    CONSTRAINT [FK_Properties_Users_OwnerId] FOREIGN KEY ([OwnerId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Messages] (
    [MessageId] int NOT NULL IDENTITY,
    [PropertyId] int NOT NULL,
    [SenderId] int NOT NULL,
    [ReceiverId] int NOT NULL,
    [Content] nvarchar(2000) NOT NULL,
    [ParentMessageId] int NULL,
    [SentAt] datetime2 NOT NULL,
    [IsRead] bit NOT NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY ([MessageId]),
    CONSTRAINT [FK_Messages_Properties_PropertyId] FOREIGN KEY ([PropertyId]) REFERENCES [Properties] ([PropertyId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Messages_Users_ReceiverId] FOREIGN KEY ([ReceiverId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Messages_Users_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Reservations] (
    [ReservationId] int NOT NULL IDENTITY,
    [PropertyId] int NOT NULL,
    [RenterId] int NOT NULL,
    [CheckInDate] datetime2 NOT NULL,
    [CheckOutDate] datetime2 NOT NULL,
    [TotalPrice] decimal(10,2) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Reservations] PRIMARY KEY ([ReservationId]),
    CONSTRAINT [FK_Reservations_Properties_PropertyId] FOREIGN KEY ([PropertyId]) REFERENCES [Properties] ([PropertyId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Reservations_Users_RenterId] FOREIGN KEY ([RenterId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Reviews] (
    [ReviewId] int NOT NULL IDENTITY,
    [PropertyId] int NOT NULL,
    [ReviewerId] int NOT NULL,
    [Rating] int NOT NULL,
    [Comment] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Reviews] PRIMARY KEY ([ReviewId]),
    CONSTRAINT [FK_Reviews_Properties_PropertyId] FOREIGN KEY ([PropertyId]) REFERENCES [Properties] ([PropertyId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Reviews_Users_ReviewerId] FOREIGN KEY ([ReviewerId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_Messages_PropertyId] ON [Messages] ([PropertyId]);
GO

CREATE INDEX [IX_Messages_ReceiverId] ON [Messages] ([ReceiverId]);
GO

CREATE INDEX [IX_Messages_SenderId] ON [Messages] ([SenderId]);
GO

CREATE INDEX [IX_Properties_OwnerId] ON [Properties] ([OwnerId]);
GO

CREATE INDEX [IX_Reservations_PropertyId] ON [Reservations] ([PropertyId]);
GO

CREATE INDEX [IX_Reservations_RenterId] ON [Reservations] ([RenterId]);
GO

CREATE INDEX [IX_Reviews_PropertyId] ON [Reviews] ([PropertyId]);
GO

CREATE INDEX [IX_Reviews_ReviewerId] ON [Reviews] ([ReviewerId]);
GO

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260314182545_InitialCreate', N'8.0.25');
GO

COMMIT;
GO

