CREATE TABLE [dbo].[StockType] (
    [IdStockType]   INT              NOT NULL,
    [Name]          NVARCHAR (50)    NOT NULL,
    [GuidStockType] UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_StockType] PRIMARY KEY CLUSTERED ([IdStockType] ASC)
);

