CREATE TABLE [FactSalesQuota](
	[SalesQuotaKey] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[EmployeeKey] [int] NOT NULL,
	[weight] [numeric](10, 2) NULL,
	[DateKey] [int] NOT NULL,
	[CalendarYear] [smallint] NOT NULL,
	[CalendarQuarter] [tinyint] NOT NULL,
	[SalesAmountQuota] [money] NOT NULL,
	[Date] [datetime] NULL
) ON [PRIMARY]
GO

EXECUTE sys.sp_addextendedproperty 
	@level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FactSalesQuota'
	, @name = N'MS_Description'
	, @value = N'Sales quota fact table';
GO

EXECUTE sp_addextendedproperty 
	@level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FactSalesQuota'
	, @level2type = N'COLUMN', @level2name = N'SalesQuotaKey'
	, @name = N'MS_Description'
	, @value = N'Sales quota row identifier.';
GO

EXECUTE sp_addextendedproperty 
	@level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FactSalesQuota'
	, @level2type = N'COLUMN', @level2name = N'SalesAmountQuota'
	, @name = N'MS_Description'
	, @value = N'Sales amount quota.';

GO