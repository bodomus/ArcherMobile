CREATE TABLE [dbo].[Users] (
    [Id]           NVARCHAR (450) NOT NULL,
    [ConfirmICR]   BIT            CONSTRAINT [DF_Users_ConfirmICR] DEFAULT ((0)) NOT NULL,
    [DateInserted] DATETIME       CONSTRAINT [DF_Users_DateInserted] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]  DATETIME       NULL,
    [DateDeleted]  DATETIME       NULL,
    [LastLogin]    DATETIME       NULL,
    [KeepLogged]   BIT            CONSTRAINT [DF_Users_KeepLogged] DEFAULT ((0)) NOT NULL,
    [IsAdmin]      BIT            CONSTRAINT [DF_Users_IsAdmin] DEFAULT ((0)) NOT NULL,
    [ICRConfirmationDate] DATETIME       NULL,
    [UserName]            NVARCHAR (50)  NULL,
    [RefreshToken]        NVARCHAR (50)  NULL,
    [Email]               NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Users_AspNetUsers] FOREIGN KEY ([Id]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

