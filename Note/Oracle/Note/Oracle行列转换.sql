select * from (select nation,city,ranking from temp)
pivot (max(city) for ranking in ('第一' as 第一,'第二' AS 第二,'第三' AS 第三,'第四' AS 第四));