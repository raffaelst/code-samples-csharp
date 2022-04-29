CREATE TABLE [dbo].[Portfolio] (
    [IdPortfolio]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (150)   NOT NULL,
    [CreatedDate]   DATETIME         NOT NULL,
    [Active]        BIT              NOT NULL,
    [GuidPortfolio] UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    [IdTrader]      BIGINT           NOT NULL,
    CONSTRAINT [PK_Portfolio] PRIMARY KEY CLUSTERED ([IdPortfolio] ASC),
    CONSTRAINT [FK_Portfolio_Trader] FOREIGN KEY ([IdTrader]) REFERENCES [dbo].[Trader] ([IdTrader])
);

