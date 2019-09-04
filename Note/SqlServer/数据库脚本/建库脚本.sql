--1.创建一个数据库(直接创建一个数据库, 没有设置任何特殊选项, 都是使用默认选项)
Create Database DBNAME;

--2.删除数据库
Drop Database DBNAME;

--3.创建数据库的时候设置一些参数选项
Create Database DBNAME
on Primary  --配置主数据文件的选项
(
    Name = 'DBNAME_Data',  --主数据文件的逻辑名称
    Filename = 'E:\SqlServer\DBNAME_Data.mdf',   --主数据文件的实际保存路径
    Size = 10MB,    --主文件的初始大小
    Maxsize = 10GB, --最大容量
    Filegrowth = 100MB --每次扩容大小
)
Log on  --配置日志文件选项
(
    Name = 'DBNAME_Log',    --日志文件逻辑名称
    Filename = 'E:\SqlServer\DBNAME_Log.ldf',   --日志文件实际保存路径
    Size = 5MB, --日志文件初始大小
    Filegrowth = 5MB,   --自动扩容大小
)