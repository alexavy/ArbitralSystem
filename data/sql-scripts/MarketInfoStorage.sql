
/*
   Market Info Storage Database Scripts
*/


-- # Global settings ----

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- # Security ----

CREATE ROLE [db_executor] AUTHORIZATION [dbo]
GO

GRANT EXECUTE TO [db_executor]
GO

CREATE USER [mlbot] FOR LOGIN [mlbot] WITH DEFAULT_SCHEMA=[dbo]
GO

EXEC sp_addrolemember @rolename = N'db_datareader', @membername = N'mlbot'
EXEC sp_addrolemember @rolename = N'db_executor', @membername = N'mlbot'
go



-- # Tables ----

create table [dbo].[OrderbookPriceEntries](
    [Symbol] [varchar](16) NOT NULL,
    [Price] [decimal](19, 9) NOT NULL,
    [Quantity] [decimal](19, 9) NOT NULL,
    [Exchange] [tinyint] NOT NULL,
    [Direction] [tinyint] NOT NULL,
    [CatchAt] [datetime2](7) NOT NULL
) on [PRIMARY]

go


CREATE CLUSTERED COLUMNSTORE INDEX [CCI-OrderbookPriceEntries] ON [dbo].[OrderbookPriceEntries] 
WITH (DROP_EXISTING = OFF, COMPRESSION_DELAY = 0, DATA_COMPRESSION = COLUMNSTORE) ON [PRIMARY]



-- # Views ----

create view [dbo].[orderbook_timestamps_vw] 
as
select 
    [Symbol]
    ,min([CatchAt]) as first_orderbook_timestamp
    ,max([CatchAt]) as last_orderbook_timestamp
    ,count(distinct(Exchange)) as exchanges_n
    ,count(*) as quotes_n
from [dbo].[OrderbookPriceEntries]
where [CatchAt] > DATEADD(DAY, -30, GETDATE())
group by [Symbol]

go


create view [dbo].[current_month_trades_stats] 
as
select 
    min([CatchAt]) as start_time
    ,max([CatchAt]) as end_time
    ,count(*) as N
from [dbo].[OrderbookPriceEntries]
group by YEAR([CatchAt]), MONTH([CatchAt]), DAY([CatchAt])

go


create view [dbo].[last_orderbooks_vw] 
as
select [Exchange]
      ,[Symbol]
      ,[Price]
      
      ,[Quantity] as Volume
      ,[Direction]
      
      ,[CatchAt] as [Timestamp]
from [dbo].[OrderbookPriceEntries]
where [CatchAt] > DATEADD(DAY, -7, GETDATE())

go


-- # Programming ----

create procedure [dbo].[get_orderbook_sp]
    @symbol varchar(16)  
    ,@fromDate datetime
    ,@toDate datetime
as
select 
    [Exchange]
    ,[Symbol]
    ,[Price]

    ,[Quantity] as Volume
    ,[Direction]

    ,[CatchAt] as [Timestamp]
from [dbo].[OrderbookPriceEntries]
where 
    [Symbol] = @symbol
    and [CatchAt] >= @fromDate
    and [CatchAt] <= @toDate
;
    
go  


create procedure [dbo].[get_bot_statuses_history_sp]
    @symbol varchar(16)  
    , @fromDate datetime
    , @toDate datetime
as   
    select 
      [Id]
      ,[Exchange]
      ,[PreviousStatus]
      ,[CurrentStatus]
      ,[ChangedAt]
    from [dbo].[DistributerStates]
    where 
        [Symbol] = @symbol
        and [ChangedAt] >= @fromDate
        and [ChangedAt] <= @toDate
;
    
go
