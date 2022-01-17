// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the License Zero Parity 7.0.0 (see LICENSE-PARITY file) and MIT (contributions, see LICENSE-MIT file) with exception License Zero Patron 1.0.0 (see LICENSE-PATRON file)
// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

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