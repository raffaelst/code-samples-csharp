CREATE TABLE [dbo].[Sector] (
    [IdSector]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (150)   NOT NULL,
    [GuidSector] UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_Sector] PRIMARY KEY CLUSTERED ([IdSector] ASC)
);

