CREATE TABLE [dbo].[Subsector] (
    [IdSubsector]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [IdSector]      BIGINT           NOT NULL,
    [Name]          NVARCHAR (200)   NOT NULL,
    [GuidSubsector] UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_SubSector] PRIMARY KEY CLUSTERED ([IdSubsector] ASC),
    CONSTRAINT [FK_Sector_Subsector] FOREIGN KEY ([IdSector]) REFERENCES [dbo].[Sector] ([IdSector])
);


GO
CREATE NONCLUSTERED INDEX [Idx_Subsector]
    ON [dbo].[Subsector]([IdSector] ASC);

