/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System.Collections.Generic;

using HarmonyLib;

using NatsunekoLaboratory.RefinedAnimationProperty.Components;

using UnityEditor;

namespace NatsunekoLaboratory.RefinedAnimationProperty
{
    public class RefinedAnimationProperty
    {
        private static readonly List<IEditorComponent> RefinedAnimationPropertyComponents = new List<IEditorComponent>()
        {
            new SearchableAddPropertyPopup(),
            new SupportToEasingFunctionsInCurveEditor(),
            new FixAnimationCurveIdGeneration()
        };

        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            var harmony = new Harmony("refined-animation-property");

            foreach (var feature in RefinedAnimationPropertyComponents) 
                feature.Initialize(harmony);
        }
    }
}