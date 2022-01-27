CREATE TABLE [dbo].[Appoinments] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Appoinment]   NVARCHAR (50)  NULL,
    [OwnerId]      NVARCHAR (450) NOT NULL,
    [RoomId]       INT            NOT NULL,
    [Start]        DATETIME       NULL,
    [End]          DATETIME       NULL,
    [DateInserted] DATETIME       CONSTRAINT [DF_Appoinments_DateInserted] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]  DATETIME       NULL,
    [DateDeleted]  DATETIME       NULL,
    CONSTRAINT [PK_Appoinments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Appoinments_Rooms] FOREIGN KEY ([RoomId]) REFERENCES [dbo].[Rooms] ([Id]),
    CONSTRAINT [FK_Appoinments_Users] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[Users] ([Id])
);

