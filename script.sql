USE [TravelAgencyDb]
GO
/****** Object:  Table [dbo].[Bookings]    Script Date: 1/31/2025 8:51:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Bookings](
	[id] [varchar](36) NOT NULL,
	[user_id] [varchar](36) NOT NULL,
	[travel_package_id] [varchar](36) NOT NULL,
	[number_of_travelers] [int] NOT NULL,
	[total_price] [decimal](10, 2) NOT NULL,
	[booking_date] [datetime] NOT NULL,
	[status] [varchar](15) NOT NULL,
 CONSTRAINT [PK__Bookings__3213E83F4782F263] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 1/31/2025 8:51:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[id] [varchar](36) NOT NULL,
	[user_id] [varchar](36) NOT NULL,
	[booking_id] [varchar](36) NOT NULL,
	[amount] [decimal](10, 2) NOT NULL,
	[payment_date] [datetime] NOT NULL,
	[payment_status] [varchar](36) NOT NULL,
 CONSTRAINT [PK__Payments__3213E83FC912D91D] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Travelers]    Script Date: 1/31/2025 8:51:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Travelers](
	[id] [varchar](36) NOT NULL,
	[booking_id] [varchar](36) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[age] [int] NOT NULL,
	[gender] [varchar](10) NOT NULL,
 CONSTRAINT [PK__Traveler__3213E83FEAC31D9B] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TravelPackages]    Script Date: 1/31/2025 8:51:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TravelPackages](
	[id] [varchar](36) NOT NULL,
	[title] [varchar](255) NOT NULL,
	[destination] [varchar](100) NOT NULL,
	[description] [text] NULL,
	[price] [decimal](10, 2) NOT NULL,
	[start_date] [datetime] NOT NULL,
	[end_date] [datetime] NOT NULL,
	[inclusions] [text] NULL,
	[cancellation_policy] [text] NULL,
	[status] [varchar](30) NOT NULL,
 CONSTRAINT [PK__TravelPa__3213E83F375A2FB2] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 1/31/2025 8:51:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [varchar](36) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[email] [varchar](100) NOT NULL,
	[password_hash] [varchar](255) NOT NULL,
	[phone] [varchar](15) NOT NULL,
	[role] [varchar](15) NOT NULL,
 CONSTRAINT [PK__Users__3213E83FB677C58E] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Users__AB6E61640802EF5A] UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Bookings] ADD  CONSTRAINT [DF__Bookings__bookin__3F466844]  DEFAULT (getdate()) FOR [booking_date]
GO
ALTER TABLE [dbo].[Bookings] ADD  CONSTRAINT [DF__Bookings__status__403A8C7D]  DEFAULT ('pending') FOR [status]
GO
ALTER TABLE [dbo].[Payments] ADD  CONSTRAINT [DF__Payments__paymen__44FF419A]  DEFAULT (getdate()) FOR [payment_date]
GO
ALTER TABLE [dbo].[Payments] ADD  CONSTRAINT [DF__Payments__paymen__45F365D3]  DEFAULT ('pending') FOR [payment_status]
GO
ALTER TABLE [dbo].[Travelers] ADD  CONSTRAINT [DF_Travelers_gender]  DEFAULT ('male') FOR [gender]
GO
ALTER TABLE [dbo].[TravelPackages] ADD  CONSTRAINT [DF_TravelPackages_status]  DEFAULT ('activate') FOR [status]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__Users__role__3A81B327]  DEFAULT ('customer') FOR [role]
GO
