using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
namespace BaseHA.Domain
{
    //cài các phụ thuộc, chạy câu lệnh dưới để lấy template và sửa
    //mỗi lần update thì run câu lệnh dưới, run dưới dạng power shell
    //dotnet ef dbcontext scaffold "Data Source=desktop-itlr9t6;Initial Catalog=WarehouseManagement;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -o Entity -c WareHouseContext -f --context-dir Contexts
    public class ScaffoldingDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            services.AddHandlebarsScaffolding(options =>
            {
                // Add custom template data
                options.TemplateData = new Dictionary<string, object>
    {
        { "models-namespace", "Share.BaseCore" },
        { "base-class", "BaseEntity" }
    };
            });
        }
    }
}
