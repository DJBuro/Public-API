SELECT name, avg_fragmentation_in_percent
FROM sys.dm_db_index_physical_stats (
       DB_ID(N'cloud4')
     , OBJECT_ID(N'audit')
     , NULL
     , NULL
     , NULL) AS a
JOIN sys.indexes AS b 
ON a.object_id = b.object_id AND a.index_id = b.index_id;


GO 

SELECT TOP 10 query_stats.query_hash AS "Query Hash",
    SUM(query_stats.total_worker_time) / SUM(query_stats.execution_count) AS "Avg CPU Time",
	SUM(query_stats.execution_count) AS "execution count",
    MIN(query_stats.statement_text) AS "Statement Text"
FROM
    (SELECT QS.*,
    SUBSTRING(ST.text, (QS.statement_start_offset/2) + 1,
    ((CASE statement_end_offset
        WHEN -1 THEN DATALENGTH(ST.text)
        ELSE QS.statement_end_offset END
            - QS.statement_start_offset)/2) + 1) AS statement_text
     FROM sys.dm_exec_query_stats AS QS
     CROSS APPLY sys.dm_exec_sql_text(QS.sql_handle) as ST) as query_stats

GROUP BY query_stats.query_hash
--having query_stats.execution_count > 100
ORDER BY 2 DESC;