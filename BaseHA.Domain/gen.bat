rmdir Entity /s /q
rmdir Contexts /s /q
dotnet ef dbcontext scaffold "Data Source=it\mssqlserver01;Initial Catalog=WarehouseManagement;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -o Entity -c WareHouseContext -f --context-dir Contexts

