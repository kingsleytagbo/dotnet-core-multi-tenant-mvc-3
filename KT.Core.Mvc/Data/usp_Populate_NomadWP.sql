USE [NomadWp]
GO

/****** Object:  StoredProcedure [dbo].[usp_Populate_NomadWP]    Script Date: 12/9/2020 2:37:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE OR ALTER   PROCEDURE [dbo].[usp_Populate_NomadWP] AS

BEGIN

	IF OBJECT_ID(N'wp_post', N'U') IS NOT NULL
	BEGIN
	  PRINT 'wp_post Exists'
	END
	ELSE 
	BEGIN
		PRINT 'Creating wp_post'
		CREATE TABLE [dbo].[wp_post](
			[ID] [bigint] IDENTITY(1,1) NOT NULL,
			[post_author] [bigint] NOT NULL,
			[post_date] [nvarchar](32) NOT NULL,
			[post_date_gmt] [nvarchar](32) NOT NULL,
			[post_content] [nvarchar](max) NOT NULL,
			[post_title] [nvarchar](max) NOT NULL,
			[post_excerpt] [nvarchar](max) NOT NULL,
			[post_status] [nvarchar](20) NOT NULL,
			[comment_status] [nvarchar](20) NOT NULL,
			[ping_status] [nvarchar](20) NOT NULL,
			[post_password] [nvarchar](20) NOT NULL,
			[post_name] [nvarchar](200) NOT NULL,
			[to_ping] [nvarchar](max) NOT NULL,
			[pinged] [nvarchar](max) NOT NULL,
			[post_modified] [nvarchar](32) NOT NULL,
			[post_modified_gmt] [nvarchar](32) NOT NULL,
			[post_content_filtered] [nvarchar](max) NOT NULL,
			[post_parent] [bigint] NOT NULL,
			[guid] [nvarchar](255) NOT NULL,
			[post_type] [nvarchar](20) NOT NULL,
			[post_mime_type] [nvarchar](100) NOT NULL,
			[comment_count] [bigint] NOT NULL,
			[post_category] [nvarchar](32) NULL,
			[site_id] [bigint] NOT NULL,
		PRIMARY KEY CLUSTERED 
		(
			[ID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	END

    IF OBJECT_ID(N'wp_image', N'U') IS NOT NULL
	BEGIN
	  PRINT 'wp_image Exists'
	END
	ELSE 
	BEGIN
		PRINT 'Creating wp_image'
		CREATE TABLE [dbo].[wp_image](
			[ID] [bigint] IDENTITY(1,1) NOT NULL,
			[url] [nvarchar](256) NOT NULL,
			[name] [nvarchar](32) NOT NULL,
			[content] [varbinary](max) NOT NULL,
			[category] [nvarchar](32) NOT NULL,
			[site_id] [bigint] NOT NULL,
		PRIMARY KEY CLUSTERED 
		(
			[ID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	END

	IF OBJECT_ID(N'wp_user', N'U') IS NOT NULL
	BEGIN
	  PRINT 'wp_user Exists'
	END
	ELSE 
	BEGIN
		PRINT 'Creating wp_user'

		/****** Object:  Table [dbo].[wp_user]    Script Date: 12/7/2020 8:40:51 PM ******/
		SET ANSI_NULLS ON
		SET QUOTED_IDENTIFIER ON

		CREATE TABLE [dbo].[wp_user](
			[ID] [bigint] IDENTITY(1,1) NOT NULL,
			[user_login] [nvarchar](60) NOT NULL,
			[user_pass] [nvarchar](64) NOT NULL,
			[user_nicename] [nvarchar](50) NOT NULL,
			[user_email] [nvarchar](100) NOT NULL,
			[user_url] [nvarchar](100) NOT NULL,
			[user_registered] [nvarchar](32) NOT NULL,
			[user_activation_key] [nvarchar](60) NOT NULL,
			[user_status] [int] NOT NULL,
			[display_name] [nvarchar](250) NOT NULL,
			[first_name] [nvarchar](64) NULL,
			[last_name] [nvarchar](64) NULL,
			[auto_join_complete] [nvarchar](10) NULL,
			[spam] [tinyint] NOT NULL,
			[deleted] [tinyint] NOT NULL,
			[site_id] [bigint] NOT NULL,
		PRIMARY KEY CLUSTERED 
		(
			[ID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		 CONSTRAINT [wp_user_user_login_key] UNIQUE NONCLUSTERED 
		(
			[site_id] ASC,
			[user_login] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		 CONSTRAINT [wp_user_user_nicename] UNIQUE NONCLUSTERED 
		(
			[site_id] ASC,
			[user_nicename] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[wp_user] ADD  DEFAULT ('') FOR [user_login]
		ALTER TABLE [dbo].[wp_user] ADD  DEFAULT ('') FOR [user_pass]
		ALTER TABLE [dbo].[wp_user] ADD  DEFAULT ('') FOR [user_nicename]
		ALTER TABLE [dbo].[wp_user] ADD  DEFAULT ('') FOR [user_email]
		ALTER TABLE [dbo].[wp_user] ADD  DEFAULT ('') FOR [user_url]
		ALTER TABLE [dbo].[wp_user] ADD  DEFAULT ('0000-00-00 00:00:00') FOR [user_registered]
		ALTER TABLE [dbo].[wp_user] ADD  DEFAULT ('') FOR [user_activation_key]
		ALTER TABLE [dbo].[wp_user] ADD  DEFAULT ('0') FOR [user_status]
		ALTER TABLE [dbo].[wp_user] ADD  DEFAULT ('') FOR [display_name]
		ALTER TABLE [dbo].[wp_user] ADD  DEFAULT ('') FOR [auto_join_complete]
		ALTER TABLE [dbo].[wp_user] ADD  DEFAULT ('0') FOR [spam]
		ALTER TABLE [dbo].[wp_user] ADD  DEFAULT ('0') FOR [deleted]
		ALTER TABLE [dbo].[wp_user] ADD  DEFAULT ((1)) FOR [site_id]


	END

END

GO


