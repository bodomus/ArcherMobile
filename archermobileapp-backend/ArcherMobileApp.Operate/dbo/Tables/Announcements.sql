CREATE TABLE [dbo].[Announcements] (
    [id]                INT            IDENTITY (1, 1) NOT NULL,
    [Title]             NVARCHAR (64)  NOT NULL,
    [Date]              DATETIME       NULL,
    [PublishDate]       DATETIME       NULL,
    [ShortDescription]  NVARCHAR (100) NULL,
    [Description]       NVARCHAR (MAX) NULL,
    [AnnouncmentTypeId] INT            NOT NULL,
    [UserCreatetorId]   NVARCHAR (450) NULL,
    [DateInserted]      DATETIME       CONSTRAINT [DF_Announcements_DateInserted] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]       DATETIME       NULL,
    [DateDeleted]       DATETIME       NULL,
    CONSTRAINT [PK_Announcements] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Announcements_AnnouncmentTypes] FOREIGN KEY ([AnnouncmentTypeId]) REFERENCES [dbo].[AnnouncmentTypes] ([Id])
);

