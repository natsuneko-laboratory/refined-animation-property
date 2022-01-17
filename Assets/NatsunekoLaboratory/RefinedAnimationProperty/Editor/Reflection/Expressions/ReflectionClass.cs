// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the License Zero Parity 7.0.0 (see LICENSE-PARITY file) and MIT (contributions, see LICENSE-MIT file) with exception License Zero Patron 1.0.0 (see LICENSE-PATRON file)
// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

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
        private readonly Type _type;

        protected Object RawUnityInstance => RawInstance as Object;

        protected object RawInstance { get; }

        protected ReflectionClass(object instance, Type type, bool isStrict = false)
        {
            RawInstance = instance;
            _type = type;

            if (Caches[_type] == null)
                Caches[_type] = new Cache();

            if (isStrict)
            {
                if (IsValid())
                    return;

                throw new InvalidOperationException();
            }
        }

        public bool IsAlive()
        {
            return RawUnityInstance != null && RawUnityInstance;
        }

        public bool IsValid()
        {
            if (RawInstance is Object)
                return IsAlive() && RawUnityInstance.GetType() == _type;
            return RawInstance.GetType() == _type;
        }

        protected TResult InvokeMethod<TResult>(string name, BindingFlags flags, params object[] parameters)
        {
            var methods = ((Cache)Caches[_type]).Methods;
            methods.TryGetValue(name, out var cache);

            if (cache != null)
                return (TResult)cache.Invoke(RawInstance, parameters);

            var mi = RawInstance.GetType().GetMethod(name, flags | BindingFlags.Instance);
            if (mi == null)
                throw new InvalidOperationException($"Method {name} is not found in this instance");

            methods.Add(name, CreateMethodAccessor(mi));
            return (TResult)methods[name].Invoke(RawInstance, parameters);
        }

        protected void InvokeMethod(string name, BindingFlags flags, params object[] parameters)
        {
            var methods = ((Cache)Caches[_type]).VoidMethods;
            methods.TryGetValue(name, out var cache);

            if (cache != null)
            {
                cache.Invoke(RawInstance, parameters);
                return;
            }

            var mi = RawInstance.GetType().GetMethod(name, flags | BindingFlags.Instance);
            if (mi == null)
                throw new InvalidOperationException($"Method {name} is not found in this instance");

            methods.Add(name, CreateVoidMethodAccessor(mi));
            methods[name].Invoke(RawInstance, parameters);
        }

        protected void InvokeMethodStrict(string name, BindingFlags flags, params StrictParameter[] parameters)
        {
            var methods = ((Cache)Caches[_type]).VoidMethods;
            methods.TryGetValue(name, out var cache);

            if (cache != null)
            {
                cache.Invoke(RawInstance, parameters.Select(w => w.Value).ToArray());
                return;
            }

            var mi = RawInstance.GetType().GetMethod(name, flags | BindingFlags.Instance, null, parameters.Select(w => w.Type).ToArray(), null);
            if (mi == null)
                throw new InvalidOperationException($"Method {name} is not found in this instance");

            methods.Add(name, CreateVoidMethodAccessor(mi));
            methods[name].Invoke(RawInstance, parameters.Select(w => w.Value).ToArray());
        }

        protected TResult InvokeField<TResult>(string name, BindingFlags flags)
        {
            var members = ((Cache)Caches[_type]).MemberGetters;
            members.TryGetValue(name, out var cache);

            if (cache != null)
                return (TResult)cache.Invoke(RawInstance);

            var fi = RawInstance.GetType().GetField(name, flags | BindingFlags.Instance);
            if (fi == null)
                throw new InvalidOperationException($"Field {name} is not found in this instance");

            members.Add(name, CreateFieldGetAccessor(fi));
            return (TResult)members[name].Invoke(RawInstance);
        }

        protected void InvokeField<TValue>(string name, BindingFlags flags, TValue value)
        {
            var members = ((Cache)Caches[_type]).MemberSetters;
            members.TryGetValue(name, out var cache);

            if (cache != null)
            {
                cache.Invoke(RawInstance, value);
                return;
            }

            var fi = RawInstance.GetType().GetField(name, flags | BindingFlags.Instance);
            if (fi == null)
                throw new InvalidOperationException($"Field {name} is not found in this instance");

            members.Add(name, CreateFieldSetAccessor<TValue>(fi));
            members[name].Invoke(RawInstance, value);
        }

        protected TResult InvokeProperty<TResult>(string name, BindingFlags flags)
        {
            var members = ((Cache)Caches[_type]).MemberGetters;
            members.TryGetValue(name, out var cache);

            if (cache != null)
                return (TResult)cache.Invoke(RawInstance);

            var fi = RawInstance.GetType().GetProperty(name, flags | BindingFlags.Instance);
            if (fi == null)
                throw new InvalidOperationException($"Property {name} is not found in this instance");

            members.Add(name, CreatePropertyGetAccessor(fi));
            return (TResult)members[name].Invoke(RawInstance);
        }

        protected void InvokeProperty<TValue>(string name, BindingFlags flags, TValue value)
        {
            var members = ((Cache)Caches[_type]).MemberSetters;
            members.TryGetValue(name, out var cache);

            if (cache != null)
            {
                cache.Invoke(RawInstance, value);
                return;
            }

            var fi = RawInstance.GetType().GetProperty(name, flags | BindingFlags.Instance);
            if (fi == null)
                throw new InvalidOperationException($"Property {name} is not found in this instance");

            members.Add(name, CreatePropertySetAccessor(typeof(TValue), fi));
            members[name].Invoke(RawInstance, value);
        }

        protected void InvokeProperty(string name, BindingFlags flags, Type t, object value)
        {
            var members = ((Cache)Caches[_type]).MemberSetters;
            members.TryGetValue(name, out var cache);

            if (cache != null)
            {
                cache.Invoke(RawInstance, value);
                return;
            }

            var fi = RawInstance.GetType().GetProperty(name, flags | BindingFlags.Instance);
            if (fi == null)
                throw new InvalidOperationException($"Property {name} is not found in this instance");

            members.Add(name, CreatePropertySetAccessor(t, fi));
            members[name].Invoke(RawInstance, value);
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

        private Func<object, object> CreateFieldGetAccessor(FieldInfo fi)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var body = Expression.Field(Expression.Convert(instance, _type), fi);

            return Expression.Lambda<Func<object, object>>(Expression.Convert(body, typeof(object)), instance).Compile();
        }

        private Action<object, object> CreateFieldSetAccessor<TValue>(FieldInfo fi)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var value = Expression.Parameter(typeof(object), "value");
            var body = Expression.Assign(Expression.Field(Expression.Convert(instance, _type), fi), Expression.Convert(value, typeof(TValue)));

            return Expression.Lambda<Action<object, object>>(Expression.Convert(body, typeof(object)), instance, value).Compile();
        }

        private Func<object, object> CreatePropertyGetAccessor(PropertyInfo pi)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var body = Expression.Property(Expression.Convert(instance, _type), pi);

            return Expression.Lambda<Func<object, object>>(Expression.Convert(body, typeof(object)), instance).Compile();
        }

        private Action<object, object> CreatePropertySetAccessor(Type t, PropertyInfo pi)
        {
            var instance = Expression.Parameter(typeof(object), "instance");

            if (t.IsArray)
            {
                var value = Expression.Parameter(typeof(object), "value");
                var e = t.GetElementType() ?? throw new InvalidOperationException();
                var w = Expression.Parameter(typeof(object), "w");
                var m = typeof(Array).GetMethods().First(v => v.Name == "ConvertAll").MakeGenericMethod(typeof(object), e);
                var c = typeof(Converter<,>).MakeGenericType(typeof(object), e);

                // ReSharper disable once AssignNullToNotNullAttribute
                var converter = Expression.Lambda(c, Expression.Convert(w, e), w); // (w) => (SomeType) w;
                var calling = Expression.Call(m, Expression.Convert(value, typeof(object[])), converter);
                var body = Expression.Assign(Expression.Property(Expression.Convert(instance, _type), pi), calling);
                return Expression.Lambda<Action<object, object>>(Expression.Convert(body, typeof(object)), instance, value).Compile();
            }
            else
            {
                var value = Expression.Parameter(typeof(object), "value");
                var body = Expression.Assign(Expression.Property(Expression.Convert(instance, _type), pi), Expression.Convert(value, t));
                return Expression.Lambda<Action<object, object>>(Expression.Convert(body, typeof(object)), instance, value).Compile();
            }
        }

        private class Cache
        {
            public readonly Dictionary<string, Func<object, object>> MemberGetters = new Dictionary<string, Func<object, object>>();
            public readonly Dictionary<string, Action<object, object>> MemberSetters = new Dictionary<string, Action<object, object>>();
            public readonly Dictionary<string, Func<object, object[], object>> Methods = new Dictionary<string, Func<object, object[], object>>();
            public readonly Dictionary<string, Action<object, object[]>> VoidMethods = new Dictionary<string, Action<object, object[]>>();
        }

        protected class StrictParameter
        {
            public object Value { get; set; }

            public Type Type { get; set; }
        }
    }
}