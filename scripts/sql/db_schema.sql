SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- Create the database
CREATE DATABASE [LiquidApi]
GO

-- Switch to the newly created database
USE [LiquidApi]
GO

CREATE TABLE [dbo].[artist]
(
	[artist_id] INTEGER PRIMARY KEY NOT NULL,
	[name] NVARCHAR(200) NOT NULL,
	[genre] VARCHAR(200) NULL,
	[country] VARCHAR(200) NULL,
	[formed_year] INTEGER NULL CHECK ([formed_year] IS NULL OR [formed_year] > 0),
	[member_count] INTEGER NULL CHECK ([member_count] IS NULL OR [member_count] > 0)
)
GO

CREATE TABLE [dbo].[album]
(
	[album_id] INTEGER PRIMARY KEY NOT NULL,
	[artist_id] INTEGER NOT NULL FOREIGN KEY REFERENCES [dbo].[artist]([artist_id]),
	[title] NVARCHAR(1000) NOT NULL,
	[genre] VARCHAR(200) NULL,
	[release_year] INTEGER NULL CHECK ([release_year] IS NULL OR [release_year] > 0)
)
GO

CREATE TABLE [dbo].[artist_alias]
(
	[artist_alias_id] INTEGER PRIMARY KEY IDENTITY,
	[artist_id] INTEGER NOT NULL FOREIGN KEY REFERENCES [dbo].[artist]([artist_id]),
	[alias] NVARCHAR(200) NOT NULL UNIQUE CHECK (LEN([alias]) > 0)
)
GO