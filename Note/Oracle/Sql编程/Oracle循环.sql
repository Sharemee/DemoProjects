declare
  x number;
begin
  x:=12;
  <<repeat_loop>>
  x:=x-1;
  Dbms_Output.put_line(x);
  if x>9 then
    GOTO repeat_loop;
  end if;
end;

