using Microsoft.Xrm.Sdk;
using NasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace NasAPI.Helpers
{
    public static class CrmToModelMapper<T> where T : BaseModel, new()
    {
        public static string GetModelPropertyName(string crmColName)
        {
            Type type = typeof(T);
            var property = type.GetProperties().FirstOrDefault(
                p => p.GetCustomAttributes(false).Any(
                    a => a.GetType() == typeof(CrmColumnAttribute) && (a as CrmColumnAttribute).Name == crmColName)
                    );

            return (property == null ? String.Empty : property.Name);
        }
        public static Dictionary<string, string> GetCrmToModelMapper()
        {
            Dictionary<string, string> mapper = new Dictionary<string, string>();

            Type type = typeof(T);
            var properties = type.GetProperties();
            foreach (var prop in properties)
            {
                var attribute = prop.GetCustomAttributes(false).FirstOrDefault(a => a.GetType() == typeof(CrmColumnAttribute));
                if (attribute != null)
                {
                    mapper[(attribute as CrmColumnAttribute).Name] = prop.Name;
                }
            }
            return mapper;
        }
        public static IEnumerable<string> GetCrmToModelMapper(IEnumerable<string> crmColumns)
        {
            var allColumnsMapping = GetCrmToModelMapper();
            return allColumnsMapping.Where(k => crmColumns.Any(c => c == k.Key)).Select(k => k.Value);

            //Dictionary<string, string> mapper = crmColumns.ToDictionary(c => c, c => string.Empty);
        }

        public static T CastFromCrm(Entity entity)
        {
            var obj = new T();
            return CastFromCrm(entity, obj);
        }

        public static T CastFromCrm(Entity entity, T _this)
        {
            var obj = _this;
            Type type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                var attribute = prop.GetCustomAttributes(false).FirstOrDefault(a => a.GetType() == typeof(CrmColumnAttribute));
                if (attribute != null)
                {
                    var attrName = (attribute as CrmColumnAttribute).Name;
                    Type propType = prop.GetType();
                    if (entity.Attributes.ContainsKey(attrName))
                        prop.SetValue(obj, entity[attrName] ); // error : need to cast 
                        //prop.SetValue(obj, entity[attrName] as propType); // error : need to cast 
                }
            }



            return obj;
        }

       

    }
}