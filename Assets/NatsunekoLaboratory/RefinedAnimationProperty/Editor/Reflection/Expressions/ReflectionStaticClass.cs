/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions
{
    public static class ReflectionStaticClass
    {
        private static readonly Hashtable Caches = new Hashtable();

        internal static TResult InvokeMethod<TResult>(Type @class, string name, BindingFlags bindingFlags, params object[] parameters)
        {
            var cache = SafeCacheMethodAccess(@class, name);
            if (cache != null)
                return (TResult)cache.Invoke(parameters);

            var mi = @class.GetMethod(name, bindingFlags | BindingFlags.Static);
            if (mi == null)
                throw new InvalidOperationException($"Method '{name}' is not found in this class");

            ((Cache)Caches[@class]).Methods.Add(name, CreateMethodAccessor(mi));
            return (TResult)((Cache)Caches[@class]).Methods[name].Invoke(parameters);
        }

        internal static void InvokeMethod(Type @class, string name, BindingFlags bindingFlags, params object[] parameters)
        {
            var cache = SafeCacheVoidMethodAccess(@class, name);
            if (cache != null)
            {
                cache.Invoke(parameters);
                return;
            }

            var mi = @class.GetMethod(name, bindingFlags | BindingFlags.Static);
            if (mi == null)
                throw new InvalidOperationException($"Method '{name}' is not found in this class");

            ((Cache)Caches[@class]).VoidMethods.Add(name, CreateVoidMethodAccessor(mi));
            ((Cache)Caches[@class]).VoidMethods[name].Invoke(parameters);
        }

        internal static TResult InvokeField<TResult>(Type @class, string name, BindingFlags bindingFlags)
        {
            var cache = SafeCacheFieldAccess(@class, name);
            if (cache != null)
                return (TResult)cache.Invoke();

            var fi = @class.GetField(name, bindingFlags | BindingFlags.Static);
            if (fi == null)
                throw new InvalidOperationException($"Field '{name}' is not found in this class");

            ((Cache)Caches[@class]).Fields.Add(name, CreateFieldAccessor(fi));
            return (TResult)((Cache)Caches[@class]).Fields[name].Invoke();
        }

        private static Func<object[], object> CreateMethodAccessor(MethodInfo mi)
        {
            var args = Expression.Parameter(typeof(object[]), "args");
            var body = mi.GetParameters().Length == 0
                ? Expression.Call(null, mi)
                : Expression.Call(null, mi, mi.GetParameters().Select((w, i) => Expression.Convert(Expression.ArrayIndex(args, Expression.Constant(i)), w.ParameterType)).Cast<Expression>().ToArray());

            return Expression.Lambda<Func<object[], object>>(Expression.Convert(body, typeof(object)), args).Compile();
        }

        private static Action<object[]> CreateVoidMethodAccessor(MethodInfo mi)
        {
            var args = Expression.Parameter(typeof(object[]), "args");
            var body = mi.GetParameters().Length == 0
                ? Expression.Call(null, mi)
                : Expression.Call(null, mi, mi.GetParameters().Select((w, i) => Expression.Convert(Expression.ArrayIndex(args, Expression.Constant(i)), w.ParameterType)).Cast<Expression>().ToArray());

            return Expression.Lambda<Action<object[]>>(body, args).Compile();
        }

        private static Func<object> CreateFieldAccessor(FieldInfo fi)
        {
            return Expression.Lambda<Func<object>>(Expression.Convert(Expression.Field(null, fi), typeof(object))).Compile();
        }

        private static Func<object[], object> SafeCacheMethodAccess(Type type, string name)
        {
            if (Caches[type] == null)
            {
                Caches[type] = new Cache();
                return null;
            }

            ((Cache)Caches[type]).Methods.TryGetValue(name, out var cache);
            return cache;
        }

        private static Action<object[]> SafeCacheVoidMethodAccess(Type type, string name)
        {
            if (Caches[type] == null)
            {
                Caches[type] = new Cache();
                return null;
            }

            ((Cache)Caches[type]).VoidMethods.TryGetValue(name, out var cache);
            return cache;
        }

        private static Func<object> SafeCacheFieldAccess(Type type, string name)
        {
            if (Caches[type] == null)
            {
                Caches[type] = new Cache();
                return null;
            }

            ((Cache)Caches[type]).Fields.TryGetValue(name, out var cache);
            return cache;
        }

        private class Cache
        {
            public readonly Dictionary<string, Func<object>> Fields = new Dictionary<string, Func<object>>();
            public readonly Dictionary<string, Func<object[], object>> Methods = new Dictionary<string, Func<object[], object>>();
            public readonly Dictionary<string, Action<object[]>> VoidMethods = new Dictionary<string, Action<object[]>>();
        }
    }
}