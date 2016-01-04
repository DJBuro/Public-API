UPDATE [Dashboard].[dbo].[tbl_Site] Set SiteKey = '54' + CAST(B.SiteId as nvarchar) + '1'
from [Dashboard].[dbo].[tbl_Site] as B
where SiteId = B.SiteId
