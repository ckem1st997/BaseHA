using EntityFrameworkCore.Scaffolding.Handlebars;
using Google.Protobuf.WellKnownTypes;
using HandlebarsDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using BaseHA.Core;
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
            //  services.AddSingleton<ICSharpEntityTypeGenerator, MyHbsCSharpEntityTypeGenerator>();
            services.AddHandlebarsScaffolding(options =>
            {
                // Add custom template data
                options.TemplateData = new Dictionary<string, object>
    {
        { "models-namespace", "BaseHA.Core" },
        { "base-class", "BaseEntity" },

    };
                // Generate both context and entities
                options.GenerateComments = true;
                options.ExcludedTables= new List<string>()
                {
                    "VWareHouseLedger"
                };
            });
            //// Add Handlebars transformer for Country property
            services.AddHandlebarsTransformers(
                propertyTransformer: p =>

                    p.PropertyName.Equals("Id")
                        ? new EntityPropertyInfo("override " +p.PropertyType, "Id", false)
                        : (p.PropertyName.Equals("OnDelete") ? new EntityPropertyInfo("override " + p.PropertyType, p.PropertyName, false) : new EntityPropertyInfo(p.PropertyType, p.PropertyName, p.PropertyIsNullable)));

        }
    }

    //public class MyHbsCSharpEntityTypeGenerator : HbsCSharpEntityTypeGenerator
    //{
    //    private readonly IOptions<HandlebarsScaffoldingOptions> _options;

    //    public MyHbsCSharpEntityTypeGenerator(IAnnotationCodeGenerator annotationCodeGenerator, ICSharpHelper cSharpHelper, IEntityTypeTemplateService entityTypeTemplateService, IEntityTypeTransformationService entityTypeTransformationService, IOptions<HandlebarsScaffoldingOptions> options) : base(annotationCodeGenerator, cSharpHelper, entityTypeTemplateService, entityTypeTransformationService, options)
    //    {
    //        _options = options;
    //    }

    //    protected override void GenerateProperties(IEntityType entityType)
    //    {
    //        var properties = new List<Dictionary<string, object>>();
    //        foreach (var property in entityType.GetProperties().OrderBy(p => p.GetColumnName()))
    //        {
    //            Console.WriteLine(property.Name);
    //            PropertyAnnotationsData = new List<Dictionary<string, object>>();

    //            if (UseDataAnnotations)
    //            {
    //                GeneratePropertyDataAnnotations(property);
    //            }

    //            var propertyType = CSharpHelper.Reference(property.ClrType);
    //            if (_options?.Value == null
    //                && property.IsNullable
    //                && !propertyType.EndsWith("?"))
    //            {
    //                propertyType += "?";
    //            }
    //            // Code elided for clarity
    //            properties.Add(new Dictionary<string, object>
    //  {

    //          // Add new item to template data
    //                { "property-isprimarykey", property.IsPrimaryKey() },
    //                { "property-not-id", property.Name.Equals("Id")==false },
    //                { "property-not-delete",property.Name.Equals("OnDelete")==false }
    //  });
    //        }

    //        var transformedProperties = EntityTypeTransformationService.TransformProperties(entityType, properties);

    //        // Add to transformed properties
    //        for (int i = 0; i < transformedProperties.Count; i++)
    //        {
    //            transformedProperties[i].Add("property-isprimarykey", properties[i]["property-isprimarykey"]);
    //            Console.Write(transformedProperties[i]);
    //        }

    //        TemplateData.Add("properties", transformedProperties);
    //    }
    //}
}
