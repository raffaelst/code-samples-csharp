CREATE TABLE [dbo].[Quotation]
(
	[QuotationId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [rowguid] UNIQUEIDENTIFIER NULL, 
    [SupplierRegistrationNumber] NCHAR(10) NULL
)
