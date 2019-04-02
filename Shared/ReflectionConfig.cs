using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Shared
{
    public abstract class ReflectionConfig
    {
        [AttributeUsage(AttributeTargets.Property)]
        public sealed class IgnoreForReflectionConfig : Attribute
        {

        }

        [AttributeUsage(AttributeTargets.Property)]
        public sealed class ConnectionStringReflectionConfig : Attribute
        {

        }

        protected ReflectionConfig()
        {
            Configure();
        }


        /// <summary>
        /// Configures this instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException"></exception>
        private void Configure()
        {
            Type configType = GetType();
            PropertyInfo[] properties = configType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            Type typeOfIgnoreForReflectionConfig = typeof(IgnoreForReflectionConfig);
            Type typeOfConnectionStringReflectionConfig = typeof(ConnectionStringReflectionConfig);

            foreach (PropertyInfo property in properties)
            {
                bool ignore = property.GetCustomAttribute(typeOfIgnoreForReflectionConfig) != null;
                bool isConnectionString = property.GetCustomAttribute(typeOfConnectionStringReflectionConfig) != null;

                if (!ignore)
                {
                    string configValue = null;

                    if (isConnectionString)
                    {
                        configValue = ConfigurationManager.ConnectionStrings[property.Name].ToString();
                    }
                    else
                    {
                        configValue = ConfigurationManager.AppSettings[property.Name];
                    }

                    if (configValue != null)
                    {
                        var type = Type.GetType(property.PropertyType.AssemblyQualifiedName);

                        if (type.IsGenericType || type.IsArray)
                        {
                            Type elementType = null;
                            MethodInfo methodDefinition = null;

                            if (type.IsArray)
                            {
                                elementType = type.GetElementType();
                                methodDefinition = typeof(ReflectionConfig).GetMethod(nameof(ParseToArray));
                            }
                            else if (type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                            {
                                elementType = type.GetGenericArguments().SingleOrDefault();
                                methodDefinition = typeof(ReflectionConfig).GetMethod(nameof(ParseToEnumerable));
                            }

                            if (elementType == null)
                            {
                                throw new InvalidOperationException($"elementType for {property.Name} is NULL, could not resolve the element type");
                            }

                            MethodInfo method = methodDefinition.MakeGenericMethod(elementType);
                            property.SetValue(this, method.Invoke(null, new object[] { configValue }));
                        }
                        else if(type.IsEnum)
                        {
                            property.SetValue(this, Enum.Parse(type, configValue));
                        }
                        else
                        {
                            var typedConfigValue = Convert.ChangeType(configValue, type);
                            property.SetValue(this, typedConfigValue);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Parses string to array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueString">The value string.</param>
        /// <param name="split">The split.</param>
        /// <returns></returns>
        public static T[] ParseToArray<T>(string valueString, char split = ',')
        {
            return valueString.Split(split)
                              .Select(s => GetValue<T>(s))
                              .ToArray();
        }


        /// <summary>
        /// Parses string to enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueString">The value string.</param>
        /// <param name="split">The split.</param>
        /// <returns></returns>
        public static IEnumerable<T> ParseToEnumerable<T>(string valueString, char split = ',')
        {
            return valueString.Split(split)
                              .Select(s => GetValue<T>(s))
                              .ToList();
        }


        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static T GetValue<T>(object value)
        {
            if (value == null)
            {
                return default(T);
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }
    }
}