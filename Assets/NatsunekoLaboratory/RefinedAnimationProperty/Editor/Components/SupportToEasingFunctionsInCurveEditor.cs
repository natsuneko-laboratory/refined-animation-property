// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the License Zero Parity 7.0.0 (see LICENSE-PARITY file) and MIT (contributions, see LICENSE-MIT file) with exception License Zero Patron 1.0.0 (see LICENSE-PATRON file)
// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using HarmonyLib;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Components
{
    internal class SupportToEasingFunctionsInCurveEditor : IEditorComponent
    {
        private const float C1 = 1.70158f;
        private const float C2 = C1 * 1.525f;
        private const float C3 = C1 + 1f;
        private const float C4 = 2 * Mathf.PI / 3f;
        private const float C5 = 2 * Mathf.PI / 4.5f;
        private const float N1 = 7.5625f;
        private const float D1 = 2.75f;
        private const float Epsilon = 0.0001f;

        public void Initialize(Harmony harmony)
        {
            var t1 = typeof(AssetStore).Assembly.GetType("UnityEditor.CurveMenuManager");
            var mOriginal1 = AccessTools.Method(t1, "AddTangentMenuItems");
            var mPostfix1 = AccessTools.Method(typeof(SupportToEasingFunctionsInCurveEditor), nameof(OnHandleAddTangentMenuItems));

            harmony.Patch(mOriginal1, null, new HarmonyMethod(mPostfix1));
        }

        // ReSharper disable once InconsistentNaming
        private static void OnHandleAddTangentMenuItems(object __instance, GenericMenu menu, List<object> keyList)
        {
            var manager = new CurveMenuManager(__instance);
            if (!manager.Updater.IsValid())
                return;

            var keyIdentifiers = keyList.Select(w => new KeyIdentifier(w)).ToList();
            var isValid = keyIdentifiers.Count > 0;
            var isLeftValid = isValid;
            var isRightValid = isValid;

            foreach (var identifier in keyIdentifiers)
            {
                isLeftValid &= identifier.HasPreviousKeyframe;
                isRightValid &= identifier.HasNextKeyframe;
            }

            menu.AddSeparator("");
            if (isLeftValid)
            {
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Sine"), false, SetLeftEaseInSine, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-Out-Sine"), false, SetLeftEaseOutSine, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Sine"), false, SetLeftEaseInOutSine, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Quad"), false, SetLeftEaseInQuad, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-Out-Quad"), false, SetLeftEaseOutQuad, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Quad"), false, SetLeftEaseInOutQuad, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Cubic"), false, SetLeftEaseInCubic, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-Out-Cubic"), false, SetLeftEaseOutCubic, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Cubic"), false, SetLeftEaseInOutCubic, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Quart"), false, SetLeftEaseInQuart, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-Out-Quart"), false, SetLeftEaseOutQuart, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Quart"), false, SetLeftEaseInOutQuart, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Quint"), false, SetLeftEaseInQuint, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-Out-Quint"), false, SetLeftEaseOutQuint, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Quint"), false, SetLeftEaseInOutQuint, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Expo"), false, SetLeftEaseInExpo, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-Out-Expo"), false, SetLeftEaseOutExpo, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Expo"), false, SetLeftEaseInOutExpo, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Circ"), false, SetLeftEaseInCirc, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-Out-Circ"), false, SetLeftEaseOutCirc, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Circ"), false, SetLeftEaseInOutCirc, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Back"), false, SetLeftEaseInBack, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-Out-Back"), false, SetLeftEaseOutBack, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Back"), false, SetLeftEaseInOutBack, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Elastic"), false, SetLeftEaseInElastic, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-Out-Elastic"), false, SetLeftEaseOutElastic, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Elastic"), false, SetLeftEaseInOutElastic, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Bounce"), false, SetLeftEaseInBounce, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-Out-Bounce"), false, SetLeftEaseOutBounce, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Bounce"), false, SetLeftEaseInOutBounce, new object[] { manager, keyIdentifiers });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Sine"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-Out-Sine"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Sine"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Quad"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-Out-Quad"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Quad"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Cubic"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-Out-Cubic"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Cubic"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Quart"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-Out-Quart"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Quart"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Quint"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-Out-Quint"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Quint"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Expo"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-Out-Expo"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Expo"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Circ"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-Out-Circ"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Circ"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Back"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-Out-Back"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Back"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Elastic"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-Out-Elastic"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Elastic"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Bounce"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-Out-Bounce"));
                menu.AddDisabledItem(new GUIContent("Left Tangent Easing/Ease-In-Out-Bounce"));
            }

            if (isRightValid)
            {
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Sine"), false, SetRightEaseInSine, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-Out-Sine"), false, SetRightEaseOutSine, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Sine"), false, SetRightEaseInOutSine, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Quad"), false, SetRightEaseInQuad, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-Out-Quad"), false, SetRightEaseOutQuad, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Quad"), false, SetRightEaseInOutQuad, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Cubic"), false, SetRightEaseInCubic, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-Out-Cubic"), false, SetRightEaseOutCubic, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Cubic"), false, SetRightEaseInOutCubic, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Quart"), false, SetRightEaseInQuart, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-Out-Quart"), false, SetRightEaseOutQuart, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Quart"), false, SetRightEaseInOutQuart, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Quint"), false, SetRightEaseInQuint, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-Out-Quint"), false, SetRightEaseOutQuint, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Quint"), false, SetRightEaseInOutQuint, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Expo"), false, SetRightEaseInExpo, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-Out-Expo"), false, SetRightEaseOutExpo, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Expo"), false, SetRightEaseInOutExpo, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Circ"), false, SetRightEaseInCirc, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-Out-Circ"), false, SetRightEaseOutCirc, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Circ"), false, SetRightEaseInOutCirc, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Back"), false, SetRightEaseInBack, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-Out-Back"), false, SetRightEaseOutBack, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Back"), false, SetRightEaseInOutBack, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Elastic"), false, SetRightEaseInElastic, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-Out-Elastic"), false, SetRightEaseOutElastic, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Elastic"), false, SetRightEaseInOutElastic, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Bounce"), false, SetRightEaseInBounce, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-Out-Bounce"), false, SetRightEaseOutBounce, new object[] { manager, keyIdentifiers });
                menu.AddItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Bounce"), false, SetRightEaseInOutBounce, new object[] { manager, keyIdentifiers });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Sine"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-Out-Sine"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Sine"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Quad"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-Out-Quad"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Quad"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Cubic"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-Out-Cubic"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Cubic"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Quart"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-Out-Quart"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Quart"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Quint"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-Out-Quint"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Quint"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Expo"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-Out-Expo"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Expo"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Circ"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-Out-Circ"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Circ"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Back"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-Out-Back"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Back"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Elastic"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-Out-Elastic"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Elastic"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Bounce"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-Out-Bounce"));
                menu.AddDisabledItem(new GUIContent("Right Tangent Easing/Ease-In-Out-Bounce"));
            }
        }

        private static bool TrySeparateKeyFrameTimesToResolution(float first, float last, int resolution, out float delta)
        {
            if ((last - first) / (1 / 60f) < resolution)
            {
                delta = 0.0f;
                return false;
            }

            delta = (last - first) / resolution;
            return true;
        }

        private static void SetLeftEaseInSine(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInSine, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseOutSine(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseOutSine, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInOutSine(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInOutSine, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInQuad(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInQuad, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseOutQuad(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseOutQuad, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInOutQuad(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInOutQuad, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInCubic(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInCubic, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseOutCubic(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseOutCubic, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInOutCubic(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInOutCubic, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInQuart(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInQuart, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseOutQuart(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseOutQuart, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInOutQuart(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInOutQuart, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInQuint(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInQuint, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseOutQuint(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseOutQuint, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInOutQuint(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInOutQuint, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInExpo(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInExpo, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseOutExpo(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseOutExpo, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInOutExpo(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInOutExpo, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInCirc(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInCirc, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseOutCirc(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseOutCirc, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInOutCirc(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInOutCirc, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInBack(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseInBack, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseOutBack(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseOutBack, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInOutBack(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseInOutBack, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInElastic(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseInElastic, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseOutElastic(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseOutElastic, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInOutElastic(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseInOutElastic, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInBounce(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseInBounce, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseOutBounce(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseOutBounce, w => w.Key - 1, w => w.Key);
        }

        private static void SetLeftEaseInOutBounce(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseInOutBounce, w => w.Key - 1, w => w.Key);
        }

        private static void SetRightEaseInSine(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInSine, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseOutSine(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseOutSine, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInOutSine(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInOutSine, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInQuad(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInQuad, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseOutQuad(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseOutQuad, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInOutQuad(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInOutQuad, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInCubic(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInCubic, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseOutCubic(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseOutCubic, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInOutCubic(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInOutCubic, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInQuart(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInQuart, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseOutQuart(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseOutQuart, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInOutQuart(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInOutQuart, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInQuint(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInQuint, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseOutQuint(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseOutQuint, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInOutQuint(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInOutQuint, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInExpo(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInExpo, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseOutExpo(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseOutExpo, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInOutExpo(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInOutExpo, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInCirc(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInCirc, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseOutCirc(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseOutCirc, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInOutCirc(object arguments)
        {
            SetEasingFunction(arguments, 15, EaseInOutCirc, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInBack(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseInBack, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseOutBack(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseOutBack, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInOutBack(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseInOutBack, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInElastic(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseInElastic, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseOutElastic(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseOutElastic, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInOutElastic(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseInOutElastic, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInBounce(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseInBounce, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseOutBounce(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseOutBounce, w => w.Key, w => w.Key + 1);
        }

        private static void SetRightEaseInOutBounce(object arguments)
        {
            SetEasingFunction(arguments, 30, EaseInOutBounce, w => w.Key, w => w.Key + 1);
        }

        private static void SetEasingFunction(object rawArguments, int resolution, Func<float, float> easing, Func<KeyIdentifier, int> firstKeySelector, Func<KeyIdentifier, int> lastKeySelector)
        {
            var arguments = (object[])rawArguments;
            var manager = (CurveMenuManager)arguments[0];
            var identifiers = (List<KeyIdentifier>)arguments[1];

            var changes = new List<ChangedCurve>();

            foreach (var identifier in identifiers)
            {
                var curve = identifier.Curve;
                var firstKeyIndex = firstKeySelector.Invoke(identifier); // identifier.PreviousKeyframe;
                var lastKeyIndex = lastKeySelector.Invoke(identifier); // identifier.Keyframe;
                var firstKey = curve[firstKeyIndex];
                var lastKey = curve[lastKeyIndex];

                if (TrySeparateKeyFrameTimesToResolution(firstKey.time, lastKey.time, resolution, out var delta))
                {
                    curve.RemoveKey(lastKeyIndex);
                    curve.RemoveKey(firstKeyIndex);

                    var keys = new List<int>();

                    for (var i = 0; i <= resolution; i++)
                    {
                        var t = firstKey.time + delta * i;
                        var v = easing.Invoke(i / (float)resolution);
                        var d = (lastKey.value - firstKey.value) * v + firstKey.value;

                        var j = curve.AddKey(t, d);
                        AnimationUtility.SetKeyBroken(curve, j, true);
                        AnimationUtility.SetKeyLeftTangentMode(curve, i, AnimationUtility.TangentMode.Free);
                        AnimationUtility.SetKeyRightTangentMode(curve, i, AnimationUtility.TangentMode.Free);

                        keys.Add(j);
                    }

                    foreach (var j in keys)
                        curve.SmoothTangents(j, 0f);

                    AnimationUtilityRefl.UpdateTangentsFromMode(curve);

                    changes.Add(ChangedCurve.Instantiate(curve, identifier.CurveId, identifier.Binding));
                }
                else
                {
                    Debug.LogWarning($"Failed to apply easing function because there is not enough delta -> require deltas: {resolution}, actual: less");
                }
            }

            manager.Updater.UpdateCurves(changes, "Apply Easing Functions");
        }

        private static float EaseInSine(float t)
        {
            return 1 - Mathf.Cos(t * Mathf.PI / 2);
        }

        private static float EaseOutSine(float t)
        {
            return Mathf.Sin(t * Mathf.PI / 2);
        }

        private static float EaseInOutSine(float t)
        {
            return -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
        }

        private static float EaseInQuad(float t)
        {
            return t * t;
        }

        private static float EaseOutQuad(float t)
        {
            return 1 - (1 - t) * (1 - t);
        }

        private static float EaseInOutQuad(float t)
        {
            return t < 0.5 ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
        }

        private static float EaseInCubic(float t)
        {
            return t * t * t;
        }

        private static float EaseOutCubic(float t)
        {
            return 1 - Mathf.Pow(1 - t, 3);
        }

        private static float EaseInOutCubic(float t)
        {
            return t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
        }

        private static float EaseInQuart(float t)
        {
            return t * t * t * t;
        }

        private static float EaseOutQuart(float t)
        {
            return 1 - Mathf.Pow(1 - t, 4);
        }

        private static float EaseInOutQuart(float t)
        {
            return t < 0.5 ? 8 * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 4) / 2;
        }

        private static float EaseInQuint(float t)
        {
            return t * t * t * t * t;
        }

        private static float EaseOutQuint(float t)
        {
            return 1 - Mathf.Pow(1 - t, 5);
        }

        private static float EaseInOutQuint(float t)
        {
            return t < 0.5 ? 16 * t * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 5) / 2;
        }

        private static float EaseInExpo(float t)
        {
            return t == 0 ? 0 : Mathf.Pow(2, 10 * t - 10);
        }

        private static float EaseOutExpo(float t)
        {
            return Math.Abs(t - 1) < Epsilon ? 1 : 1 - Mathf.Pow(2, -10 * t);
        }

        private static float EaseInOutExpo(float t)
        {
            if (t == 0)
                return 0;
            if (Mathf.Abs(t - 1) < Epsilon)
                return 1;
            return t < 0.5 ? Mathf.Pow(2, 20 * t - 10) / 2 : (2 - Mathf.Pow(2, -20 * t + 10)) / 2;
        }

        private static float EaseInCirc(float t)
        {
            return 1 - Mathf.Sqrt(1 - Mathf.Pow(t, 2));
        }

        private static float EaseOutCirc(float t)
        {
            return Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2));
        }

        private static float EaseInOutCirc(float t)
        {
            return t < 0.5 ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * t, 2))) / 2 : (Mathf.Sqrt(1 - Mathf.Pow(-2 * t + 2, 2)) + 1) / 2;
        }

        private static float EaseInBack(float t)
        {
            return C3 * t * t * t - C1 * t * t;
        }

        private static float EaseOutBack(float t)
        {
            return 1 + C3 * Mathf.Pow(t - 1, 3) + C1 * Mathf.Pow(t - 1, 2);
        }

        private static float EaseInOutBack(float t)
        {
            return t < 0.5 ? Mathf.Pow(2 * t, 2) * ((C2 + 1) * 2 * t - C2) / 2 : (Mathf.Pow(2 * t - 2, 2) * ((C2 + 1) * (t * 2 - 2) + C2) + 2) / 2;
        }

        private static float EaseInElastic(float t)
        {
            if (t == 0)
                return 0;
            if (Mathf.Abs(1 - t) < Epsilon)
                return 1;
            return -Mathf.Pow(2, 10 * t - 10) * Mathf.Sin((t * 10 - 10.75f) * C4);
        }

        private static float EaseOutElastic(float t)
        {
            if (t == 0)
                return 0;
            if (Mathf.Abs(1 - t) < Epsilon)
                return 1;
            return Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * C4) + 1;
        }

        private static float EaseInOutElastic(float t)
        {
            if (t == 0)
                return 0;
            if (Mathf.Abs(1 - t) < Epsilon)
                return 1;
            return t < 0.5 ? -(Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * C5)) / 2 : Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 11.125f) * C5) / 2 + 1;
        }

        private static float EaseInBounce(float t)
        {
            return 1 - EaseOutBounce(1 - t);
        }

        private static float EaseOutBounce(float t)
        {
            if (t < 1 / D1)
                return N1 * t * t;
            if (t < 2 / D1)
                return N1 * (t += 1.5f / D1) * t * 0.75f;
            if (t < 2.5 / D1)
                return N1 * (t -= 2.25f / D1) * t + 0.9375f;
            return N1 * (t -= 2.625f / D1) * t + 0.984375f;
        }

        private static float EaseInOutBounce(float t)
        {
            return t < 0.5 ? (1 - EaseOutBounce(1 - 2 * t)) / 2 : (1 + EaseOutBounce(2 * t - 1)) / 2;
        }
    }
}