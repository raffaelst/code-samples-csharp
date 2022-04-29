CREATE TABLE [dbo].[Segment] (
    [IdSegment]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [IdSubsector] BIGINT           NOT NULL,
    [Name]        NVARCHAR (250)   NOT NULL,
    [GuidSegment] UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_Segment] PRIMARY KEY CLUSTERED ([IdSegment] ASC),
    CONSTRAINT [FK_Subsector_Segment] FOREIGN KEY ([IdSubsector]) REFERENCES [dbo].[Subsector] ([IdSubsector])
);


GO
CREATE NONCLUSTERED INDEX [Idx_Segment]
    ON [dbo].[Segment]([IdSubsector] ASC);

