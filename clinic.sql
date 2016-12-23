USE [DoctorsManagementSystem]
GO

/****** Object:  Table [dbo].[Clinic]    Script Date: 22/12/2016 10.58.26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Clinic](
	[ClinicId] [int] IDENTITY(1,1) NOT NULL,
	[DoctorId] [int] NOT NULL,
	[ClinicName] [varchar](50) NOT NULL,
	[Address] [varchar](50) NOT NULL,
	[City] [varchar](50) NOT NULL,
	[State] [varchar](50) NOT NULL,
	[PinCode] [varchar](10) NOT NULL,
	[Latitude] [decimal](18, 0) NOT NULL,
	[Longitude] [decimal](18, 0) NOT NULL,
	[ReceptionistAvaliable] [bit] NOT NULL,
	[LoginNeededForReceptionist] [bit] NULL,
	[ReceptionistLoginMobileNumber] [varchar](15) NULL,
	[LandlineNumberOrMobileNumber] [varchar](50) NOT NULL,
	[EmailId] [varchar](50) NULL,
	[MorningStartTime] [varchar](50) NOT NULL,
	[MorningEndTime] [varchar](50) NOT NULL,
	[EveningStartTime] [varchar](50) NOT NULL,
	[EveningEndTime] [varchar](50) NOT NULL,
	[Holidays] [varchar](50) NOT NULL,
	[ScheduleDeatils] [varchar](50) NULL,
	[AppointmentMode] [bit] NOT NULL,
	[AppointmentSlotTime] [bit] NULL,
	[AdditionalServicesInClinic] [varchar](100) NULL,
	[Fees] [nchar](10) NULL,
	[FollowUp] [nchar](10) NULL,
 CONSTRAINT [PK_Clinic] PRIMARY KEY CLUSTERED 
(
	[ClinicId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Clinic]  WITH CHECK ADD  CONSTRAINT [FK_Clinic_DoctorId] FOREIGN KEY([DoctorId])
REFERENCES [dbo].[Doctor] ([DoctorId])
GO

ALTER TABLE [dbo].[Clinic] CHECK CONSTRAINT [FK_Clinic_DoctorId]
GO

