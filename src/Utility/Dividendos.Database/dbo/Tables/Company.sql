CREATE TABLE [dbo].[Company] (
    [IdCompany]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100)   NOT NULL,
    [IdLogo]      BIGINT           NULL,
    [IdSegment]   BIGINT           NOT NULL,
    [FullName]    NVARCHAR (300)   NULL,
    [GuidCompany] UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    [Code]        NVARCHAR (20)    NULL,
    CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([IdCompany] ASC),
    CONSTRAINT [FK_Logo_Company] FOREIGN KEY ([IdLogo]) REFERENCES [dbo].[Logo] ([IdLogo]),
    CONSTRAINT [FK_Segment_Company] FOREIGN KEY ([IdSegment]) REFERENCES [dbo].[Segment] ([IdSegment]),
    CONSTRAINT [UQ_Company_Code] UNIQUE NONCLUSTERED ([Code] ASC)
);


GO
CREATE NONCLUSTERED INDEX [Idx_Company_Logo]
    ON [dbo].[Company]([IdLogo] ASC);


GO
CREATE NONCLUSTERED INDEX [Idx_Company_Segment]
    ON [dbo].[Company]([IdSegment] ASC);

