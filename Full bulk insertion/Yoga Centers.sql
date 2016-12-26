USE [DoctorsManagementSystem]
GO

/****** Object:  Table [dbo].[Yogacenter]    Script Date: 18/12/2016 5.50.34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Yogacenter](
	[ProcessingNumberId] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](100) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[Gender] [bit] NOT NULL,
	[Nationality] [varchar](50) NOT NULL,
	[State] [varchar](50) NOT NULL,
	[City] [varchar](50) NOT NULL,
	[PermanantAddress] [varchar](100) NULL,
	[PinCode] [char](10) NOT NULL,
	[PhoneNumber] [varchar](15) NOT NULL,
	[Date] [date] NOT NULL,
	[Email] [varchar](50) NULL,
	[AcademicQualification] [varchar](50) NULL,
	[Course] [int] NOT NULL,
	[HealthStatus] [int] NOT NULL,
 CONSTRAINT [PK_Yogacenter] PRIMARY KEY CLUSTERED 
(
	[ProcessingNumberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

