using System;
using System.Reflection;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions;

using UnityEditorInternal;

using UnityEngine;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class AddCurvesPopup : ReflectionClass
    {
        private static readonly Type T;

        static AddCurvesPopup()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditorInternal.AddCurvesPopup");
        }

        public AddCurvesPopup(object instance) : base(instance, T) { }

        public Vector2 GetWindowSize()
        {
            return InvokeMethod<Vector2>("GetWindowSize", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static void AddNewCurve(object node)
        {
            ReflectionStaticClass.InvokeMethod(T, nameof(AddNewCurve), BindingFlags.NonPublic | BindingFlags.Static, node);
        }

        public void Close()
        {
            InvokeMethod(nameof(Close), BindingFlags.Public | BindingFlags.Instance);
        }

        public static AddCurvesPopupHierarchy Hierarchy
        {
            get
            {
                var obj = ReflectionStaticClass.InvokeField<object>(T, "s_Hierarchy", BindingFlags.NonPublic | BindingFlags.Static);
                return new AddCurvesPopupHierarchy(obj);
            }
        }

        public static AnimationWindowState State
        {
            get
            {
                var obj = ReflectionStaticClass.InvokeField<object>(T, "s_State", BindingFlags.NonPublic | BindingFlags.Static);
                return new AnimationWindowState(obj);
            }
        }
    }
}