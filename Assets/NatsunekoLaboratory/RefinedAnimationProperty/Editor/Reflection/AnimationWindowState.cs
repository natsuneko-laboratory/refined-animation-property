/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

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