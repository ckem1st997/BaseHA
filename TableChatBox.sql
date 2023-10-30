USE [WarehouseManagement]
GO
/****** Object:  Table [dbo].[Answers]    Script Date: 09/30/2023 7:39:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Answers](
	[Id] [varchar](36) NOT NULL,
	[CategoryID] [varchar](36) NULL,
	[AnswerVN] [nvarchar](1000) NULL,
	[Inactive] [bit] NOT NULL,
	[OnDelete] [bit] NOT NULL,
 CONSTRAINT [pk_id_a] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 09/30/2023 7:39:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [varchar](36) NOT NULL,
	[NameCategory] [nvarchar](100) NOT NULL,
	[IntentCodeEN] [nvarchar](255) NOT NULL,
	[IntentCodeVN] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[ParentID] [nvarchar](36) NULL,
	[Inactive] [bit] NOT NULL,
	[OnDelete] [bit] NOT NULL,
 CONSTRAINT [pk_id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Intents]    Script Date: 09/30/2023 7:39:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Intents](
	[Id] [varchar](36) NOT NULL,
	[CategoryID] [varchar](36) NULL,
	[IntentEN] [nvarchar](255) NULL,
	[IntentVN] [nvarchar](255) NULL,
	[Inactive] [bit] NOT NULL,
	[OnDelete] [bit] NOT NULL,
 CONSTRAINT [pk_id_i] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'0623608b-56f0-445e-8ec0-df203e6261f4', N'1d96fd2e-a549-4928-ac2f-4dc438b7c110', N'Quý khách vui lòng truy cập vào website sau để được hướng dẫn mua hàng: HACOM - Hướng Dẫn Mua Hàng Trực Tuyến
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'072b57a6-145e-4f07-b3b8-3fd74318f997', N'5591a211-93c5-493a-8a57-2dd6467fd529', N'Đây là danh sách sản phẩm bên mình đang quan tâm: https://hacom.vn/thiet-bi-mang
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'0a57ce12-70ff-4ee4-8f5d-e2d4af5d155e', N'e633e720-0f59-4b16-9cbc-818f1c7f98af', N'Bạn vui lòng truy cập https://hacom.vn/chinh-sach-bao-hanh để biết thêm thông tin về chính sách cũng như điều kiện bảo hành
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'0c20df70-6ec6-4269-836d-dc962ff91118', N'ac354e61-31bb-4572-8003-5ea6723d9938', N'Bạn vui long cung cấp thêm thông tin về sản phẩm( Tên sản phẩm, cấu hình, dung lượng,..)
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'10e3aff1-d7b9-4346-81fa-d86c19a209a4', N'e633e720-0f59-4b16-9cbc-818f1c7f98af', NULL, 0, 1)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'1b6632dc-f8ed-4ea2-9fa7-eff0662c2db7', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', N'Bạn vui lòng liên hệ bên kỹ thuật hoặc đến cửa hàng HACOM gần nhất để được hỗ trợ kịp thời!
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'1b7f8b9d-22f0-400c-8240-cf0ca6e3e2bd', N'963b7f92-731a-4abb-9eb6-1426b811e81b', N'Đây là danh sách sản phẩm bên mình đang quan tâm: https://hacom.vn/may-tinh-de-ban
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'20437ff5-2a03-412f-ba24-c39d6b42241a', N'e34e212b-be06-4f28-8260-8eb813f6b3e1', N'HACOM hiện đang hỗ trợ trả góp 0%, thông tin xem tại: https://hacom.vn/huong-dan-mua-hang-tra-gop
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'2dfcfcdd-fe3b-4124-b985-23d6ed2aa659', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', N'Nếu hiện tại đang quá giờ hành chính thì quý khách vui lòng chờ đến sáng mai CSKH sẽ liên hệ lại
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'2fb5fb7c-6de9-4df3-961a-194964733aa8', N'21af92c6-ed87-44d4-ad57-3cb31db89e45', N'Bên mình không thu mua lại SSD đã qua sử dụng đâu bạn nhé!
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'2fbe0011-6842-4220-985a-22061e2eacdf', N'90b8802e-d76b-44ef-9e97-2287ac5688ef', N'Đây là danh sách sản phẩm bên mình đang quan tâm: https://hacom.vn/laptop
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'33f66b89-6af8-4381-9b11-6875c78ed2d2', N'5f52f46b-949a-4ba2-a275-85980e3fd7b3', N'Bên mình có hỗ trợ cài đặt hệ điều hành Windows, bộ công cụ Office 365 và các phần mềm Antivirus bản quyền. Chi tiết xem tại: https://hacom.vn/phan-mem-ban-quyen
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'403cb8e8-fce7-4920-868f-ff9e8dd8dbca', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', N'Bạn vui lòng mang máy qua để bên mình kiểm tra nhé!
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'43f5ae0d-7924-4bfa-b51b-ab023b4212f5', N'e633e720-0f59-4b16-9cbc-818f1c7f98af', N'Bạn vui lòng cung cấp mã serial number và thông tin của sản phẩm để bên mình hỗ trợ tra cứu
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'4425ceb7-a705-44f3-a757-d2330823708c', N'68eeca50-d72e-4cff-8c9d-585d11b3dd48', N'HACOM cung cấp dịch vụ giao hàng miễn phí trong 100km quanh khu vực Hà Nội và TP.HCM
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'4a53d60d-fb49-4c24-b8b5-765e125ac11d', N'e633e720-0f59-4b16-9cbc-818f1c7f98af', NULL, 0, 1)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'4e1b7cc0-ddc5-4b5a-b310-bfa593e40c12', N'ac354e61-31bb-4572-8003-5ea6723d9938', N'Bạn hỏi cụ thể sản phẩm nào vậy ạ?
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'517a8de6-8034-4dbd-8eed-89b929fd01d8', N'd88a09f1-3b4f-4bd3-925b-ee6f8bc212c1', N'Dạ không biết là bạn đang quan tâm máy tầm giá bao nhiêu và nhu cầu chính là gì ạ?
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'530636bf-604b-44da-b361-f8a0f2545e28', N'a4c031c1-597b-4bae-9e77-63a4fe075a18', N'Bên mình hỗ trợ cài Windows bản quyền cho cá nhân và doanh nghiệp: https://hacom.vn/he-dieu-hanh
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'54beca15-c81e-4035-9325-47be9c3680f0', N'2c37992e-5f9e-4b34-8312-ec307c43cff9', N'Bên mình có hỗ trợ lắp đặt sản phẩm, giá dịch vụ tham khảo tại đây: https://hacom.vn/bang-gia-vat-tu-va-dich-vu-lap-dat-hacom
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'5ab8c50c-e8d4-4aab-a286-90ebca4218d0', N'445960fb-542d-408b-ab78-030df7b2cebb', N'Bên mình ko hỗ trợ đổi trả theo nhu cầu ạ!
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'5ad7da34-793c-45b4-9c95-2cd545e81522', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', N'Xin lỗi Quý khách! Hiện nhân viên tư vấn đang bận. Quý khách vui lòng chờ trong giây lát nhân viên tư vấn sẽ hỗ trợ ngay hoặc Quý khách vui lòng liên hệ tổng đài HACOM 19001903  để được phục vụ. Xin cảm ơn!
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'5af014e3-5367-4491-b528-481bc1c0673e', N'1c32b6bd-9be3-4ace-be3b-0ec7d3394130', N'Đây là danh sách sản phẩm bên mình đang quan tâm: https://hacom.vn/man-hinh-may-tinh
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'60fe945b-59df-4be6-a254-896cd48103cb', N'a7e219b4-b94c-46cb-91be-20b31a5a4b35', N'Bên mình giao hàng miễn phí trong ngày phạm vi nội thành Hà Nội, TP.HCM
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'67fa68a0-2436-479d-b0f6-daa85d0b3ca7', N'604935fe-5405-42d4-a322-7a15b3cb786c', N'Đây là danh sách sản phẩm bên mình đang quan tâm: https://hacom.vn/o-cung-ssd
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'6c422790-9b35-4c4d-b412-00e7e759cc31', N'068da8c4-cb5a-45f8-ab2c-24abc193a95c', N'test', 0, 1)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'752d971d-ea86-42fd-ab98-9d7363347a8a', N'81480b82-d938-40b5-870b-252b24e087be', N'Vui lòng truy cập vào website https://hacom.vn/quen-mat-khau và làm theo hướng dẫn để lấy lại mật khẩu đã mất
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'7b0bf4d4-41aa-44e4-a711-0c6b98ab8b19', N'445960fb-542d-408b-ab78-030df7b2cebb', N'HACOM hỗ trợ đổi trả sản phẩm trong 15 ngày đầu, chính sách và điều kiện đổi trả xem tại: https://hacom.vn/chinh-sach-bao-hanh
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'7d634935-40d1-4bc9-a0d4-e273edeae9a3', N'068da8c4-cb5a-45f8-ab2c-24abc193a95c', N'Đây là danh sách sản phẩm bên mình đang quan tâm: https://hacom.vn/camera-thiet-bi-an-ninh
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'7f095467-d464-4c45-aece-f50ca9788b4b', N'e01adc88-6cc5-403b-b45b-b0241c2047f7', N'Đây là danh sách sản phẩm bên mình đang quan tâm: https://hacom.vn/vo-case
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'808bdf7f-8b83-4695-93bd-11b1a7d87462', N'a94c27bf-8dcc-4054-8dec-37d57f37ec28', N'Những sản phầm laptop mua tại Hacom được vệ sinh miễn phí bạn nhé !
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'8182fb67-b68d-49cd-823c-80483d6b21bd', N'e7eb4752-f2a6-4184-8283-fce4b2529ec8', N'câu trả lời đúng nhất', 0, 1)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'830d73ff-6afe-4f92-8cf0-456f1482031e', N'8d3d7f5e-a099-4140-8975-b2b1cf3448a0', N'Không biết máy của bạn bị lỗi như thế nào ạ! Bạn miêu tả kĩ hơn được không?
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'8f3fb705-bf0b-4049-92c0-e4a9cd7944fb', N'8b0d2c60-282d-4b1b-9b7c-2a54b94e31fe', N'"Quý khách vui lòng cung cấp thông tin như sau :
1. Họ và tên
2. Địa chỉ
3. Số điện thoại di động
4. Thông tin viết VAT cho công ty ( nếu có ) + Email nhận hóa đơn điện tử"
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'9116785b-2228-4ae2-a5e6-659e4d86544a', N'b476fb4d-924a-41f8-9550-5611c0aa3afb', N'Đây là danh sách sản phẩm bên mình đang quan tâm: https://hacom.vn/ghe-gaming-ghe-choi-game
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'960c051d-e548-4f35-a49a-6aa267711171', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', N'Hiện tại các tư vấn viên đang nhận chat quá tải, quý khách vui lòng để lại số điện thoại , tư vấn viên sẽ liên hệ lại hoặc phản hồi chat trong giây lát, xin cám ơn Quý khách !
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'966a52ef-9df8-42b5-82ff-4fb8abe22050', N'9869cf7b-0719-43c6-a11f-e2dd05f81bb6', N'Đây là danh sách sản phẩm bên mình đang quan tâm: https://hacom.vn/linh-kien-may-tinh
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'97f6cf2e-bf3e-4e1f-b13e-c71d02b1fc1f', N'45c21929-d08d-4b22-ac40-5a2e49977819', N'Quý khách vui lòng truy cập vào website sau và nhập thông tin cá nhân để theo dõi đơn hàng: https://hacom.vn/tra-don-hang
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'9ba3a5bb-3c75-4c82-bb26-d6434d5393e8', N'e7eb4752-f2a6-4184-8283-fce4b2529ec8', N'Quý khách vui lòng chờ chút để tôi chuyển thông tin cho Kinh doanh tư vấn cho mình ạ
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'a6f30e6c-a587-4c59-9325-f4e781fb58b9', N'68eeca50-d72e-4cff-8c9d-585d11b3dd48', N'Bạn vui lòng cung cấp địa chỉ giao hàng và giờ giao hàng nhé!
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'abf65f7d-c622-46ff-b03f-fde6c0bd97ef', N'e633e720-0f59-4b16-9cbc-818f1c7f98af', N'HACOM chỉ nhận bảo hành các sản phẩm chưa qua sửa chữa, các hỏng hóc do quá trình sử dụng sẽ không được bảo hành, thông tin chi tiết xem tại: https://hacom.vn/chinh-sach-bao-hanh
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'adbbec93-7886-4322-ab60-e7b1ea2315ac', N'21af92c6-ed87-44d4-ad57-3cb31db89e45', N'Bên mình hỗ trợ lắp đặt SSD mới, còn SSD cũ thì tạm thời chưa hỗ trợ thu lại ạ!
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'b06a916e-ea17-48a5-b977-e4a71c15ae5a', N'a94c27bf-8dcc-4054-8dec-37d57f37ec28', N'Máy mang qua chi nhánh sẽ được vệ sinh miễn phí bạn nhé!', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'bb6c3360-410d-4508-92f2-6a2bbcf7cbfa', N'7ab2e703-016c-473f-8f4e-2f013e685635', N'
Mình sẽ kết nối tời BP Bán hàng để tư vấn hủy đơn cho bạn nhé !
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'beee0c35-75b5-4fdb-be70-32715134f42b', N'a94c27bf-8dcc-4054-8dec-37d57f37ec28', N'Bên mình có nhận vệ sinh laptop tại nhà và tại cửa hàng bạn nhé !
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'bf14024c-9072-4813-bfa7-9b8fb0b6f503', N'3f110730-26ec-49ea-bd37-b18a22c2e9f3', N'Đây là danh sách sản phẩm bên mình đang quan tâm: https://hacom.vn/vga-card-man-hinh
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'bf46a1eb-5740-4b65-8164-dd6bb23fab45', N'445960fb-542d-408b-ab78-030df7b2cebb', N'"Quý khách vui lòng lưu ý: sản phẩm bên em không nhận đổi trả nếu sản phẩm không lỗi, 1 đổi 1 trong vòng 15 ngày nếu sản phẩm lỗi, sau 15 ngày bảo hành chính hãng (Không áp dụng đổi mới với các sản phẩm: CPU, máy in, máy chiếu, máy photo, máy fax, Tivi, các sản phẩm của Apple, surface, hàng thanh lý...)
"
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'c1d12a91-250f-4048-81b7-d8b674262092', N'7ab2e703-016c-473f-8f4e-2f013e685635', N'Bạn cung cấp thông tin đơn đã tạo để Hacom hướng dẫn chi tiết bạn nhé !', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'c2331310-41f5-4870-927f-208ec41bdfc8', N'8d3d7f5e-a099-4140-8975-b2b1cf3448a0', N'Nếu máy bị treo, đơ, vui lòng bấm giữ nút nguồn cho đến khi máy tắt hẳn rồi tiến hành bật lại máy!
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'c651e504-b40b-4ab5-b301-45d73c30b9cd', N'1d96fd2e-a549-4928-ac2f-4dc438b7c110', N'Hãy cho tôi biết sản phẩm bạn muốn đặt và địa chỉ giao hàng ', 1, 1)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'c7af85f4-d75f-4f04-b4eb-f854b2a12325', N'2c37992e-5f9e-4b34-8312-ec307c43cff9', N'Bạn vui lòng để lại địa chỉ nhận hàng ạ!
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'cee8b9e9-0ffe-4b7d-aeb6-c917b1825575', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', N'Chào bạn, mình đã sẵn sàng tư vấn ! Mình giúp được gì cho bạn ạ!
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'd5e68604-6265-485f-870f-146c552dc994', N'f3cb8dc5-4b04-4ee5-b6b7-9587a47963a9', N'Đây là danh sách sản phẩm bên mình đang quan tâm: https://hacom.vn/tai-nghe
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'd7e9e3f9-cfdd-4fcd-8fb0-0ff288a7fcc3', N'e633e720-0f59-4b16-9cbc-818f1c7f98af', N'bạn hãy nhấn nút Thêm mới', 0, 1)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'e02712af-8349-460f-96da-02c04a13b4ff', N'a6926fed-f1ae-48ed-b08f-40530f5ee517', N'Cảm ơn quý khách đã quan tâm và sử dụng dịch vụ bên HACOM. Chúc quý khách luôn mạnh khỏe và thành công!
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'ece5dd72-78ef-4920-a29e-a60c24c9d81b', N'a7e219b4-b94c-46cb-91be-20b31a5a4b35', N'Bên mình hỗ trợ giao hàng toàn quốc, thời gian từ 1 -3 ngày tùy địa điểm, vui lòng để lại địa chỉ để mình đưa ra thông tin chính xác hơn
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'f099277e-9274-4a9d-9f3e-b91d6d42c831', N'445960fb-542d-408b-ab78-030df7b2cebb', N'Dạ đổi trả theo nhu cầu sẽ tính phí ạ!
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'f2397c64-9b02-4232-a02b-843ce9ca151f', N'ee67af25-a9f2-4f6f-b546-84f1ac1958d2', N'Đây là danh sách sản phẩm bên mình đang quan tâm: https://hacom.vn/may-choi-game-tay-game
', 1, 0)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'f297b201-1c1c-4970-9ac0-b0fcb7cdc0c8', N'e633e720-0f59-4b16-9cbc-818f1c7f98af', N'haha ok k yh', 0, 1)
INSERT [dbo].[Answers] ([Id], [CategoryID], [AnswerVN], [Inactive], [OnDelete]) VALUES (N'f98318ca-dbf8-494f-9346-94a458517b02', N'fe9a332d-36a2-4f7d-830e-552567206b74', N'Đây là danh sách sản phẩm bên mình đang quan tâm: https://hacom.vn/ram-bo-nho-trong
', 1, 0)
GO
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'068da8c4-cb5a-45f8-ab2c-24abc193a95c', N'ORDER', N'san_pham_camera', N'san_pham_camera', N'Sản phẩm camera', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'07fe61e4-0bfe-4272-8057-af285bed59bc', N'ORDER', N'dk_dich_vu', N'dk_dich_vu', N'Đăng ký dịch vụ', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'129eab80-b7a9-436b-a62c-7e4dd4d94f6c', N'ORDER', N'chuyen_doi_tk', N'chuyen_doi_tk', N'Đổi tài khoản', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'1c32b6bd-9be3-4ace-be3b-0ec7d3394130', N'ORDER', N'san_pham_man_hinh', N'san_pham_man_hinh', N'Sản phẩm màn hình', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'1d96fd2e-a549-4928-ac2f-4dc438b7c110', N'ORDER', N'dat_hang', N'dat_hang', N'Đặt hàng', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'1e876105-f399-4f55-b20c-3c823f67e060', N'ORDER', N'phi_giao_hang', N'phi_giao_hang', N'Phí giao hàng', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'2161959b-3c95-443b-8c07-421df40cf7ca', N'ORDER', N'kt_quy_dinh_tra_lai_tien', N'kt_quy_dinh_tra_lai_tien', N'Kiểm tra quy định trả lại tiền', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'21af92c6-ed87-44d4-ad57-3cb31db89e45', N'ORDER', N'mua_lai_ssd_cu', N'mua_lai_ssd_cu', N'Khách muốn bán lại SSD đã qua sử dụng', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'2a5ca111-833a-493d-bd0d-269ba338af96', N'ORDER', N'lay_tien_tra_lai', N'lay_tien_tra_lai', N'Lấy tiền trả lại', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'2c37992e-5f9e-4b34-8312-ec307c43cff9', N'ORDER', N'lap_dat', N'lap_dat', N'Thắc mắc về lắp đặt sản phẩm', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'34ea6fe4-e882-403f-bf16-d54eb7c91a47', N'ORDER', N'ho_tro_khac', N'ho_tro_khac', N'Hỗ trợ khác', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'3880470e-e817-49d4-a6ff-9008d18a2111', N'ORDER', N'chon_ht_giao_van', N'chon_ht_giao_van', N'Chọn hình thức giao vận', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', N'ORDER', N'kiem_tra', N'kiem_tra', N'Kiểm tra kỹ thuật', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'3f110730-26ec-49ea-bd37-b18a22c2e9f3', N'ORDER', N'san_pham_card', N'san_pham_card', N'Sản phẩm card', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'445960fb-542d-408b-ab78-030df7b2cebb', N'ORDER', N'doi_tra', N'doi_tra', N'Đổi, trả hàng', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'45c21929-d08d-4b22-ac40-5a2e49977819', N'ORDER', N'thoi_doi_don', N'thoi_doi_don', N'Theo dõi đơn', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'45df31ee-07e8-4fa1-accb-80caf3ca53c5', N'ORDER', N'in_phieu_tt', N'in_phieu_tt', N'In phiếu thanh toán', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'5591a211-93c5-493a-8a57-2dd6467fd529', N'ORDER', N'san_pham_webcam', N'san_pham_webcam', N'Sản phẩm webcam', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'58e75b49-7d73-4d65-b995-7ffb8919577b', N'ORDER', N'showroom', N'showroom', N'Showroom', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'5f52f46b-949a-4ba2-a275-85980e3fd7b3', N'ORDER', N'phan_mem', N'phan_mem', N'Phần mềm', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'604935fe-5405-42d4-a322-7a15b3cb786c', N'ORDER', N'san_pham_o_cung', N'san_pham_o_cung', N'Sản phẩm ổ cứng', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'68eeca50-d72e-4cff-8c9d-585d11b3dd48', N'ORDER', N'dia_chi_ship', N'dia_chi_ship', N'Ghi địa chỉ ship', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'7ab2e703-016c-473f-8f4e-2f013e685635', N'ORDER', N'huy_hd', N'huy_hd', N'Hủy đơn', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'7bdf04f9-4e4e-43d2-bcdd-dd10859af5af', N'ORDER', N'khieu_nai', N'khieu_nai', N'Khiếu nại', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'80b67125-7032-41a5-a24f-303d81c5ad61', N'ORDER', N'a', N'a', N'a', NULL, 0, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'81480b82-d938-40b5-870b-252b24e087be', N'ORDER', N'dat_lai_mk', N'dat_lai_mk', N'Lấy lại mật khẩu', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'8b0d2c60-282d-4b1b-9b7c-2a54b94e31fe', N'ORDER', N'lay_hoa_don', N'lay_hoa_don', N'Lấy hóa đơn', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'8c4f786b-ddce-4805-ba6b-716191381043', N'ORDER', N'kt_phuong_thuc_tt', N'kt_phuong_thuc_tt', N'Kiểm tra phương thức thanh toán', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'8d3d7f5e-a099-4140-8975-b2b1cf3448a0', N'ORDER', N'bao_loi', N'bao_loi', N'Báo lỗi sản phẩm', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'90b8802e-d76b-44ef-9e97-2287ac5688ef', N'ORDER', N'san_pham_laptop', N'san_pham_laptop', N'Sản phẩm laptop', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'942265ea-5c6e-4ed4-8967-daa07ccc9ce1', N'ORDER', N'tao_tk', N'tao_tk', N'Tạo tài khoản', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'95016d24-a071-4755-b502-b95347a2cc74', N'ORDER', N'change_order', N'thay_doi_don', N'Thay đổi đơn', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'963b7f92-731a-4abb-9eb6-1426b811e81b', N'ORDER', N'san_pham_PC', N'san_pham_PC', N'Sản phẩm PC', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'9869cf7b-0719-43c6-a11f-e2dd05f81bb6', N'ORDER', N'san_pham_linh_kien', N'san_pham_linh_kien', N'Sản phẩm linh kiện', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'9b0d1707-56f5-4c53-b4b7-762bdc40090e', N'ORDER', N'ho_tro_ky_thuat', N'ho_tro_ky_thuat', N'Hỗ trợ kỹ thuật', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'9be36644-177f-4bfd-aea8-c2c4bcf2dc25', N'ORDER', N'kt_phi_huy', N'kt_phi_huy', N'Kiểm tra phí hủy', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'a4c031c1-597b-4bae-9e77-63a4fe075a18', N'ORDER', N'phan_mem_windows', N'phan_mem_windows', N'Phần mềm window', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'a6926fed-f1ae-48ed-b08f-40530f5ee517', N'ORDER', N'goodbye', N'goodbye', N'Goodbye', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'a71d1c34-bf04-4485-9f63-83f51f441f17', N'ORDER', N'edit_account', N'chinh_tk', N'Cập nhật tài khoản', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'a7e219b4-b94c-46cb-91be-20b31a5a4b35', N'ORDER', N'thoi_gian_giao_van', N'thoi_gian_giao_van', N'Thời gian giao hàng', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'a94c27bf-8dcc-4054-8dec-37d57f37ec28', N'ORDER', N've_sinh', N've_sinh', N'Vệ sinh', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'ac354e61-31bb-4572-8003-5ea6723d9938', N'ORDER', N'tm_san_pham', N'tm_san_pham', N'Thắc mắc về sản phẩm', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'b476fb4d-924a-41f8-9550-5611c0aa3afb', N'ORDER', N'san_pham_ghe', N'san_pham_ghe', N'Sản phẩm ghế', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'b8a64706-d05f-4714-9b01-54c339efe075', N'ORDER', N'sua_chua', N'sua_chua', N'Sửa chữa', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'c10288f5-9462-47fc-8941-9c593c686259', N'ORDER', N'thay_doi_dc_ship', N'thay_doi_dc_ship', N'Thay đổi địa chỉ ship', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'c18fffc0-aae1-4cba-942b-59912a78f2f9', N'ORDER', N'contact_cskh', N'contact_cskh', N'Liên hệ CSKH', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'c1da0780-071a-4bfb-a836-655cc9456c8d', N'ORDER', N'xoa_tk', N'xoa_tk', N'Xóa tài khoản', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'c8142474-775d-45f0-887f-2616047dcccd', N'ORDER', N'san_pham_wifi', N'san_pham_wifi', N'Sản phẩm wifi', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'd4da111f-f245-44ea-9ee3-c32a7319d2e1', N'ORDER', N'b', N'b', N'b', NULL, 0, 1)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'd6440c70-55ce-4258-a7bd-a26dfe6d4439', N'ORDER', N'ghi_nhan_vd', N'ghi_nhan_vd', N'Ghi nhận vấn đề', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'd88a09f1-3b4f-4bd3-925b-ee6f8bc212c1', N'ORDER', N'san_pham', N'san_pham', N'Sản phẩm', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'd90e36be-e6e3-407d-b58b-2fb8600bfe5c', N'ORDER', N'san_pham_chuot', N'san_pham_chuot', N'Sản phẩm chuột', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'dc2953f1-520b-4f0e-8161-55338b2957b6', N'ORDER', N'thoi_doi_tra_tien', N'thoi_doi_tra_tien', N'Theo dõi trả lại tiền', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'e01adc88-6cc5-403b-b45b-b0241c2047f7', N'ORDER', N'san_pham_case', N'san_pham_case', N'Sản phẩm case', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'e34e212b-be06-4f28-8260-8eb813f6b3e1', N'ORDER', N'tra_gop', N'tra_gop', N'Trả góp sản phẩm', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'e633e720-0f59-4b16-9cbc-818f1c7f98af', N'ORDER', N'bao_hanh', N'bao_hanh', N'Kiểm tra thông tin bảo hành', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'e7eb4752-f2a6-4184-8283-fce4b2529ec8', N'ORDER', N'contact_nvkd', N'contact_nvkd', N'Liên hệ NVKD', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'ee67af25-a9f2-4f6f-b546-84f1ac1958d2', N'ORDER', N'san_pham_game', N'san_pham_game', N'Sản phẩm game', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'f3cb8dc5-4b04-4ee5-b6b7-9587a47963a9', N'ORDER', N'san_pham_tai_nghe', N'san_pham_tai_nghe', N'Sản phẩm tai nghe', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'fe9a332d-36a2-4f7d-830e-552567206b74', N'ORDER', N'san_pham_ram', N'san_pham_ram', N'Sản phẩm ram', NULL, 1, 0)
INSERT [dbo].[Categories] ([Id], [NameCategory], [IntentCodeEN], [IntentCodeVN], [Description], [ParentID], [Inactive], [OnDelete]) VALUES (N'ffe7de9c-e158-4973-ab13-f454bb53d40c', N'ORDER', N'kt_hoa_don', N'kt_hoa_don', N'Kểm tra hóa đơn', NULL, 1, 0)
GO
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'017cabca-3a81-45e1-9939-1d335a354468', N'58e75b49-7d73-4d65-b995-7ffb8919577b', NULL, N'Chi nhánh Nguyễn Văn Cừ
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'049e61b5-3003-4692-a3a6-ccf9ff27e2c1', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Xin chào admin
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'04fbed0a-103a-4ee8-b82a-601f066f616f', N'1c32b6bd-9be3-4ace-be3b-0ec7d3394130', NULL, N'Màn hình ViewSonic VA2430-H-W-6
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'052b3dec-1c98-4d9c-8b87-1c27c08df0a3', N'34ea6fe4-e882-403f-bf16-d54eb7c91a47', NULL, N'Trả trước 20% số tiền của máy
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'0796608c-23b5-4b9c-b7d8-d68025c00c92', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', NULL, N'Laptop của tôi báo lỗi rồi tắt màn hình nhưng quạt vẫn chạy
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'0ae7346d-bc0c-4689-8c7f-801dd0726df1', N'e34e212b-be06-4f28-8260-8eb813f6b3e1', NULL, N'Trả góp theo phương thức nào khác không
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'0b498a1b-2711-4a7b-96c7-8c96dbed9f02', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', NULL, N'Cho mình hỏi tầm 10tr thì cấu hình có CPU, main, ram, nguồn, vỏ case thì nên lấy những
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'0c5176b3-4667-4a60-b4eb-43b981ca0b17', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'ALO
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'125b2d4e-90ae-4b58-8ac5-4e51a90683a9', N'90b8802e-d76b-44ef-9e97-2287ac5688ef', NULL, N'Dell M4800
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'179d0136-dfc9-48e9-929a-da0b094a71bf', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Tư vấn thêm cho tôi.
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'186f99e2-4f73-4ac1-af1d-d7091ae08937', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Chào bạn
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'19347aad-2060-4f37-a3bb-ea73c38a4fd1', N'90b8802e-d76b-44ef-9e97-2287ac5688ef', NULL, N'Laptop Asus VivoBook A515EA-BQ491T
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'1b014c70-1e52-457c-b52a-9e06ca850deb', N'e633e720-0f59-4b16-9cbc-818f1c7f98af', NULL, N'Hết bảo hành và lại bị hỏng tấm nền
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'1b8e12ff-067a-4b57-b78a-18a21dfdf44d', N'e34e212b-be06-4f28-8260-8eb813f6b3e1', NULL, N'Minh chứng thu nhập
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'1c22f16f-3f58-4ab2-b3bc-6b5e4d14aa16', N'604935fe-5405-42d4-a322-7a15b3cb786c', NULL, N'Ổ cứng di động
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'1ff01129-6ec8-43ca-9de1-1111eef42f3a', N'2c37992e-5f9e-4b34-8312-ec307c43cff9', NULL, N'Có tính phí lắp đặt không
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'215d7b74-c0c8-4664-bd71-a703e627c08c', N'7ab2e703-016c-473f-8f4e-2f013e685635', NULL, N'làm thế nào tôi có thể hủy đơn hàng cuối cùng tôi đã tạo
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'23698f87-d6e3-43d7-86ff-a3fd8d255705', N'7ab2e703-016c-473f-8f4e-2f013e685635', NULL, N'tôi muốn hướng dẫn để hủy đơn
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'23ca65b5-5fbc-4175-bc41-b8e6dd903ee7', N'68eeca50-d72e-4cff-8c9d-585d11b3dd48', NULL, N'Lấy tại cửa hàng không ship
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'2414aa14-1aeb-472a-8b8e-d0d21080b4b8', N'ee67af25-a9f2-4f6f-b546-84f1ac1958d2', NULL, N'Tay cầm
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'252fd0f0-b036-4676-b4e1-fb0c70c0495f', N'd90e36be-e6e3-407d-b58b-2fb8600bfe5c', NULL, N'Tôi muốn mua chuột Fulen G90
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'289be217-2e2e-4c9c-b021-d1cc4f10df07', N'a7e219b4-b94c-46cb-91be-20b31a5a4b35', NULL, N'Thời gian giao hàng là bao lâu
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'2c288258-3acb-4178-9784-0c6bcae34fb7', N'7ab2e703-016c-473f-8f4e-2f013e685635', NULL, N'làm thế nào để tôi hủy đơn mua hàng của tôi', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'2c7d1a25-28ee-4b75-924c-f0b4529ce02c', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Chào bạn
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'2f16d243-7e8a-476d-a13e-0255a816e6a3', N'e34e212b-be06-4f28-8260-8eb813f6b3e1', NULL, N'Có trả góp được không 
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'2f9e9fcb-ac83-4ab3-a1f2-1cf4e43fdc92', N'7ab2e703-016c-473f-8f4e-2f013e685635', NULL, N'tôi cố gắng hủy 1 đơn
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'35247c09-e436-4166-9219-39606df7ae5f', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', NULL, N'Tư vấn lắp thêm ram
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'35c99752-b7f4-458a-8132-2ec06b0aa255', N'e633e720-0f59-4b16-9cbc-818f1c7f98af', NULL, N'Chương trình bán sản phẩm mới và vẫn bảo hành đầy đủ đúng không?
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'38c5f4c8-7ccf-4a65-90f8-6f4051b1623e', N'68eeca50-d72e-4cff-8c9d-585d11b3dd48', NULL, N'Địa chỉ của tôi
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'3a05a041-8657-48ac-975d-ffb3d79118a5', N'f3cb8dc5-4b04-4ee5-b6b7-9587a47963a9', NULL, N'Tai nghe E-Dra EH410 Pro USB Led RGB
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'3dac8f90-d222-41f3-99ab-408d2f6aac4f', N'9869cf7b-0719-43c6-a11f-e2dd05f81bb6', NULL, N'PC - Laptop và linh kiện
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'3f78c6a3-e36f-49d4-bef5-4bdd968f4cc6', N'963b7f92-731a-4abb-9eb6-1426b811e81b', NULL, N'Tôi cần máy mini chạy ít tốn điện nhất
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'3fd090ef-106a-46f5-9e0e-7a7326bb0552', N'a4c031c1-597b-4bae-9e77-63a4fe075a18', NULL, N'Windows bản quyền
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'40f7834b-0fcf-4fa1-9af8-8c31321c9b36', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Bạn cho mình hỏi chút với
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'41c9efc7-58e1-4dfe-84c2-3bdd396e38a9', N'f3cb8dc5-4b04-4ee5-b6b7-9587a47963a9', NULL, N'Tai nghe Kingston HyperX CLOUD STINGER CORE 7.1 Black - HHSS1C-AA-BK/G
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'46be2b07-5776-4291-88af-560706c1085d', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Còn sản phẩm này không
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'4901e964-d7c6-4030-9d06-a244f235b2b7', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Shop ơi cho em hỏi
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'4fa0e579-5231-4a22-9e33-910b15f49568', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', NULL, N'Hacom sẽ thu lại ssd của tôi và lắp một chiếc ssd khác tốt hơn phải không
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'507136f3-8919-4c14-befe-dc3fdd6665ba', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Dạ mình cảm ơn ạ
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'548bfcb6-9bfe-45f2-b09b-392e8b34df3d', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', NULL, N'Máy tính tôi bị lỗi
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'56e2451c-d45b-4667-8538-63c5d4e271a7', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Gửi đơn qua email
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'59d7d68f-4fbd-4c87-bf2a-bc344142eefc', N'b476fb4d-924a-41f8-9550-5611c0aa3afb', NULL, N'Ghế gamer E-Dra Queen - EGC225
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'5c625add-4560-4472-97b3-09ced024e14d', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Đặt cọc sản phẩm
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'5d087306-b8d1-4b00-a80e-86fa60379e1f', N'34ea6fe4-e882-403f-bf16-d54eb7c91a47', NULL, N'Tra cứu giao hàng/ Hỗ trợ khác
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'60e9ce1a-bdf1-4ace-b4ef-09dad637da5c', N'e633e720-0f59-4b16-9cbc-818f1c7f98af', NULL, N'Bảo hành bao lâu
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'65ab34c5-b6ce-47b8-b5b8-31cef6ba5fea', N'1c32b6bd-9be3-4ace-be3b-0ec7d3394130', NULL, N'https://hacom.vn/man-hinh-lg-24qp750-b
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'6657e556-bccd-4edd-a328-6687fa80c326', N'7ab2e703-016c-473f-8f4e-2f013e685635', NULL, N'tôi muốn hủy đơn tôi vừa tạo', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'670e6c2a-3efd-4159-82f9-02a5da0c40f7', N'fe9a332d-36a2-4f7d-830e-552567206b74', NULL, N'Ram Desktop Adata XPG Spectrix D50 RGB
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'67b51553-3976-4ebb-9f5b-52bdca8eaeb5', N'a6926fed-f1ae-48ed-b08f-40530f5ee517', NULL, N'Cảm ơn
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'6c64377b-7a57-4d52-9e56-7433eb96c79f', N'e01adc88-6cc5-403b-b45b-b0241c2047f7', NULL, N'Vỏ Case Vitra POSEIDON G5 (Mid Tower / Màu Đen)
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'70274d6d-cf2f-4011-a707-f1fbcdac12d7', N'58e75b49-7d73-4d65-b995-7ffb8919577b', NULL, N'Hacom Phủ Lý
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'70c52335-b275-41fe-a6cf-49482ff47151', N'963b7f92-731a-4abb-9eb6-1426b811e81b', NULL, N'Máy tính INTEL NUC7CJYH Celeron J4005
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'70cdf69a-3e04-449f-b59d-c2b7f797f402', N'1e876105-f399-4f55-b20c-3c823f67e060', NULL, N'vậy tôi thanh toán hết là được miễn ship đúng không
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'73269f4a-71e6-4907-a13d-2401d3ca1ee5', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Tư vấn bán hàng
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'7336fce7-e039-456f-8886-74c13683aae2', N'604935fe-5405-42d4-a322-7a15b3cb786c', NULL, N'Ổ cứng msata ssd
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'75b1e712-b3cc-4071-8758-d5b4c99ecea9', N'90b8802e-d76b-44ef-9e97-2287ac5688ef', NULL, N'Laptop Acer Aspire 5 A515
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'760b50c9-cd65-456d-9fac-eaf3c1819177', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', NULL, N'Thay tấm nền mất bao lâu và chi phí như thế nào
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'7e675654-99f1-48c6-abe5-81f3264988b4', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Chào
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'7f65736e-29db-4021-a10b-8f14d9d42e23', N'68eeca50-d72e-4cff-8c9d-585d11b3dd48', NULL, N'Ship tận nhà tại Hà Nội có miễn phí không
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'8060313a-a86b-4847-9b69-161a5c106a38', N'5f52f46b-949a-4ba2-a275-85980e3fd7b3', NULL, N'Bây giờ tôi đang muốn dùng microsoft
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'811b3275-fe28-4a30-88d8-1adc7a3230ab', N'90b8802e-d76b-44ef-9e97-2287ac5688ef', NULL, N'laptop MSI Modern 14 core i3 1115G4
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'829ab3d2-ec88-4030-82e8-fbcb4e6abb73', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Hỗ trợ Kỹ thuật
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'8a09648a-d321-4b08-b29a-4212bc5e5a93', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', NULL, N'Hỗ trợ Kỹ thuật
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'8f3b0084-bff5-45a9-b9e4-09b76e4fa85d', N'1c32b6bd-9be3-4ace-be3b-0ec7d3394130', NULL, N'MÀN HÌNH ASUS PRO ART PA248QV (24INCH/WUXGA/IPS/75HZ/5MS/300NITS/HDMI
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'8f94915a-9bb7-42dc-b9d0-71edbb44ecd9', N'963b7f92-731a-4abb-9eb6-1426b811e81b', NULL, N'Máy tinh mini đơn giản nhất
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'99c176ee-d8ea-4d49-bb17-1ca6a85e2995', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', NULL, N'Tôi đang dùng thì nó báo lỗi rồi nó tắt màn hình
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'9bf8637f-c492-4458-8f8f-29d45a9665f4', N'd90e36be-e6e3-407d-b58b-2fb8600bfe5c', NULL, N'Chuột laptop
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'9bfd5702-d5bc-4e8f-84e9-c91b597a0df3', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', NULL, N'Cài đặt lại cổng kết nối
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'9d263d49-98ed-4775-8afc-f85b00396955', N'604935fe-5405-42d4-a322-7a15b3cb786c', NULL, N'Mình muốn hỏi ổ cứng ngoài cho macbook
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'9e23f49a-10e0-48eb-ac47-e7027c92a3da', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Tôi muốn mua laptop tư vấn cho tôi', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'9e5ef71c-7a02-456f-966c-ec4f756428c1', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', NULL, N'Ấn nút nguồn mãi máy mới lên
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'9ec25d76-85a4-4f09-b6ff-1d679f302406', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Tư vấn bán hàng
', 0, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'9f29ff63-7369-4940-a415-99e0ab705e8f', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', NULL, N'Vừa sạc vừa dùng máy tính
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'a0bd75d9-82d5-4cfb-a6b4-7ed5358ec69b', N'58e75b49-7d73-4d65-b995-7ffb8919577b', NULL, N'Chi nhánh Hai Bà Trưng
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'a4975473-391d-4b16-870b-38037a17849a', N'ee67af25-a9f2-4f6f-b546-84f1ac1958d2', NULL, N'Vô lăng PXN V900
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'a8a28b6e-ae79-4b04-938b-7c52d6b3591f', N'68eeca50-d72e-4cff-8c9d-585d11b3dd48', NULL, N'Tôi mong muốn có hàng trong sáng mai
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'a958bec7-fa49-43e8-9c79-6e2565fffeff', N'a94c27bf-8dcc-4054-8dec-37d57f37ec28', NULL, N'Chương trình vệ sinh máy miễn phí
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'afef3bfe-3ef8-484c-ac05-0a7b2b9af15b', N'1c32b6bd-9be3-4ace-be3b-0ec7d3394130', NULL, N'Màn hình LG UltraGear 27GL850-B', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'b058541d-afba-4751-8fdf-4334582334bd', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', NULL, N'Bên mình có sửa bàn phím cơ không
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'b0fc136a-935a-487b-a02f-6665ca9c68a6', N'5591a211-93c5-493a-8a57-2dd6467fd529', NULL, N'Webcam
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'b38e1ae3-266b-4eda-b3dc-08b952203f77', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', NULL, N'Bị hỏng tấm nền
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'baf8cb0d-5354-4b8b-a255-b54a0352fe79', N'2c37992e-5f9e-4b34-8312-ec307c43cff9', NULL, N'Hàng về có được xem lắp đặt không
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'c1fa0b1a-1f7e-4308-82c4-c1009e0a6205', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Laptop của tôi mua ở bên Hacom đang gặp chút vấn đề 
', 0, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'c59e944a-a0ef-4486-965b-227fc8dda5c2', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Để tôi tìm sản phẩm khác
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'c7cd9251-84e1-4333-8ae8-09bf00615b47', N'5f52f46b-949a-4ba2-a275-85980e3fd7b3', NULL, N'Bản cài sẵn trong máy bắt buộc mình phải có tài khoản microsoft
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'c7d44cde-4f20-47d1-a205-71eea7cec291', N'ee67af25-a9f2-4f6f-b546-84f1ac1958d2', NULL, N'Xbox series x
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'cc8c1a70-a25f-4e44-b988-5290a9837167', N'90b8802e-d76b-44ef-9e97-2287ac5688ef', NULL, N'Asus UX581
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'cd3c1082-d630-48f2-b695-12594ebbc313', N'34ea6fe4-e882-403f-bf16-d54eb7c91a47', NULL, N'Hỗ trợ khác
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'cd9fc2bc-87a4-47ae-a849-4f3e2510181a', N'604935fe-5405-42d4-a322-7a15b3cb786c', NULL, N'Ổ cứng SSD 512
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'ce71347c-8fcd-476c-bb8d-cdee25c6c7ef', N'c8142474-775d-45f0-887f-2616047dcccd', NULL, N'Wifi mesh
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'cf6bec80-023c-4276-b79e-5befd23632f5', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Thiết bị văn phòng
', 0, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'cf6dbeaf-9865-4e63-96d0-d7f69d85ebc8', N'2c37992e-5f9e-4b34-8312-ec307c43cff9', NULL, N'Bên Hacom có hỗ trợ lắp từ máy cũ sang case mới không
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'd160fc4d-c4b9-461e-a652-538a1da3bfca', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Cảm ơn
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'd171b7c6-b8e6-431b-a09c-a893ad9a84b9', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Còn hàng không
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'd2bdb3a6-b641-4efb-a9ed-6784aa97e18e', N'e633e720-0f59-4b16-9cbc-818f1c7f98af', NULL, N'Kiểm tra thông tin bảo hành
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'd40ee149-191d-408a-ad28-f8b2feccc443', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', NULL, N'Thay tản nhiệt khi thành tản nhiệt nước được không
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'd644c9ee-a874-47a5-a181-caa192319807', N'90b8802e-d76b-44ef-9e97-2287ac5688ef', NULL, N'Ổ CỨNG DI ĐỘNG HIKVISION SSD 256GB USB3.1,TYPEC HS-ESSD-P0256BWD MÀU ĐEN
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'dacb09cb-b3f0-4452-b445-584d70a27af0', N'a6926fed-f1ae-48ed-b08f-40530f5ee517', NULL, N'Chúc bạn một buổi tối vui vẻ
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'ddf44007-072c-4111-b225-930da4842f9d', N'a94c27bf-8dcc-4054-8dec-37d57f37ec28', NULL, N'Vệ sinh máy tính với thay nước tản nhiệt tại nhà
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'df4cf4de-e64c-4655-b50e-599f16e26820', N'e34e212b-be06-4f28-8260-8eb813f6b3e1', NULL, N'Có nhu cầu trả góp
', 1, 0)
GO
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'e0626977-d71c-4327-bd0e-2ca726f299d9', N'3f110730-26ec-49ea-bd37-b18a22c2e9f3', NULL, N'Card màn hình', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'e075f46d-429d-4e8a-b8c5-0e4f3cb18144', N'e34e212b-be06-4f28-8260-8eb813f6b3e1', NULL, N'Trả góp qua thẻ visa
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'e1fe26f8-6960-4285-abaf-39c4cd537372', N'90b8802e-d76b-44ef-9e97-2287ac5688ef', NULL, N'Laptop MSI Gaming Katana GF66 
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'e37d30e4-1bb0-452c-ad11-1c3ec8bf170d', N'e633e720-0f59-4b16-9cbc-818f1c7f98af', NULL, N'Thời hạn bảo hành còn không
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'e8dea811-ac38-4296-b652-c768cb45bd58', N'c8142474-775d-45f0-887f-2616047dcccd', NULL, N'Moden nào vừa cắm trực tiếp vừa phát wifi
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'ed1c1276-7bac-45ae-8818-b0176a8e0c4e', N'90b8802e-d76b-44ef-9e97-2287ac5688ef', NULL, N'Lenovo IdeaPad 3 14ALC6
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'edd8a83a-2c5c-438e-8cb9-e5338f119035', N'c18fffc0-aae1-4cba-942b-59912a78f2f9', NULL, N'Liên hệ qua zalo, tôi cần tư vấn sản phẩm
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'f2ac1d6d-cc5b-498e-8d64-1b1498bcf788', N'445960fb-542d-408b-ab78-030df7b2cebb', NULL, N'Hỗ trợ đổi hàng
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'fb10d83d-5bab-4a2b-b70c-57648ac8ccb9', N'68eeca50-d72e-4cff-8c9d-585d11b3dd48', NULL, N'Nếu mua sản phẩm có đc hỗ trợ ship không
', 1, 0)
INSERT [dbo].[Intents] ([Id], [CategoryID], [IntentEN], [IntentVN], [Inactive], [OnDelete]) VALUES (N'fd666915-b3e2-48c7-a885-4ee18951e930', N'3df3217b-70cf-47ce-a4b6-27d9d9cfe131', NULL, N'Dịch vụ nâng cấp ssd
', 1, 0)
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Categori__4A7106400642077A]    Script Date: 09/30/2023 7:39:25 PM ******/
ALTER TABLE [dbo].[Categories] ADD UNIQUE NONCLUSTERED 
(
	[IntentCodeEN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Categori__4A769C3515B6B10C]    Script Date: 09/30/2023 7:39:25 PM ******/
ALTER TABLE [dbo].[Categories] ADD UNIQUE NONCLUSTERED 
(
	[IntentCodeVN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Answers]  WITH CHECK ADD  CONSTRAINT [fk_c] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([Id])
GO
ALTER TABLE [dbo].[Answers] CHECK CONSTRAINT [fk_c]
GO
ALTER TABLE [dbo].[Intents]  WITH CHECK ADD  CONSTRAINT [fk_cate] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([Id])
GO
ALTER TABLE [dbo].[Intents] CHECK CONSTRAINT [fk_cate]
GO
