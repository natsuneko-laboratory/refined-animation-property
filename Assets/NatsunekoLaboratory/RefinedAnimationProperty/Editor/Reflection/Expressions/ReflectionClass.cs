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

using Object = UnityEngine.Object;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions
{
    public class ReflectionClass
    {
        private static readonly Hashtable Caches = new Hashtable();
        private readonly object _instance;
        private readonly Type _type;

        protected Object RawUnityInstance => _instance as Object;

        protected object RawInstance => _instance;

        protected ReflectionClass(object instance, Type type)
        {
            _instance = instance;
            _type = type;

            if (Caches[_type] == null)
                Caches[_type] = new Cache();
        }

        public bool IsAlive()
        {
            return RawUnityInstance != null && RawUnityInstance;
        }

        public bool IsValid()
        {
            if (_instance is Object)
                return IsAlive() && RawUnityInstance.GetType() == _type;
            return _instance.GetType() == _type;
        }

        protected TResult InvokeMethod<TResult>(string name, BindingFlags flags, params object[] parameters)
        {
            var methods = ((Cache)Caches[_type]).Methods;
            methods.TryGetValue(name, out var cache);

            if (cache != null)
                return (TResult)cache.Invoke(_instance, parameters);

            var mi = _instance.GetType().GetMethod(name, flags | BindingFlags.Instance);
            if (mi == null)
                throw new InvalidOperationException($"Method {name} is not found in this instance");

            methods.Add(name, CreateMethodAccessor(mi));
            return (TResult)methods[name].Invoke(_instance, parameters);
        }

        protected void InvokeMethod(string name, BindingFlags flags, params object[] parameters)
        {
            var methods = ((Cache)Caches[_type]).VoidMethods;
            methods.TryGetValue(name, out var cache);

            if (cache != null)
            {
                cache.Invoke(_instance, parameters);
                return;
            }

            var mi = _instance.GetType().GetMethod(name, flags | BindingFlags.Instance);
            if (mi == null)
                throw new InvalidOperationException($"Method {name} is not found in this instance");

            methods.Add(name, CreateVoidMethodAccessor(mi));
            methods[name].Invoke(_instance, parameters);
        }

        protected TResult InvokeField<TResult>(string name, BindingFlags flags)
        {
            var members = ((Cache)Caches[_type]).Members;
            members.TryGetValue(name, out var cache);

            if (cache != null)
                return (TResult)cache.Invoke(_instance);

            var fi = _instance.GetType().GetField(name, flags | BindingFlags.Instance);
            if (fi == null)
                throw new InvalidOperationException($"Field {name} is not found in this instance");

            members.Add(name, CreateFieldAccessor(fi));
            return (TResult)members[name].Invoke(_instance);
        }

        protected TResult InvokeProperty<TResult>(string name, BindingFlags flags)
        {
            var members = ((Cache)Caches[_type]).Members;
            members.TryGetValue(name, out var cache);

            if (cache != null)
                return (TResult)cache.Invoke(_instance);

            var fi = _instance.GetType().GetProperty(name, flags | BindingFlags.Instance);
            if (fi == null)
                throw new InvalidOperationException($"Property {name} is not found in this instance");

            members.Add(name, CreatePropertyAccessor(fi));
            return (TResult)members[name].Invoke(_instance);
        }

        private Func<object, object[], object> CreateMethodAccessor(MethodInfo mi)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var args = Expression.Parameter(typeof(object[]), "args");
            var body = mi.GetParameters().Length == 0
                ? Expression.Call(Expression.Convert(instance, _type), mi)
                : Expression.Call(Expression.Convert(instance, _type), mi, mi.GetParameters().Select((w, i) => Expression.Convert(Expression.ArrayIndex(args, Expression.Constant(i)), w.ParameterType)).Cast<Expression>().ToArray());

            return Expression.Lambda<Func<object, object[], object>>(Expression.Convert(body, typeof(object)), instance, args).Compile();
        }

        private Action<object, object[]> CreateVoidMethodAccessor(MethodInfo mi)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var args = Expression.Parameter(typeof(object[]), "args");
            var body = mi.GetParameters().Length == 0
                ? Expression.Call(Expression.Convert(instance, _type), mi)
                : Expression.Call(Expression.Convert(instance, _type), mi, mi.GetParameters().Select((w, i) => Expression.Convert(Expression.ArrayIndex(args, Expression.Constant(i)), w.ParameterType)).Cast<Expression>().ToArray());

            return Expression.Lambda<Action<object, object[]>>(Expression.Convert(body, typeof(void)), instance, args).Compile();
        }

        private Func<object, object> CreateFieldAccessor(FieldInfo fi)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var body = Expression.Field(Expression.Convert(instance, _type), fi);

            return Expression.Lambda<Func<object, object>>(Expression.Convert(body, typeof(object)), instance).Compile();
        }

        private Func<object, object> CreatePropertyAccessor(PropertyInfo pi)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var body = Expression.Property(Expression.Convert(instance, _type), pi);

            return Expression.Lambda<Func<object, object>>(Expression.Convert(body, typeof(object)), instance).Compile();
        }

        private class Cache
        {
            public readonly Dictionary<string, Func<object, object>> Members = new Dictionary<string, Func<object, object>>();
            public readonly Dictionary<string, Func<object, object[], object>> Methods = new Dictionary<string, Func<object, object[], object>>();
            public readonly Dictionary<string, Action<object, object[]>> VoidMethods = new Dictionary<string, Action<object, object[]>>();
        }
    }
}