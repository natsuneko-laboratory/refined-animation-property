// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the License Zero Parity 7.0.0 (see LICENSE-PARITY file) and MIT (contributions, see LICENSE-MIT file) with exception License Zero Patron 1.0.0 (see LICENSE-PATRON file)
// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions;

using UnityEditorInternal;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class CurveEditor : ReflectionClass
    {
        private static readonly Type T;

        static CurveEditor()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditor.CurveEditor");
        }

        internal CurveEditor(object instance) : base(instance, T) { }

        public void UpdateCurves(List<ChangedCurve> curve, string undoText)
        {
            var t = typeof(List<>).MakeGenericType(typeof(AssetStore).Assembly.GetType("UnityEditor.ChangedCurve"));
            var i = Activator.CreateInstance(t);
            foreach (var c in curve)
                ((IList)i).Add(c.Original);

            var first = new StrictParameter { Type = t, Value = i };
            var second = new StrictParameter { Type = typeof(string), Value = undoText };

            InvokeMethodStrict(nameof(UpdateCurves), BindingFlags.Public | BindingFlags.Instance, first, second);
        }
    }
}