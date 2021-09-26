/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Reflection;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions;

using UnityEditor;

using UnityEngine;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal static class AnimationUtilityRefl
    {
        private static readonly Type T;

        static AnimationUtilityRefl()
        {
            T = typeof(AnimationUtility);
        }
        
        public static void UpdateTangentsFromMode(AnimationCurve curve)
        {
            ReflectionStaticClass.InvokeMethod(T, nameof(UpdateTangentsFromMode), BindingFlags.NonPublic | BindingFlags.Static, curve);
        }
    }
}