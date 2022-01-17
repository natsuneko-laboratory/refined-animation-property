// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the License Zero Parity 7.0.0 (see LICENSE-PARITY file) and MIT (contributions, see LICENSE-MIT file) with exception License Zero Patron 1.0.0 (see LICENSE-PATRON file)
// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions;

using UnityEditorInternal;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class CurveMenuManager : ReflectionClass
    {
        private static readonly Type T;

        static CurveMenuManager()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditor.CurveMenuManager");
        }

        internal CurveMenuManager(object instance) : base(instance, T) { }

        public CurveEditor Updater
        {
            get
            {
                var obj = InvokeField<object>("updater", BindingFlags.NonPublic | BindingFlags.Instance);
                return new CurveEditor(obj);
            }
        }
    }
}