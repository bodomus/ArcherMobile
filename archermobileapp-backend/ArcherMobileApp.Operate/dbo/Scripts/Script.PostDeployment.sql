/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

--Admin guid
DECLARE @aguid nvarchar(450)
SET @aguid = N'bbdb2d8b-0ac2-4603-8496-35b8766d615d';

--INSERT ANNOUNCMENT TYPES
SET IDENTITY_INSERT [dbo].[AnnouncmentTypes] ON
INSERT INTO [AnnouncmentTypes] ([Id], [Title],[Description],[DateInserted],[DateUpdated],[DateDeleted]) VALUES (1, 'Important', 'Important announcment', GETDATE(), NULL, NULL);
SET IDENTITY_INSERT [dbo].[AnnouncmentTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[AnnouncmentTypes] ON
INSERT INTO [AnnouncmentTypes] ([Id], [Title],[Description],[DateInserted],[DateUpdated],[DateDeleted]) VALUES (2, 'No matter', 'No matter announcment', GETDATE(), NULL, NULL)
SET IDENTITY_INSERT [dbo].[AnnouncmentTypes] OFF
GO

SET IDENTITY_INSERT [dbo].[Announcements] ON
INSERT INTO [Announcements] ([Id], [Title],
[Date], [PublishDate], [ShortDescription], 
[Description],[AnnouncmentTypeId],[UserCreatetorId],  
[DateInserted],[DateUpdated],[DateDeleted]) VALUES 
(1, 'Announcment 1', GETDATE(), GETDATE(), 'Important announcment short 1',
'Important announcment 1', 1,@aguid, GETDATE(), NULL, NULL);
SET IDENTITY_INSERT [dbo].[Announcements] OFF
GO
SET IDENTITY_INSERT [dbo].[Announcements] ON
INSERT INTO [Announcements] ([Id], [Title],
[Date], [PublishDate], [ShortDescription], 
[Description],[AnnouncmentTypeId],[UserCreatetorId],  
[DateInserted],[DateUpdated],[DateDeleted]) VALUES 
(2, 'Announcment 2', GETDATE(), GETDATE(), 'Important announcment short 2',
'Important announcment 2', 2,@aguid, GETDATE(), NULL, NULL);
SET IDENTITY_INSERT [dbo].[Announcements] OFF
GO
SET IDENTITY_INSERT [dbo].[Announcements] ON
INSERT INTO [Announcements] ([Id], [Title],
[Date], [PublishDate], [ShortDescription], 
[Description],[AnnouncmentTypeId],[UserCreatetorId],  
[DateInserted],[DateUpdated],[DateDeleted]) VALUES 
(3, 'Announcment 3', GETDATE(), GETDATE(), 'Important announcment short 3',
'Important announcment 3', 2,@aguid, GETDATE(), NULL, NULL);
SET IDENTITY_INSERT [dbo].[Announcements] OFF
GO

--INSERT DOCUMENT TYPES
SET IDENTITY_INSERT [DocumentTypes] ON
GO
DBCC CHECKIDENT ('[DocumentTypes]', RESEED, 0);
GO
INSERT INTO [DocumentTypes] ([Id], [Name],[Description],[DateInserted],[DateUpdated],[DateDeleted]) VALUES (1, 'Company Goals', 'Company Goals Description', GETDATE(), NULL, NULL)
INSERT INTO [DocumentTypes] ([Id], [Name],[Description],[DateInserted],[DateUpdated],[DateDeleted]) VALUES (2, 'Mission', 'Mission Description', GETDATE(), NULL, NULL)
INSERT INTO [DocumentTypes] ([Id], [Name],[Description],[DateInserted],[DateUpdated],[DateDeleted]) VALUES (3, 'Vision', 'Vision Description', GETDATE(), NULL, NULL)
INSERT INTO [DocumentTypes] ([Id], [Name],[Description],[DateInserted],[DateUpdated],[DateDeleted]) VALUES (4, 'Values', 'Values Description', GETDATE(), NULL, NULL)
INSERT INTO [DocumentTypes] ([Id], [Name],[Description],[DateInserted],[DateUpdated],[DateDeleted]) VALUES (5, 'Company Internal Rules', 'Company Internal Rules Description', GETDATE(), NULL, NULL)

SET IDENTITY_INSERT [DocumentTypes] OFF
GO



--ADD DOCUMENTS 
SET IDENTITY_INSERT [Documents] ON
GO
DBCC CHECKIDENT ('[Documents]', RESEED, 0);
GO

INSERT INTO [Documents]([Id], [Title], [Description], [Uri], [DocumentTypeId], [DateInserted], [DateUpdated], [DateDeleted])
SELECT 1, 'Company Goals', 'Company Goals Description', 'https://docs.google.com/document/d/1QrPS1CLzofK5K1a2YlsZGaphazXrEmNqoZaip1xgGmM/edit?usp=sharing', 1, GETDATE(), NULL, NULL
UNION 
SELECT 2, 'Mission', 'Mission Description', 'https://docs.google.com/document/d/1_p4u4q4OeaqZnGvqQpe0Ken-QNPzU-vQtqhVpDx7Wy4/edit?usp=sharing', 2, GETDATE(), NULL, NULL
UNION
SELECT 3, 'Vision', 'Vision Description', 'https://docs.google.com/document/d/18G6ioVqvGS6Cvl85rxWIPCk2IJT-Qgup3CtXpc2mfa0/edit?usp=sharing', 3, GETDATE(), NULL, NULL
UNION
SELECT 4, 'Values', 'Values Description', 'https://docs.google.com/document/d/13kNjaIgZ373F-4h29HJlYW3uPauvSGUlWYuz_9EDpN0/edit?usp=sharing', 4, GETDATE(), NULL, NULLUNION
UNION
SELECT 5, 'Company Internal Rules', 'Company Internal Rules Description', 'https://docs.google.com/document/d/1ks5IBzdWG-Xkt8BwzpT2NRY34H2mcghFFkKNimuRwn8/edit?usp=sharing', 5, GETDATE(), NULL, NULL

SET IDENTITY_INSERT [Documents] OFF
GO

--INSERT ADMIN
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'bbdb2d8b-0ac2-4603-8496-35b8766d615d', N'bodomus@gmail.com', N'BODOMUS@GMAIL.COM', N'bodomus@gmail.com', N'BODOMUS@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAELD2n7AGqLYv6WFB/Qc6i+JarXIhBnFhJjAyaCuQ8hgNWDiUR2pTrmbTrS9xKV+38g==', N'ZE2K4KVVIP7BKDDTEUWX6ZYPI5MK7HMV', N'1d3b118f-bfcf-42e0-9649-68945956f699', NULL, 0, 0, NULL, 1, 0)


--INSERT ROLES
insert into [dbo].[AspNetRoles] (id, [ConcurrencyStamp], [Name], [NormalizedName]) select N'C7E76FEC-C7E0-42C0-95D9-B190E7CAE09A', NEWID(), 'Admins','ADMINS'
insert into [dbo].[AspNetRoles] (id, [ConcurrencyStamp], [Name], [NormalizedName]) select N'2F0A5B29-4656-4370-900D-DAC5731874FE', NEWID(), 'Employees','EMPLOYEES'
insert into [dbo].[AspNetRoles] (id, [ConcurrencyStamp], [Name], [NormalizedName]) select N'B8D26222-D7CE-41B7-BAAA-83ABFCA781A3', NEWID(), 'Project Team','PROJECT TEAM'
insert into [dbo].[AspNetRoles] (id, [ConcurrencyStamp], [Name], [NormalizedName]) select N'7FE959F3-F8B5-401F-B06C-88B838EFA1F8', NEWID(), 'Stakeholders','STAKEHOLDERS'
insert into [dbo].[AspNetRoles] (id, [ConcurrencyStamp], [Name], [NormalizedName]) select N'72E40962-47AE-4FF6-A6BC-51C6DF6CAF6E', NEWID(), 'QA','QA'
insert into [dbo].[AspNetRoles] (id, [ConcurrencyStamp], [Name], [NormalizedName]) select N'DCF41AB6-422F-48DC-AF3F-E1678E417D1A', NEWID(), 'HR','HR'
insert into [dbo].[AspNetRoles] (id, [ConcurrencyStamp], [Name], [NormalizedName]) select N'C83C549C-BEC7-4BAB-9B6E-4FDF4E5108DB', NEWID(), 'PM','PM'
insert into [dbo].[AspNetRoles] (id, [ConcurrencyStamp], [Name], [NormalizedName]) select N'2B8839BE-7385-48E9-8A42-245048CA4D74', NEWID(), 'Infrastructure','INFRASTRUCTURE'
	

--INSERT LINK ADMIN
insert into [dbo].[AspNetUserRoles] ([UserId],[RoleId]) select N'bbdb2d8b-0ac2-4603-8496-35b8766d615d', N'C7E76FEC-C7E0-42C0-95D9-B190E7CAE09A' from dbo.AspNetUsers


--INSERT INTO USER
INSERT INTO USERS (
	[Id], 
	[ConfirmICR], 
	[DateInserted],
	[DateUpdated],
	[DateDeleted],
	[LastLogin],
	[KeepLogged],
	[IsAdmin],
	[Email])
	VALUES (
	N'bbdb2d8b-0ac2-4603-8496-35b8766d615d', 1, GETDATE(), NULL, NULL, GETDATE(), 1, 1, N'bodomus@gmail.com'
	)


