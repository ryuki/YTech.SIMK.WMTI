USE [master]
GO
/****** Object:  Database [DB_SIMK_WMTI]    Script Date: 10/19/2013 02:29:55 ******/
CREATE DATABASE [DB_SIMK_WMTI] ON  PRIMARY 
( NAME = N'DB_SIMK_WMTI', FILENAME = N'E:\Database\SQLDEV2005\DB_SIMK_WMTI.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DB_SIMK_WMTI_log', FILENAME = N'E:\Database\SQLDEV2005\DB_SIMK_WMTI_log.ldf' , SIZE = 1280KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [DB_SIMK_WMTI] SET COMPATIBILITY_LEVEL = 90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DB_SIMK_WMTI].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [DB_SIMK_WMTI] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET ANSI_NULLS OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET ANSI_PADDING OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET ARITHABORT OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [DB_SIMK_WMTI] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [DB_SIMK_WMTI] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [DB_SIMK_WMTI] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET  DISABLE_BROKER
GO
ALTER DATABASE [DB_SIMK_WMTI] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [DB_SIMK_WMTI] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [DB_SIMK_WMTI] SET  READ_WRITE
GO
ALTER DATABASE [DB_SIMK_WMTI] SET RECOVERY FULL
GO
ALTER DATABASE [DB_SIMK_WMTI] SET  MULTI_USER
GO
ALTER DATABASE [DB_SIMK_WMTI] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [DB_SIMK_WMTI] SET DB_CHAINING OFF
GO
