using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseHA.Core.Enums
{
    public enum DbType
    {
        MSSQL, Oracle
    }

    //
    // Summary:
    //     Represents a log level
    public enum LogLevel
    {
        Debug = 10,
        Information = 20,
        Warning = 30,
        Error = 40,
        Fatal = 50
    }


    public enum CrudType
    {
        Create = 1,
        Read,
        Update,
        Delete
    }
}
