// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the License Zero Parity 7.0.0 (see LICENSE-PARITY file) and MIT (contributions, see LICENSE-MIT file) with exception License Zero Patron 1.0.0 (see LICENSE-PATRON file)
// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Text.RegularExpressions;

using HarmonyLib;

using NatsunekoLaboratory.RefinedAnimationProperty.Cryptography;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Components
{
    internal class FixAnimationCurveIdGeneration : IEditorComponent
    {
        private static readonly Regex VersionRegex = new Regex("(?<major>\\d+)\\.(?<minor>\\d+)\\.(?<patch>\\d+).*", RegexOptions.Compiled);

        public void Initialize(Harmony harmony)
        {
#if ENABLE_MONO

            if (ShouldApplyPatchInCurrentMonoRuntime())
            {
                Debug.Log($"RefinedAnimationProperty has detected that the editor script is running on old version of Mono runtime, so it has enabled the {nameof(FixAnimationCurveIdGeneration)} feature.");

                {
                    var t = typeof(AssetStore).Assembly.GetType("UnityEditor.EditorCurveBinding");
                    var mOriginal = AccessTools.Method(t, "GetHashCode");
                    var mPrefix = AccessTools.Method(typeof(FixAnimationCurveIdGeneration), nameof(OnEditorCurveBindingGetHashCode));

                    harmony.Patch(mOriginal, new HarmonyMethod(mPrefix));
                }

                {
                    var t = typeof(AssetStore).Assembly.GetType("UnityEditorInternal.AnimationWindowUtility");
                    var mOriginal = AccessTools.Method(t, "GetPropertyNodeID");
                    var mPrefix = AccessTools.Method(typeof(FixAnimationCurveIdGeneration), nameof(OnHandleGetPropertyNodeID));

                    harmony.Patch(mOriginal, new HarmonyMethod(mPrefix));
                }
            }
#endif
        }

        // Versions prior to Mono 6.0.0 use hash algorithm that are prone to collisions, for example, "134-い" and "132-も" will collide.
        // To avoid this, RefinedAnimationProperty used the MurMur3 hash algorithm, which is more collision resistant to the relevant parts.
        private static bool ShouldApplyPatchInCurrentMonoRuntime()
        {
            var t = Type.GetType("Mono.Runtime");
            if (t == null)
                return false;

            var m = t.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);
            var v = m.Invoke(null, null);
            var i = VersionRegex.Match(v.ToString());
            var (major, minor, patch) = (int.Parse(i.Groups["major"].Value), int.Parse(i.Groups["minor"].Value), int.Parse(i.Groups["patch"].Value));

            var hasStrongHashAlgorithm = major >= 6 && minor >= 0 && patch >= 0;
            return !hasStrongHashAlgorithm;
        }

        private static bool OnEditorCurveBindingGetHashCode(ref int __result, EditorCurveBinding __instance)
        {
            var key = $"{__instance.path}:{__instance.type.Name}:{__instance.propertyName}";
            __result = MurMur3.CalcHash(key);

            return false;
        }

        private static bool OnHandleGetPropertyNodeID(ref int __result, ref int setId, ref string path, Type type, ref string propertyName)
        {
            var key = $"{setId + path + type.Name + propertyName}";
            __result = MurMur3.CalcHash(key);

            return false;
        }
    }
}