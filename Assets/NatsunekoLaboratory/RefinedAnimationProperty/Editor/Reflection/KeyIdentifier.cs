/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Reflection;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class KeyIdentifier : ReflectionClass
    {
        private static readonly Type T;

        public int Key => InvokeField<int>("key", BindingFlags.Public | BindingFlags.Instance);

        public AnimationCurve Curve
        {
            get => InvokeField<AnimationCurve>("curve", BindingFlags.Public | BindingFlags.Instance);
            set => InvokeField("curve", BindingFlags.Public | BindingFlags.Instance, value);
        }

        public int CurveId => InvokeField<int>("curveId", BindingFlags.Public | BindingFlags.Instance);

        public EditorCurveBinding Binding => InvokeField<EditorCurveBinding>("binding", BindingFlags.Public | BindingFlags.Instance);

        public bool HasNextKeyframe => Curve.length > Key + 1;

        public bool HasPreviousKeyframe => Key > 0;

        static KeyIdentifier()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditor.KeyIdentifier");
        }

        internal KeyIdentifier(object instance) : base(instance, T) { }
    }
}