CREATE TABLE [dbo].[Dividend] (
    [IdDividend]       BIGINT           IDENTITY (1, 1) NOT NULL,
    [IdStock]          BIGINT           NOT NULL,
    [IdPortfolio]      BIGINT           NOT NULL,
    [PaymentDate]      DATETIME         NULL,
    [BaseQuantity]     INT              NOT NULL,
    [GrossValue]       DECIMAL (18, 2)  NOT NULL,
    [NetValue]         DECIMAL (18, 2)  NOT NULL,
    [NotificationSent] BIT              NOT NULL,
    [GuidDividend]     UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    [HomeBroker]       NVARCHAR (MAX)   NOT NULL,
    [IdDividendType] INT NOT NULL, 
    CONSTRAINT [PK_Dividend] PRIMARY KEY CLUSTERED ([IdDividend] ASC),
    CONSTRAINT [FK_Portfolio_Dividend] FOREIGN KEY ([IdPortfolio]) REFERENCES [dbo].[Portfolio] ([IdPortfolio]),
    CONSTRAINT [FK_Stock_Dividend] FOREIGN KEY ([IdStock]) REFERENCES [dbo].[Stock] ([IdStock]), 
    CONSTRAINT [FK_DividendType_Dividend] FOREIGN KEY ([IdDividendType]) REFERENCES [dbo].[DividendType]([IdDividendType])
);


GO
CREATE NONCLUSTERED INDEX [Idx_Dividend_Stock]
    ON [dbo].[Dividend]([IdStock] ASC);


GO
CREATE NONCLUSTERED INDEX [Idx_Dividend_Portfolio]
    ON [dbo].[Dividend]([IdPortfolio] ASC);

