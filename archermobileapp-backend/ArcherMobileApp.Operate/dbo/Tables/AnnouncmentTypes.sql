CREATE TABLE [dbo].[AnnouncmentTypes] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Title]        NVARCHAR (50)  NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [DateInserted] DATETIME       CONSTRAINT [DF_AnnouncmentTypes_DateInserted] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]  DATETIME       NULL,
    [DateDeleted]  DATETIME       NULL,
    CONSTRAINT [PK_AnnouncmentTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

