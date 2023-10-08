using Autofac;
using BaseHA.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseHA.Core.Base
{
    public interface ICommonServices
    {
        IComponentContext Container { get; }
        INotifier Notifier { get; }
    }


    public class CommonServices : ICommonServices
    {
        private readonly IComponentContext _container;
        private readonly Lazy<INotifier> _notifier;

        public CommonServices(
            IComponentContext container,
            Lazy<INotifier> notifier)
        {
            _container = container;
            _notifier = notifier;
        }

        public IComponentContext Container => _container;
        public INotifier Notifier => _notifier.Value;
    }

}
