DELETE FROM [PolicyChangeOptions] where LineOfBusinessID=2 and options='Adding/Deleting Location'
UPDATE [PolicyChangeOptions] SET Options=N'Mortgagee/Loss Payee' WHERE Options=N'Mortgagee/Loss Payee changes' AND LineOfBusinessID=2;
