// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the License Zero Parity 7.0.0 (see LICENSE-PARITY file) and MIT (contributions, see LICENSE-MIT file) with exception License Zero Patron 1.0.0 (see LICENSE-PATRON file)
// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

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