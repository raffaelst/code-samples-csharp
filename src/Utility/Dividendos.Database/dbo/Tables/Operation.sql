CREATE TABLE [dbo].[Operation] (
    [IdOperation]    BIGINT           IDENTITY (1, 1) NOT NULL,
    [IdStock]        BIGINT           NOT NULL,
    [IdPortfolio]    BIGINT           NOT NULL,
    [NumberOfShares] BIGINT           NOT NULL,
    [AveragePrice]   DECIMAL (18, 2)  NOT NULL,
    [DateAdded]      DATETIME         NOT NULL,
    [GuidOperation]  UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    [HomeBroker]     NVARCHAR (MAX)   NOT NULL,
    [LastUpdatedDate] DATETIME NOT NULL, 
    CONSTRAINT [PK_Operation] PRIMARY KEY CLUSTERED ([IdOperation] ASC),
    CONSTRAINT [FK_Portfolio_Operation] FOREIGN KEY ([IdPortfolio]) REFERENCES [dbo].[Portfolio] ([IdPortfolio]),
    CONSTRAINT [FK_Stock_Operation] FOREIGN KEY ([IdStock]) REFERENCES [dbo].[Stock] ([IdStock]), 
    CONSTRAINT [UQ_Stock_Portfolio_Stock] UNIQUE ([IdPortfolio], [IdStock])
);


GO
CREATE NONCLUSTERED INDEX [Idx_Operation_Stock]
    ON [dbo].[Operation]([IdStock] ASC);


GO
CREATE NONCLUSTERED INDEX [Idx_Operation_Portfolio]
    ON [dbo].[Operation]([IdPortfolio] ASC);

