CREATE TABLE [dbo].[Rooms] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [Name]                 NVARCHAR (50)  NOT NULL,
    [Description]          NVARCHAR (MAX) NULL,
    [PhysicalAddress]      NVARCHAR (450) NULL,
    [LinkToGoogleCalendar] NVARCHAR (450) NULL,
    [DateInserted]         DATETIME       NOT NULL,
    [DateUpdated]          DATETIME       NULL,
    [DateDeleted]          DATETIME       NULL,
    CONSTRAINT [PK_Rooms\] PRIMARY KEY CLUSTERED ([Id] ASC)
);

