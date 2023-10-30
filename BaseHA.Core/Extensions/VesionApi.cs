using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseHA.Core.Extensions
{
    public static class VesionApi
    {
        public static string Vision { get; set; }


        public static string GetVisionToApi()
        {
            return "v1";
        }
    }
}
