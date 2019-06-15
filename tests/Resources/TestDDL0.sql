CREATE TABLE [FactSalesQuota](
	[SalesQuotaKey] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeKey] [int] NOT NULL,
	[weight] [numeric](10, 2) NULL,
	[DateKey] [int] NOT NULL,
	[CalendarYear] [smallint] NOT NULL,
	[CalendarQuarter] [tinyint] NOT NULL,
	[SalesAmountQuota] [money] NOT NULL,
	[Date] [datetime] NULL
) ON [PRIMARY];