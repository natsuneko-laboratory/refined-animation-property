// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the License Zero Parity 7.0.0 (see LICENSE-PARITY file) and MIT (contributions, see LICENSE-MIT file) with exception License Zero Patron 1.0.0 (see LICENSE-PATRON file)
// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

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