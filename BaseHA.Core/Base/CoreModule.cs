using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using BaseHA.Core.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Module = Autofac.Module;

namespace BaseHA.Core.Base
{
    public class CoreModule : Module
    {

        public CoreModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {


            // Logging stuff
            builder.RegisterType<Notifier>().As<INotifier>().InstancePerLifetimeScope();
            builder.RegisterType<CommonServices>().As<ICommonServices>().InstancePerLifetimeScope();
            builder.RegisterType<NullTypeFinder>().As<ITypeFinder>().InstancePerLifetimeScope();
        }

        //protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
        //{
        //    // Look for first settable property of type "ICommonServices" and inject
        //    var servicesProperty = FindCommonServicesProperty(registration.Activator.LimitType);

        //    if (servicesProperty == null)
        //        return;

        //    registration.Metadata.Add("Property.ICommonServices", FastProperty.Create(servicesProperty));

        //    registration.on += (sender, e) =>
        //    {
        //        if (DataSettings.DatabaseIsInstalled())
        //        {
        //            var prop = e.Component.Metadata.Get("Property.ICommonServices") as FastProperty;
        //            var services = e.Context.Resolve<ICommonServices>();
        //            prop.SetValue(e.Instance, services);
        //        }
        //    };
        //}

        private static PropertyInfo FindCommonServicesProperty(Type type)
        {
            var prop = type
                .GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
                .Select(p => new
                {
                    PropertyInfo = p,
                    p.PropertyType,
                    IndexParameters = p.GetIndexParameters(),
                    Accessors = p.GetAccessors(false)
                })
                .Where(x => x.PropertyType == typeof(ICommonServices)) // must be ICommonServices
                .Where(x => x.IndexParameters.Count() == 0) // must not be an indexer
                .Where(x => x.Accessors.Length != 1 || x.Accessors[0].ReturnType == typeof(void)) //must have get/set, or only set
                .Select(x => x.PropertyInfo)
                .FirstOrDefault();

            return prop;
        }

        private IEnumerable<Action<IComponentContext, object>> BuildLoggerInjectors(Type componentType)
        {
            // Look for first settable property of type "ICommonServices" 
            var loggerProperties = componentType
                .GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
                .Select(p => new
                {
                    PropertyInfo = p,
                    p.PropertyType,
                    IndexParameters = p.GetIndexParameters(),
                    Accessors = p.GetAccessors(false)
                })
                .Where(x => x.PropertyType == typeof(ILogger)) // must be a logger
                .Where(x => x.IndexParameters.Count() == 0) // must not be an indexer
                .Where(x => x.Accessors.Length != 1 || x.Accessors[0].ReturnType == typeof(void)) //must have get/set, or only set
                .Select(x => FastProperty.Create(x.PropertyInfo));

            // Return an array of actions that resolve a logger and assign the property
            foreach (var prop in loggerProperties)
            {
                yield return (ctx, instance) =>
                {
                    string component = componentType.ToString();
                    var logger = ctx.Resolve<ILogger>();
                    prop.SetValue(instance, logger);
                };
            }
        }
    }
}
