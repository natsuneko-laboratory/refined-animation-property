/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

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