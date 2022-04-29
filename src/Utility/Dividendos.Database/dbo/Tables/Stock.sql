CREATE TABLE [dbo].[Stock] (
    [IdStock]     BIGINT           IDENTITY (1, 1) NOT NULL,
    [IdCompany]   BIGINT           NOT NULL,
    [Symbol]      NVARCHAR (10)    NOT NULL,
    [IdStockType] INT              NOT NULL,
    [GuidStock]   UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_Stock] PRIMARY KEY CLUSTERED ([IdStock] ASC),
    CONSTRAINT [FK_Company_Stock] FOREIGN KEY ([IdCompany]) REFERENCES [dbo].[Company] ([IdCompany]),
    CONSTRAINT [FK_StockType_Stock] FOREIGN KEY ([IdStockType]) REFERENCES [dbo].[StockType] ([IdStockType]),
    CONSTRAINT [UQ_Stock_Symbol] UNIQUE NONCLUSTERED ([Symbol] ASC)
);


GO
CREATE NONCLUSTERED INDEX [Idx_Stock_Company]
    ON [dbo].[Stock]([IdCompany] ASC);


GO
CREATE NONCLUSTERED INDEX [Idx_Stock_StockType]
    ON [dbo].[Stock]([IdStockType] ASC);

