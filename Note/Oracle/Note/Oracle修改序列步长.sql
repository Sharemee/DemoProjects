Alter Sequence SEQ_FILEATTACHMENT increment by 500;--可以为负值
SELECT SEQ_FILEATTACHMENT.Nextval FROM DUAL;  --执行此条语句后: 现序列值=原序列值+步长(11+500=511 SEQ.NextVal=511)
Alter Sequence SEQ_FILEATTACHMENT increment by 1;