CREATE TABLE [dbo].[JobOpportunities] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [Title]             NVARCHAR (50)  NOT NULL,
    [Description]       NVARCHAR (MAX) NULL,
    [Responsibilities]  NVARCHAR (450) NOT NULL,
    [Requirements]      NVARCHAR (450) NOT NULL,
    [StandOut]          NVARCHAR (450) NOT NULL,
    [RecruiterContacts] NVARCHAR (450) NOT NULL,
    [IsArchive]         BIT            CONSTRAINT [DF_JobOpportunities_IsArchive] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_JobOpportunities] PRIMARY KEY CLUSTERED ([Id] ASC)
);

