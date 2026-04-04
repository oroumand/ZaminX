IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'dbo')
BEGIN
    EXEC(N'CREATE SCHEMA [dbo]');
END;
GO

IF OBJECT_ID(N'dbo.ParrotTranslations', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ParrotTranslations]
    (
        [Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Key] NVARCHAR(450) NOT NULL,
        [Culture] NVARCHAR(32) NULL,
        [Value] NVARCHAR(MAX) NOT NULL,
        [CreatedAtUtc] DATETIME2 NOT NULL CONSTRAINT [DF_ParrotTranslations_CreatedAtUtc] DEFAULT SYSUTCDATETIME(),
        [UpdatedAtUtc] DATETIME2 NOT NULL CONSTRAINT [DF_ParrotTranslations_UpdatedAtUtc] DEFAULT SYSUTCDATETIME()
    );

    CREATE UNIQUE INDEX [UX_ParrotTranslations_Key_Culture]
        ON [dbo].[ParrotTranslations] ([Key], [Culture]);
END;
GO