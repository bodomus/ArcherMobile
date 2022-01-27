--INSERT ANNOUNCMENT TYPES
SET IDENTITY_INSERT [dbo].[Users] ON
INSERT INTO [AnnouncmentTypes] ([Id], [Title],[Description],[DateInserted],[DateUpdated],[DateDeleted]) VALUES (1, 'Important', 'Important announcment', GETDATE(), NULL, NULL);
GO
INSERT INTO [AnnouncmentTypes] ([Id], [Title],[Description],[DateInserted],[DateUpdated],[DateDeleted]) VALUES (2, 'No matter', 'No matter announcment', GETDATE(), NULL, NULL)

--INSERT DOCUMENT TYPES
INSERT INTO [DocumentTypes] ([Id], [Name],[Description],[DateInserted],[DateUpdated],[DateDeleted]) VALUES (1, 'Company Goals', 'Company Goals Description', GETDATE(), NULL, NULL)
INSERT INTO [DocumentTypes] ([Id], [Name],[Description],[DateInserted],[DateUpdated],[DateDeleted]) VALUES (2, 'Mission', 'Mission Description', GETDATE(), NULL, NULL)
INSERT INTO [DocumentTypes] ([Id], [Name],[Description],[DateInserted],[DateUpdated],[DateDeleted]) VALUES (3, 'Vision', 'Vision Description', GETDATE(), NULL, NULL)
INSERT INTO [DocumentTypes] ([Id], [Name],[Description],[DateInserted],[DateUpdated],[DateDeleted]) VALUES (4, 'Values', 'Values Description', GETDATE(), NULL, NULL)
INSERT INTO [DocumentTypes] ([Id], [Name],[Description],[DateInserted],[DateUpdated],[DateDeleted]) VALUES (5, 'Company Internal Rules', 'Company Internal Rules Description', GETDATE(), NULL, NULL)


--ADD DOCUMENTS 

INSERT INTO [Documents]([Id], [Title], [Description], [Uri], [DocumentTypeId], [DateInserted], [DateUpdated], [DateDeleted])
SELECT 1, 'Company Goals', 'Company Goals Description', 'https://docs.google.com/document/d/e/2PACX-1vR_yyGpg6FpzcjvBDgAoxay_waWOkaolKvirl-uDtsNgglphUM_AFnmKwixkCaednN4lyVzP_XJjUfc/pub', 1, GETDATE(), NULL, NULL
UNION 
SELECT 2, 'Mission', 'Mission Description', 'https://docs.google.com/document/d/e/2PACX-1vTPUhoSNUxML0WFwEMoCnqN31S-BglX6A-qTr-HXKELW7TJhbqoZHLGUo7TXqKiGZfsfdr0tu3rvAXY/pub', 2, GETDATE(), NULL, NULL
UNION
SELECT 3, 'Vision', 'Vision Description', 'https://docs.google.com/document/d/e/2PACX-1vS4wuuQeqygcBxCSb35Eu0YJXt2R8j8DR7z52TR9zh9-M8hzhqTWhYoxJnx2leXC6snEDOOyXKfFCfU/pub', 3, GETDATE(), NULL, NULL
UNION
SELECT 4, 'Values', 'Values Description', 'https://docs.google.com/document/d/e/2PACX-1vRAHfRI78P4XTfD-_jGN9ypGqkOTAptp6ueredTzJkMKD_KCL1VWihcl1AuxLnCLfRGgFWSjF39D67Q/pub', 4, GETDATE(), NULL, NULLUNION
UNION
SELECT 5, 'Company Internal Rules', 'Company Internal Rules Description', 'https://docs.google.com/document/d/e/2PACX-1vQmjL0CaYy2858k-HqWaIl2h6a_zp4KX_ExTe3BCnSRuq23uO2az-YlhkVxu6nxh8wo2KGvBsywMCoj/pub', 5, GETDATE(), NULL, NULL


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
	[IsAdmin])
	VALUES (
	N'bbdb2d8b-0ac2-4603-8496-35b8766d615d', 1, GETDATE(), NULL, NULL, GETDATE(), 1, 1
	)


