// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the License Zero Parity 7.0.0 (see LICENSE-PARITY file) and MIT (contributions, see LICENSE-MIT file) with exception License Zero Patron 1.0.0 (see LICENSE-PATRON file)
// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions;

using UnityEditorInternal;

using UnityEngine;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class AnimationWindowState : ReflectionClass
    {
        private static readonly Type T;

        public GameObject ActiveRootGameObject => InvokeProperty<GameObject>("activeRootGameObject", BindingFlags.Public | BindingFlags.Instance);

        public ScriptableObject ActiveScriptableObject => InvokeProperty<ScriptableObject>("activeScriptableObject", BindingFlags.Public | BindingFlags.Instance);

        static AnimationWindowState()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditorInternal.AnimationWindowState");
        }

        public AnimationWindowState(object instance) : base(instance, T) { }
    }
}