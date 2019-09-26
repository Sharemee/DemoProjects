--医院统计数据之用户信息
MERGE INTO STATHOSUSERINFO T1
USING(
    --注册用户可能证件号码类型改了但号码没变
    SELECT TS1.HOSID,TS1.USERPERMITTYPE,TS1.USERPERMITNUM,TS1.USERNAME,TS1.USERREGTIME
    FROM(
        SELECT ROW_NUMBER() OVER (PARTITION BY TS.HOSID,TS.USERPERMITNUM ORDER BY TS.USERREGTIME DESC) RN,TS.*
        FROM (
            --找到了最早注册的用户
            SELECT TEM.NETHOSID AS HOSID,TEM.USERPERMITTYPE,TEM.USERPERMITNUM,TEM.USERNAME,TEM.TREATSHEETTIME AS USERREGTIME
            FROM(
                SELECT ROW_NUMBER() OVER(PARTITION BY T.NETHOSID,T.USERPERMITTYPE,T.USERPERMITNUM ORDER BY T.TREATSHEETTIME ASC) RN,T.*
                FROM SDTREATINFO T
                --WHERE T.TREATSHEETTIME BETWEEN 
            ) TEM
            WHERE TEM.RN=1
        ) TS
    ) TS1
    WHERE TS1.RN=1
) T2
ON (T1.HOSID=T2.HOSID AND T1.USERPERMITNUM=T2.USERPERMITNUM)
WHEN MATCHED THEN --更新:
  UPDATE SET T1.USERPERMITTYPE=T2.USERPERMITTYPE,T1.USERNAME=T2.USERNAME,T1.USERREGTIME=T2.USERREGTIME,T1.UPDATETIME=SYSDATE
  WHERE T1.USERPERMITTYPE!=T2.USERPERMITTYPE OR T1.USERNAME!=T2.USERNAME
WHEN NOT MATCHED THEN --插入:注册时间固定
  INSERT (T1.HOSID,T1.USERPERMITTYPE,T1.USERPERMITNUM,T1.USERNAME,T1.USERREGTIME,T1.UPDATETIME)
  VALUES(T2.HOSID,T2.USERPERMITTYPE,T2.USERPERMITNUM,T2.USERNAME,T2.USERREGTIME,SYSDATE);