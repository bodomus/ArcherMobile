CREATE TABLE [dbo].[DocumentTypes] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50)  NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [DateInserted] DATETIME       NOT NULL,
    [DateUpdated]  DATETIME       NULL,
    [DateDeleted]  DATETIME       NULL,
    CONSTRAINT [PK_DocumentTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

