ALTER TABLE table_name
ADD CONSTRAINT constraint_name CHECK (column_name condition) [DISABLE];
--DISABLE关键之是可选项。如果使用了DISABLE关键字，当CHECK约束被创建后，CHECK约束的限制条件不会生效。