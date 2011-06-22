/*
Copyright 2010 Google Inc

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Google.Apis.Util
{
    /// <summary>
    /// A class containing utility methods
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Fetches an element from a dictionary in a safe way, returning null if there is no value present.
        /// </summary>
        public static TValue GetValueAsNull<TKey, TValue>(this IDictionary<TKey, TValue> data, TKey key)
        {
            TValue result;
            if (!data.TryGetValue(key, out result))
            {
                return default(TValue);
            }
            return result;
        }

        /// <summary>
        /// If key exists in data and is an IEnumerable will return each element converted to a string 
        /// with ToString, or null. If key does not exist in data, is null or not an IEnumerable we will 
        /// return the empty list.
        /// </summary>
        public static IEnumerable<string> GetValueAsStringListOrEmpty<TKey, TValue>(this IDictionary<TKey, TValue> data,
                                                                                    TKey key)
        {
            data.ThrowIfNull("data");
            IEnumerable value = GetValueAsNull(data, key) as IEnumerable;
            if (value == null)
            {
                yield break;
            }
            foreach (object obj in value)
            {
                yield return obj != null ? obj.ToString() : null;
            }
        }

        /// <summary>Extension method on object, which throws a ArgumentNullException if obj is null</summary>
        public static void ThrowIfNull(this object obj, string paramName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Throws an exception if the string is null or empty
        /// </summary>
        public static void ThrowIfNullOrEmpty(this string str, string paramName)
        {
            str.ThrowIfNull(paramName);
            if (str.Length == 0)
            {
                throw new ArgumentException("Parameter was empty", paramName);
            }
        }

        /// <summary>
        /// Throws the collection is null or empty
        /// </summary>
        public static void ThrowIfNullOrEmpty<T>(this ICollection<T> coll, string paramName)
        {
            coll.ThrowIfNull(paramName);
            if (coll.Count == 0)
            {
                throw new ArgumentException("Parameter was empty", paramName);
            }
        }

        /// <summary>
        /// Returns a readonly variant of the given dictionary
        /// </summary>
        public static IDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            dict.ThrowIfNull("this");
            return new ReadOnlyDictionary<TKey, TValue>(dict);
        }

        /// <summary>
        /// Returns true when the string is null or empty
        /// </summary>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Returns true when the string is NOT null or empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Returns true when the collection is NOT null or empty
        /// </summary>
        public static bool IsNotNullOrEmpty<T>(this ICollection<T> coll)
        {
            return coll != null && coll.Count > 0;
        }

        /// <summary>
        /// Returns true when the enumerable is null or empty.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> coll)
        {
            return coll == null || coll.Count() == 0;
        }

        /// <summary>
        /// Returns true when the collection is null or empty
        /// </summary>
        public static bool IsNullOrEmpty<T>(this ICollection<T> coll)
        {
            return coll == null || coll.Count == 0;
        }

        /// <summary>
        /// Returns the first <see cref="System.CodeDom.CodeMemberProperty"/> with a 
        /// name that matches the passed in name - or null if no match is found.
        /// </summary>
        /// <param name="coll">May not be null</param>
        /// <param name="name">May not be null or empty</param>
        public static CodeMemberField FindFieldByName(this CodeTypeMemberCollection coll, string name)
        {
            coll.ThrowIfNull("coll");
            name.ThrowIfNullOrEmpty(name);

            foreach (CodeTypeMember member in coll)
            {
                if ((member is CodeMemberField) == false)
                {
                    continue;
                }
                var field = (CodeMemberField) member;
                if (field.Name == name)
                {
                    return field;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the first <see cref="System.CodeDom.CodeTypeMember"/> with a 
        /// name that matches the passed in name - or null if no match is found.
        /// </summary>
        /// <param name="coll">May not be null</param>
        /// <param name="name">May not be null or empty</param>
        public static CodeTypeMember FindMemberByName(this CodeTypeMemberCollection coll, string name)
        {
            coll.ThrowIfNull("coll");
            name.ThrowIfNullOrEmpty(name);

            foreach (CodeTypeMember member in coll)
            {
                if (member.Name == name)
                {
                    return member;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the first <see cref="System.CodeDom.CodeTypeDeclaration"/> with a 
        /// name that matches the passed in name - or null if no match is found.
        /// </summary>
        /// <param name="coll">May not be null</param>
        /// <param name="name">May not be null or empty</param>
        public static CodeTypeDeclaration FindTypeMemberByName(this CodeTypeMemberCollection coll, string name)
        {
            coll.ThrowIfNull("coll");
            name.ThrowIfNullOrEmpty(name);

            foreach (CodeTypeMember member in coll)
            {
                var field = member as CodeTypeDeclaration;
                if (field != null && field.Name == name)
                {
                    return field;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the first <see cref="System.CodeDom.CodeMemberProperty"/> with a 
        /// name that matches the passed in name - or Null if no match found.
        /// </summary>
        /// <param name="coll">May not be null</param>
        /// <param name="name">May not be null or empty</param>
        public static CodeMemberProperty FindPropertyByName(this CodeTypeMemberCollection coll, string name)
        {
            coll.ThrowIfNull("coll");
            name.ThrowIfNullOrEmpty(name);

            foreach (CodeTypeMember member in coll)
            {
                if ((member is CodeMemberProperty) == false)
                {
                    continue;
                }
                var property = (CodeMemberProperty) member;
                if (property.Name == name)
                {
                    return property;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the first matching custom attribute (or null) of the specified member
        /// </summary>
        public static T GetCustomAttribute<T>(this MemberInfo info) where T : Attribute
        {
            object[] results = info.GetCustomAttributes(typeof(T), false);
            return results.Length == 0 ? null : (T) results[0];
        }

        /// <summary>
        /// Returns the defined string value of an enum value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            FieldInfo entry = value.GetType().GetField(value.ToString());
            entry.ThrowIfNull("value");

            // If set, return the value
            var attribute = entry.GetCustomAttribute<StringValueAttribute>();
            if (attribute != null)
            {
                return attribute.Text;
            }

            // Otherwise throw an exception
            throw new ArgumentException(
                string.Format("Enum value '{0}' does not contain a StringValue attribute", entry), "value");
        }

        /// <summary>
        /// Retrieves the underlying type if the specified type is a Nullable, or the type itself otherwise.
        /// </summary>
        public static Type GetNonNullableType(Type type)
        {
            if (!type.IsGenericType || !typeof(Nullable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                return type; // Not a Nullable.
            }

            // First: Find the Nullable type.
            while (!(type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))))
            {
                type = type.BaseType;

                if (type == null)
                {
                    return null;
                }
            }

            // Return the type which is encapsulated by the Nullable.
            return type.GetGenericArguments()[0];
        }

        /// <summary>
        /// Tries to convert the specified object to a string. Uses custom type converters if available.
        /// Returns null for a null object.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ConvertToString(object o)
        {
            if (o == null)
            {
                return null;
            }

            // Get the type converter
            TypeConverter converter = TypeDescriptor.GetConverter(o);
            return converter.ConvertToString(o);
        }
        
        /// <summary>
        /// Please don't use this unless absolutely nessasery.
        /// Returns if the current runtime is Mono, this is used to work around different behavior in the runtime.
        /// </summary>
        public static bool IsMonoRuntime()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}