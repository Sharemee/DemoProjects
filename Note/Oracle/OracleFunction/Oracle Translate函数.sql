--需求一: 找出A字段中非数字的记录(A字段保存的是数字字符串,如'123',找出错误插入文本字符的记录)
Select * From Table t Where Translate(t.A,'*0123456789','*') is not null;

--说明: Translate 是用来替换字符的函数

--语法：
Translate(char, from_str,to_str)
--char: 待处理的字符串
--from_str: 按顺序排列若干个要被替换的字符集合，注意是字符集合而不是字符串。
--to_str: 按顺序对应from_str要被替换成的字符集合。如果to_str是blank('')或者null,则所有字符都会替换成null

--注意: 
--from_str和to_str 是一对一转换的关系所以
translate('abcd','abc','A') -- 字符a会被'A'替换， 而b与c则没有指定替换成什么，oracle会默认替换成null。
--所以返回'Ad'.