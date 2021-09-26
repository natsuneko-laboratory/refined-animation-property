/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class ChangedCurve : ReflectionClass
    {
        private static readonly Type T;

        static ChangedCurve()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditor.ChangedCurve");
        }

        private ChangedCurve(object instance) : base(instance, T) { }

        public static ChangedCurve Instantiate(AnimationCurve curve, int curveId, EditorCurveBinding binding)
        {
            var obj = Activator.CreateInstance(T, curve, curveId, binding);
            return new ChangedCurve(obj);
        }

        public object Original => RawInstance;
    }
}