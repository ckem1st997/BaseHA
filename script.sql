USE [Category]
GO

CREATE TABLE [dbo].[CategoryTB](
	[Id] [varchar](36) NOT NULL,
	[Category] [varchar](255) NOT NULL,
	[Intent_Code_EN] [nvarchar](255) NULL,
	[Intent_Code_VN] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[OnDelete] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Intent](
	[Id] [varchar](36) NOT NULL,
	[Intent_Code_EN] [nvarchar](255) NULL,
	[Intent_EN] [nvarchar](255) NULL,
	[Intent_VN] [nvarchar](255) NULL,
	[CategoryId] [varchar](36) NULL,
	[OnDelete] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE Intent
ADD CONSTRAINT FK_Category_Intent FOREIGN KEY (CategoryId)
REFERENCES CategoryTB(Id); -- Đặt ràng buộc khóa ngoại từ cột CategoryId của bảng Intent đến cột Id của bảng Category


INSERT INTO [Intent] (Id, Intent_Code_EN, Intent_EN, Intent_VN, OnDelete)
VALUES
(NEWID(),'cancel_order','I want to cancel the order I have made',N'tôi muốn hủy đơn tôi vừa tạo',0),
(NEWID(),'cancel_order','how do I cancel my purchase?',N'làm thế nào để tôi hủy đơn mua hàng của tôi',0),
(NEWID(),'cancel_order','how could I cancel the last damn order I made ?',N'làm thế nào tôi có thể hủy đơn hàng cuối cùng tôi đã tạo',0),
(NEWID(),'cancel_order','I try to cancel an order',N'tôi cố gắng hủy 1 đơn',0),
(NEWID(),'cancel_order','i need assistance to cancel my purchase',N'tôi muốn hướng dẫn để hủy đơn',0)


INSERT INTO [CategoryTB] (Id, Category, Intent_Code_EN, Intent_Code_VN, Description, OnDelete)
VALUES
(NEWID(),'ORDER', 'cancel_order', 'huy_hd', N'Hủy đơn',0),
(NEWID(),'ORDER', 'change_order', 'thay_doi_don', N'Thay đổi đơn',0),
(NEWID(),'ORDER', 'change_shipping_address', 'thay_doi_dc_ship', N'Thay đổi địa chỉ ship',0),
(NEWID(),'ORDER', 'check_cancellation_fee', 'kt_phi_huy', N'Kiểm tra phí hủy',0),
(NEWID(),'ORDER', 'check_invoice', 'huy_hd', N'Kiểm tra hóa đơn',0)


UPDATE Intent
SET Intent.CategoryId = CategoryTB.Id
FROM Intent
INNER JOIN CategoryTB ON CategoryTB.Intent_Code_EN = Intent.Intent_Code_EN;


ALTER TABLE [dbo].[CategoryTB] ADD  DEFAULT ('') FOR [Id]
GO
ALTER TABLE [dbo].[CategoryTB] ADD  DEFAULT (NULL) FOR [Category]
GO
ALTER TABLE [dbo].[CategoryTB] ADD  DEFAULT (NULL) FOR [Intent_Code_EN]
GO
ALTER TABLE [dbo].[CategoryTB] ADD  DEFAULT (NULL) FOR [Intent_Code_VN]
GO
ALTER TABLE [dbo].[CategoryTB] ADD  DEFAULT (NULL) FOR [Description]
GO
ALTER TABLE [dbo].[CategoryTB] ADD  CONSTRAINT [DF_CategoryTB_OnDelete]  DEFAULT ((0)) FOR [OnDelete]
GO


ALTER TABLE [dbo].[Intent] ADD  DEFAULT ('') FOR [Id]
GO
ALTER TABLE [dbo].[Intent] ADD  DEFAULT (NULL) FOR [Intent_Code_EN]
GO
ALTER TABLE [dbo].[Intent] ADD  DEFAULT (NULL) FOR [Intent_EN]
GO
ALTER TABLE [dbo].[Intent] ADD  DEFAULT (NULL) FOR [Intent_VN]
GO
ALTER TABLE [dbo].[Intent] ADD  DEFAULT (NULL) FOR [CategoryId]
GO