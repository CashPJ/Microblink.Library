using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Mapper.Attributes;

namespace Mapper
{
    
    /// <summary>
    ///  Object-object mapping. 
    ///  Object-object mapping works by transforming an input object of one type 
    ///  into an output object of a different type. What makes this RAD auto mapper interesting is that it works
    ///  on simple conventions and helps with the boring work of mapping type A to type B. 
    ///  As long as type B follows RADs established convention: Property name and type have to be the same 
    ///  zero configuration will be needed to map two types.
    /// </summary>
    public static class Mapper
    {
     
        ///<summary>
        /// Object-object mapper. Transforms an input object of one type into an output object of a different type. 
        /// Mapping properties by convention - same name and same type, ignores case. 
        /// </summary>
        /// <param name="source">Source type</param>
        /// <typeparam name="S">Objects to map from</typeparam>
        /// <typeparam name="D">Mapped destination objects</typeparam>
        public static List<D> Map<S, D>(this IEnumerable<S> source)
        {
            if (source == null)
                return default;

            return source.Select(Map<S, D>).ToList();
        }

        /// <summary>
        /// Object-object mapper. Transforms an input object of one type into an output object of a different type. 
        /// Mapping properties by convention - same name and same type, ignores case. 
        /// For each can write property existing on destination type we will try to find an appropriate property 
        /// on the source object and if it exists copy the value.
        /// </summary>
        /// <param name="source">Source object</param>
        /// <typeparam name="S">Source type</typeparam>
        /// <typeparam name="D">Destination type</typeparam>
        /// <returns></returns>
        public static D Map<S, D>(this S source)
        {
            if (source == null)
                return default;

            D destinationInstance = Activator.CreateInstance<D>();

            IEnumerable<PropertyInfo> sourceParameterList =
                                        typeof(S).GetPublicProperties().
                                        Where(sourceProperty => sourceProperty.CanRead);

            IEnumerable<PropertyInfo> destinationParameters =
                                        typeof(D).GetProperties().
                                        Where(destinationProperty => destinationProperty.CanWrite).
                                        Where(filtered => sourceParameterList.
                                        Count(sourceProperty => sourceProperty.Name == filtered.Name) > 0);

            foreach (PropertyInfo destinationProperty in destinationParameters)
            {
                var sourceProperty = sourceParameterList.FirstOrDefault((p) => p.Name == destinationProperty.Name && destinationProperty.PropertyType == p.PropertyType);

                if (sourceProperty != null)
                    destinationProperty.SetValue(destinationInstance, sourceProperty.GetValue(source, null), null);
            }

            return destinationInstance;
        }

        public static PropertyInfo[] GetPublicProperties(this Type type)
        {
            if (type.IsInterface)
            {
                var propertyInfos = new List<PropertyInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(type);
                queue.Enqueue(type);
                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces())
                    {
                        if (considered.Contains(subInterface)) continue;

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetProperties(
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Instance);

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }

            return type.GetProperties(BindingFlags.FlattenHierarchy
                | BindingFlags.Public | BindingFlags.Instance);
        }



        #region DataTable Maping Helpers


        /// <summary>
        /// Object-object mapper. Transforms an input object of one type into an output object of a different type. 
        /// Mapping properties by convention - same name and same type, ignores case. 
        /// </summary>
        /// <typeparam name="D">Destination type to create</typeparam>
        /// <param name="source">Objects to map from</param>
        /// <returns>Mapped destination objects</returns>
        public static List<D> Map<D>(this DataTable source)
        {
            if (!(source?.Rows.Count > 0)) return new List<D>();
            List<D> list = new List<D>();

            PropertyInfo[] fields =
                            typeof(D).//Get destination type properties
                            GetProperties(BindingFlags.Public | BindingFlags.Instance).//that are public and instance
                            Where(pInfo => !Attribute.IsDefined(pInfo, typeof(DoNotMapAttribute))).//that are not marked with our excluding attribute
                            ToArray();

            foreach (DataRow dr in source.Rows)
            {
                D instance = Activator.CreateInstance<D>();

                foreach (PropertyInfo propertyInfo in fields)
                {

                    //Map to destination with another property name
                    var fieldNameMapAttribute = propertyInfo.GetCustomAttribute<ColumnNameAttribute>();
                    
                    //If FieldNameMapAttribute exists use it or if not use the DataColumn name(DEFAULT)
                    var propertyInfoName = fieldNameMapAttribute?.FieldName ?? propertyInfo.Name;

                    
                    //TODO: D5  - ovo ljepse sloziti, vjerojatno se da
                    if (fieldNameMapAttribute != null && (fieldNameMapAttribute.IsDate || fieldNameMapAttribute.IsDateTime))
                    {                        
                        //SET the converted datetime object to an instance 
                        propertyInfo.SetValue(instance, ConvertSqlDateToString(dr[propertyInfoName], fieldNameMapAttribute.IsDateTime));
                    }
                    else
                    {
                        //SET the converted object to an instance 
                        propertyInfo.SetValue(instance, ConvertSqlTypeToNet(dr[propertyInfoName], propertyInfo.PropertyType));
                    }
                    
                }
                list.Add(instance);
            }
            return list;
        }

        private static object ConvertSqlTypeToNet(object dataColumnValue, Type destinationPropertyType)
        {
            //Convert data values from SQL TO C#
            if (dataColumnValue is SqlBinary)
                return (dataColumnValue == DBNull.Value || dataColumnValue.ToString() == "Null") ? null : ((SqlBinary)(dataColumnValue)).Value;
            else if (destinationPropertyType == typeof(bool))
                return dataColumnValue != DBNull.Value && Convert.ToBoolean(dataColumnValue);
            else
                return dataColumnValue == DBNull.Value ? null : dataColumnValue;
        }

        private static string ConvertSqlDateToString(object dataColumnValue, bool dateTimeFormat = false)
        {
            var date = (DateTime?)ConvertSqlTypeToNet(dataColumnValue, typeof(DateTime?));

            var format = DateFormat;

            if (dateTimeFormat)
            {
                format = DateTimeFormat;
            }

            //in future, this should be passed from mapper initiator
            return date?.ToString(format);
        }

        private const string _DateFormat = "yyyy-MM-dd";
        private const string _DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";

        public static string DateFormat
        {
            get
            {
                //todo: pass mapper date format
                //return ConfigurationManager.AppSettings["MapperDateFormat"] ?? _DateFormat;
                return  _DateFormat;
            }
        }

        public static string DateTimeFormat
        {
            get
            {
                //todo: pass mapper date time format
                //return ConfigurationManager.AppSettings["MapperDateTimeFormat"] ?? _DateTimeFormat;
                return _DateTimeFormat;
            }
        }

        #endregion
    }
}
