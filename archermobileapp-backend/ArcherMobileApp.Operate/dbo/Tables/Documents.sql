CREATE TABLE [dbo].[Documents] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (50)  NOT NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [Uri]            NVARCHAR (450) NULL,
    [DocumentTypeId] INT            NOT NULL,
    [DateInserted]   DATETIME       NOT NULL,
    [DateUpdated]    DATETIME       NULL,
    [DateDeleted]    DATETIME       NULL,
    CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Documents_DocumentTypes] FOREIGN KEY ([DocumentTypeId]) REFERENCES [dbo].[DocumentTypes] ([Id])
);

