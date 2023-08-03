using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Share.BaseCore.DiagnosticListener
{
    #region DiagnosticObserver
    public class DiagnosticObserver : IObserver<System.Diagnostics.DiagnosticListener>
    {
        public void OnCompleted()
            => throw new NotImplementedException();

        public void OnError(Exception error)
            => throw new NotImplementedException();

        public void OnNext(System.Diagnostics.DiagnosticListener value)
        {
            if (value.Name == DbLoggerCategory.Name) // "Microsoft.EntityFrameworkCore"
            {
                value.Subscribe(new KeyValueObserver());
            }
        }
    }
    #endregion

    #region KeyValueObserver
    public class KeyValueObserver : IObserver<KeyValuePair<string, object>>
    {
        public void OnCompleted()
            => throw new NotImplementedException();

        public void OnError(Exception error)
            => throw new NotImplementedException();

        public void OnNext(KeyValuePair<string, object> value)
        {
            if (value.Key == CoreEventId.ContextInitialized.Name)
            {
                var payload = (ContextInitializedEventData)value.Value;
                Console.WriteLine($"EF is initializing {payload.Context.GetType().Name} ");
            }

            if (value.Key == RelationalEventId.ConnectionOpening.Name)
            {
                var payload = (ConnectionEventData)value.Value;
                Console.WriteLine($"EF is opening a connection to {payload.Connection.ConnectionString} ");
            }
        }
    }
    #endregion  
}


