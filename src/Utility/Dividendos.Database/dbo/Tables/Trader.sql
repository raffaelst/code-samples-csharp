CREATE TABLE [dbo].[Trader] (
    [IdTrader]    BIGINT           IDENTITY (1, 1) NOT NULL,
    [IdUser]      NVARCHAR (450)   NOT NULL,
    [Identifier]  NVARCHAR (30)    NOT NULL,
    [Password]    NVARCHAR (50)    NOT NULL,
    [CreatedDate] DATETIME         NOT NULL,
    [Active]      BIT              NOT NULL,
    [GuidTrader]  UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Trader] PRIMARY KEY CLUSTERED ([IdTrader] ASC),
    CONSTRAINT [FK_Trader_AspNetUsers] FOREIGN KEY ([IdUser]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

