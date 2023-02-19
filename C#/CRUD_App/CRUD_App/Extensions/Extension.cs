using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRUD_App.Extensions
{
    public static class Extension
    {
        public static T Clone<T>(this T obj) where T : class
        {
            var tJson = JsonSerializer.Serialize(obj, typeof(T)); 

            var clone = JsonSerializer.Deserialize<T>(tJson);

            return clone;
        }
    }
}
