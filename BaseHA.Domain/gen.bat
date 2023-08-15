rmdir Entity /s /q
rmdir Contexts /s /q
dotnet ef dbcontext scaffold "Data Source=ADMIN;Initial Catalog=WarehouseManagement;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False" Microsoft.EntityFrameworkCore.SqlServer -o Entity -c WareHouseContext -f --context-dir Contexts