// CarShop.Areas.HelpPage.ObjectGenerator
using CarShop.Areas.HelpPage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CarShop.Areas.HelpPage
{

    public class ObjectGenerator
    {
        private class SimpleTypeObjectGenerator
        {
            private long _index;

            private static readonly Dictionary<Type, Func<long, object>> DefaultGenerators = InitializeGenerators();

            private static Dictionary<Type, Func<long, object>> InitializeGenerators()
            {
                Dictionary<Type, Func<long, object>> dictionary = new Dictionary<Type, Func<long, object>>();
                dictionary.Add(typeof(bool), (long index) => true);
                dictionary.Add(typeof(byte), (long index) => (byte)64);
                dictionary.Add(typeof(char), (long index) => 'A');
                dictionary.Add(typeof(DateTime), (long index) => DateTime.Now);
                dictionary.Add(typeof(DateTimeOffset), (long index) => new DateTimeOffset(DateTime.Now));
                dictionary.Add(typeof(DBNull), (long index) => DBNull.Value);
                dictionary.Add(typeof(decimal), (long index) => (decimal)index);
                dictionary.Add(typeof(double), (long index) => (double)index + 0.1);
                dictionary.Add(typeof(Guid), (long index) => Guid.NewGuid());
                dictionary.Add(typeof(short), (long index) => (short)(index % 32767));
                dictionary.Add(typeof(int), (long index) => (int)(index % int.MaxValue));
                dictionary.Add(typeof(long), (long index) => index);
                dictionary.Add(typeof(object), (long index) => new object());
                dictionary.Add(typeof(sbyte), (long index) => (sbyte)64);
                dictionary.Add(typeof(float), (long index) => (float)((double)index + 0.1));
                dictionary.Add(typeof(string), (long index) => string.Format(CultureInfo.CurrentCulture, "sample string {0}", new object[1]
                {
                index
                }));
                dictionary.Add(typeof(TimeSpan), (long index) => TimeSpan.FromTicks(1234567L));
                dictionary.Add(typeof(ushort), (long index) => (ushort)(index % 65535));
                dictionary.Add(typeof(uint), (long index) => (uint)(index % 4294967295L));
                dictionary.Add(typeof(ulong), (long index) => (ulong)index);
                dictionary.Add(typeof(Uri), (long index) => new Uri(string.Format(CultureInfo.CurrentCulture, "http://webapihelppage{0}.com", new object[1]
                {
                index
                })));
                return dictionary;
            }

            public static bool CanGenerateObject(Type type)
            {
                return DefaultGenerators.ContainsKey(type);
            }

            public object GenerateObject(Type type)
            {
                return DefaultGenerators[type](++_index);
            }
        }

        private const int DefaultCollectionSize = 3;

        private readonly SimpleTypeObjectGenerator SimpleObjectGenerator = new SimpleTypeObjectGenerator();

        public object GenerateObject(Type type)
        {
            return GenerateObject(type, new Dictionary<Type, object>());
        }

        private object GenerateObject(Type type, Dictionary<Type, object> createdObjectReferences)
        {
            try
            {
                if (SimpleTypeObjectGenerator.CanGenerateObject(type))
                {
                    return SimpleObjectGenerator.GenerateObject(type);
                }
                if (type.IsArray)
                {
                    return GenerateArray(type, 3, createdObjectReferences);
                }
                if (type.IsGenericType)
                {
                    return GenerateGenericType(type, 3, createdObjectReferences);
                }
                if (type == typeof(IDictionary))
                {
                    return GenerateDictionary(typeof(Hashtable), 3, createdObjectReferences);
                }
                if (typeof(IDictionary).IsAssignableFrom(type))
                {
                    return GenerateDictionary(type, 3, createdObjectReferences);
                }
                if (type == typeof(IList) || type == typeof(IEnumerable) || type == typeof(ICollection))
                {
                    return GenerateCollection(typeof(ArrayList), 3, createdObjectReferences);
                }
                if (typeof(IList).IsAssignableFrom(type))
                {
                    return GenerateCollection(type, 3, createdObjectReferences);
                }
                if (type == typeof(IQueryable))
                {
                    return GenerateQueryable(type, 3, createdObjectReferences);
                }
                if (type.IsEnum)
                {
                    return GenerateEnum(type);
                }
                if (type.IsPublic || type.IsNestedPublic)
                {
                    return GenerateComplexObject(type, createdObjectReferences);
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        private static object GenerateGenericType(Type type, int collectionSize, Dictionary<Type, object> createdObjectReferences)
        {
            Type genericTypeDefinition = type.GetGenericTypeDefinition();
            if (genericTypeDefinition == typeof(Nullable<>))
            {
                return GenerateNullable(type, createdObjectReferences);
            }
            if (genericTypeDefinition == typeof(KeyValuePair<,>))
            {
                return GenerateKeyValuePair(type, createdObjectReferences);
            }
            if (IsTuple(genericTypeDefinition))
            {
                return GenerateTuple(type, createdObjectReferences);
            }
            Type[] genericArguments = type.GetGenericArguments();
            if (genericArguments.Length == 1)
            {
                if (genericTypeDefinition == typeof(IList<>) || genericTypeDefinition == typeof(IEnumerable<>) || genericTypeDefinition == typeof(ICollection<>))
                {
                    Type collectionType = typeof(List<>).MakeGenericType(genericArguments);
                    return GenerateCollection(collectionType, collectionSize, createdObjectReferences);
                }
                if (genericTypeDefinition == typeof(IQueryable<>))
                {
                    return GenerateQueryable(type, collectionSize, createdObjectReferences);
                }
                Type type2 = typeof(ICollection<>).MakeGenericType(genericArguments[0]);
                if (type2.IsAssignableFrom(type))
                {
                    return GenerateCollection(type, collectionSize, createdObjectReferences);
                }
            }
            if (genericArguments.Length == 2)
            {
                if (genericTypeDefinition == typeof(IDictionary<,>))
                {
                    Type dictionaryType = typeof(Dictionary<,>).MakeGenericType(genericArguments);
                    return GenerateDictionary(dictionaryType, collectionSize, createdObjectReferences);
                }
                Type type3 = typeof(IDictionary<,>).MakeGenericType(genericArguments[0], genericArguments[1]);
                if (type3.IsAssignableFrom(type))
                {
                    return GenerateDictionary(type, collectionSize, createdObjectReferences);
                }
            }
            if (type.IsPublic || type.IsNestedPublic)
            {
                return GenerateComplexObject(type, createdObjectReferences);
            }
            return null;
        }

        private static object GenerateTuple(Type type, Dictionary<Type, object> createdObjectReferences)
        {
            Type[] genericArguments = type.GetGenericArguments();
            object[] array = new object[genericArguments.Length];
            bool flag = true;
            ObjectGenerator objectGenerator = new ObjectGenerator();
            for (int i = 0; i < genericArguments.Length; i++)
            {
                array[i] = objectGenerator.GenerateObject(genericArguments[i], createdObjectReferences);
                flag &= (array[i] == null);
            }
            if (flag)
            {
                return null;
            }
            return Activator.CreateInstance(type, array);
        }

        private static bool IsTuple(Type genericTypeDefinition)
        {
            if (!(genericTypeDefinition == typeof(Tuple<>)) && !(genericTypeDefinition == typeof(Tuple<,>)) && !(genericTypeDefinition == typeof(Tuple<,,>)) && !(genericTypeDefinition == typeof(Tuple<,,,>)) && !(genericTypeDefinition == typeof(Tuple<,,,,>)) && !(genericTypeDefinition == typeof(Tuple<,,,,,>)) && !(genericTypeDefinition == typeof(Tuple<,,,,,,>)))
            {
                return genericTypeDefinition == typeof(Tuple<,,,,,,,>);
            }
            return true;
        }

        private static object GenerateKeyValuePair(Type keyValuePairType, Dictionary<Type, object> createdObjectReferences)
        {
            Type[] genericArguments = keyValuePairType.GetGenericArguments();
            Type type = genericArguments[0];
            Type type2 = genericArguments[1];
            ObjectGenerator objectGenerator = new ObjectGenerator();
            object obj = objectGenerator.GenerateObject(type, createdObjectReferences);
            object obj2 = objectGenerator.GenerateObject(type2, createdObjectReferences);
            if (obj == null && obj2 == null)
            {
                return null;
            }
            return Activator.CreateInstance(keyValuePairType, obj, obj2);
        }

        private static object GenerateArray(Type arrayType, int size, Dictionary<Type, object> createdObjectReferences)
        {
            Type elementType = arrayType.GetElementType();
            Array array = Array.CreateInstance(elementType, size);
            bool flag = true;
            ObjectGenerator objectGenerator = new ObjectGenerator();
            for (int i = 0; i < size; i++)
            {
                object obj = objectGenerator.GenerateObject(elementType, createdObjectReferences);
                array.SetValue(obj, i);
                flag = (flag && obj == null);
            }
            if (flag)
            {
                return null;
            }
            return array;
        }

        private static object GenerateDictionary(Type dictionaryType, int size, Dictionary<Type, object> createdObjectReferences)
        {
            Type type = typeof(object);
            Type type2 = typeof(object);
            if (dictionaryType.IsGenericType)
            {
                Type[] genericArguments = dictionaryType.GetGenericArguments();
                type = genericArguments[0];
                type2 = genericArguments[1];
            }
            object obj = Activator.CreateInstance(dictionaryType);
            MethodInfo methodInfo = dictionaryType.GetMethod("Add") ?? dictionaryType.GetMethod("TryAdd");
            MethodInfo methodInfo2 = dictionaryType.GetMethod("Contains") ?? dictionaryType.GetMethod("ContainsKey");
            ObjectGenerator objectGenerator = new ObjectGenerator();
            for (int i = 0; i < size; i++)
            {
                object obj2 = objectGenerator.GenerateObject(type, createdObjectReferences);
                if (obj2 == null)
                {
                    return null;
                }
                if (!(bool)methodInfo2.Invoke(obj, new object[1]
                {
                obj2
                }))
                {
                    object obj3 = objectGenerator.GenerateObject(type2, createdObjectReferences);
                    methodInfo.Invoke(obj, new object[2]
                    {
                    obj2,
                    obj3
                    });
                }
            }
            return obj;
        }

        private static object GenerateEnum(Type enumType)
        {
            Array values = Enum.GetValues(enumType);
            if (values.Length > 0)
            {
                return values.GetValue(0);
            }
            return null;
        }

        private static object GenerateQueryable(Type queryableType, int size, Dictionary<Type, object> createdObjectReferences)
        {
            bool isGenericType = queryableType.IsGenericType;
            object obj;
            if (isGenericType)
            {
                Type collectionType = typeof(List<>).MakeGenericType(queryableType.GetGenericArguments());
                obj = GenerateCollection(collectionType, size, createdObjectReferences);
            }
            else
            {
                obj = GenerateArray(typeof(object[]), size, createdObjectReferences);
            }
            if (obj == null)
            {
                return null;
            }
            if (isGenericType)
            {
                Type type = typeof(IEnumerable<>).MakeGenericType(queryableType.GetGenericArguments());
                MethodInfo method = typeof(Queryable).GetMethod("AsQueryable", new Type[1]
                {
                type
                });
                return method.Invoke(null, new object[1]
                {
                obj
                });
            }
            return ((IEnumerable)obj).AsQueryable();
        }

        private static object GenerateCollection(Type collectionType, int size, Dictionary<Type, object> createdObjectReferences)
        {
            Type type = collectionType.IsGenericType ? collectionType.GetGenericArguments()[0] : typeof(object);
            object obj = Activator.CreateInstance(collectionType);
            MethodInfo method = collectionType.GetMethod("Add");
            bool flag = true;
            ObjectGenerator objectGenerator = new ObjectGenerator();
            for (int i = 0; i < size; i++)
            {
                object obj2 = objectGenerator.GenerateObject(type, createdObjectReferences);
                method.Invoke(obj, new object[1]
                {
                obj2
                });
                flag = (flag && obj2 == null);
            }
            if (flag)
            {
                return null;
            }
            return obj;
        }

        private static object GenerateNullable(Type nullableType, Dictionary<Type, object> createdObjectReferences)
        {
            Type type = nullableType.GetGenericArguments()[0];
            ObjectGenerator objectGenerator = new ObjectGenerator();
            return objectGenerator.GenerateObject(type, createdObjectReferences);
        }

        private static object GenerateComplexObject(Type type, Dictionary<Type, object> createdObjectReferences)
        {
            object value = null;
            if (createdObjectReferences.TryGetValue(type, out value))
            {
                return value;
            }
            if (type.IsValueType)
            {
                value = Activator.CreateInstance(type);
            }
            else
            {
                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor == null)
                {
                    return null;
                }
                value = constructor.Invoke(new object[0]);
            }
            createdObjectReferences.Add(type, value);
            SetPublicProperties(type, value, createdObjectReferences);
            SetPublicFields(type, value, createdObjectReferences);
            return value;
        }

        private static void SetPublicProperties(Type type, object obj, Dictionary<Type, object> createdObjectReferences)
        {
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            ObjectGenerator objectGenerator = new ObjectGenerator();
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                if (propertyInfo.CanWrite)
                {
                    object value = objectGenerator.GenerateObject(propertyInfo.PropertyType, createdObjectReferences);
                    propertyInfo.SetValue(obj, value, null);
                }
            }
        }

        private static void SetPublicFields(Type type, object obj, Dictionary<Type, object> createdObjectReferences)
        {
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            ObjectGenerator objectGenerator = new ObjectGenerator();
            FieldInfo[] array = fields;
            foreach (FieldInfo fieldInfo in array)
            {
                object value = objectGenerator.GenerateObject(fieldInfo.FieldType, createdObjectReferences);
                fieldInfo.SetValue(obj, value);
            }
        }
    }
}