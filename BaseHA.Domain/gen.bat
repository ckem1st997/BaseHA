rmdir Entity /s /q
rmdir Contexts /s /q
dotnet ef dbcontext scaffold "Data Source=LAPTOP-2OONI8N5;Initial Catalog=Category;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -o Entity -c CategoryTbContext -f --context-dir Contexts
timeout /t 1