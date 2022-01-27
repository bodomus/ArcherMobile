CREATE TABLE [dbo].[DocumentToUser] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [UserId]       NVARCHAR (450) NOT NULL,
    [DocumentId]   INT            NOT NULL,
    [DateInserted] DATETIME       NOT NULL,
    [DateUpdated]  DATETIME       NULL,
    [DateDeleted]  DATETIME       NULL,
    CONSTRAINT [PK_DocumentToUser] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DocumentToUser_Documents] FOREIGN KEY ([DocumentId]) REFERENCES [dbo].[Documents] ([Id]),
    CONSTRAINT [FK_DocumentToUser_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

