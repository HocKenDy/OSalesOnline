create table Crm_Note
(
	Id						int identity
	, IsDeleted				bit default(0)
	, CreatedDate			datetime default getdate()
	, CreatedUserId			int
	, ModifiedDate			datetime default getdate()
	, ModifiedUserId		int
	, AssignedUserId		int
	
	, [Code] nvarchar(50)
	, [Name] nvarchar(250)
	, [Content] nvarchar(MAX)
	, [ContentHtml] nvarchar(MAX)

		
	Primary Key(Id)
)

-- lấy id của dòng trên sau khi Execute điền vào cột [ParentId]
INSERT INTO [dbo].[System_Page]
           ([Name],[AreaName],[ActionName],[ControllerName],[Url],[OrderNo],[Status],[CssClassIcon]
           ,[IsDeleted],[IsVisible],[IsView],[IsAdd],[IsEdit],[IsDelete],[IsImport],[IsExport],[IsPrint])
     VALUES
           ('Crm_Note_Index', N'Crm', N'Index', N'Note', NULL, 1, 1, NULL
           , 0, 1, 0, 0, 0, 0, 0, 0, 0);

INSERT INTO [dbo].[System_Page]
           ([Name],[AreaName],[ActionName],[ControllerName],[Url],[OrderNo],[Status],[CssClassIcon]
           ,[IsDeleted],[IsVisible],[IsView],[IsAdd],[IsEdit],[IsDelete],[IsImport],[IsExport],[IsPrint])
     VALUES
           ('Crm_Note_Create', N'Crm', N'Create', N'Note', NULL, 2, 1, NULL
           , 0, 0, 0, 0, 0, 0, 0, 0, 0);

INSERT INTO [dbo].[System_Page]
           ([Name],[AreaName],[ActionName],[ControllerName],[Url],[OrderNo],[Status],[CssClassIcon]
           ,[IsDeleted],[IsVisible],[IsView],[IsAdd],[IsEdit],[IsDelete],[IsImport],[IsExport],[IsPrint])
     VALUES
           ('Crm_Note_Edit', N'Crm', N'Edit', N'Note', NULL, 3, 1, NULL
           , 0, 0, 0, 0, 0, 0, 0, 0, 0);
		   
INSERT INTO [dbo].[System_Page]
           ([Name],[AreaName],[ActionName],[ControllerName],[Url],[OrderNo],[Status],[CssClassIcon]
           ,[IsDeleted],[IsVisible],[IsView],[IsAdd],[IsEdit],[IsDelete],[IsImport],[IsExport],[IsPrint])
     VALUES
           ('Crm_Note_Detail', N'Crm', N'Detail', N'Note', NULL, 3, 1, NULL
           , 0, 0, 0, 0, 0, 0, 0, 0, 0);

INSERT INTO [dbo].[System_Page]
           ([Name],[AreaName],[ActionName],[ControllerName],[Url],[OrderNo],[Status],[CssClassIcon]
           ,[IsDeleted],[IsVisible],[IsView],[IsAdd],[IsEdit],[IsDelete],[IsImport],[IsExport],[IsPrint])
     VALUES
           ('Crm_Note_Delete', N'Crm', N'Delete', N'Note', NULL, 4, 1, NULL
           , 0, 0, 0, 0, 0, 0, 0, 0, 0);
GO

---- sau khi chạy 5 dòng trên thì lấy id tương ứng của 5 dòng đó điền vào cột [PageId]
---------------------------------------------------------
INSERT INTO [dbo].[System_PageMenu]
           ([LanguageId],[PageId],[Name])
     VALUES
           ('vi-VN', (select Id from [System_Page] where [ActionName] = N'Index' and [ControllerName] = N'Note'), N'Ghi chú')
GO

SELECT TOP 5 *
  FROM [dbo].[System_Page]
  order by id desc

SELECT TOP 1 [LanguageId], Name
  FROM [dbo].[System_PageMenu]
  order by id desc