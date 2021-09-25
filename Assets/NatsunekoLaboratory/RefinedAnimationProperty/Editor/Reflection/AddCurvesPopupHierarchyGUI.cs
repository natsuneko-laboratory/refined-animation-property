using System;
using System.Reflection;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions;

using UnityEditor.IMGUI.Controls;

using UnityEditorInternal;

using UnityEngine;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    // ReSharper disable once InconsistentNaming
    internal class AddCurvesPopupHierarchyGUI : ReflectionClass
    {
        private static readonly Type T;

        static AddCurvesPopupHierarchyGUI()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditorInternal.AddCurvesPopupHierarchyGUI");
        }

        internal AddCurvesPopupHierarchyGUI(object instance) : base(instance, T) { }

        public void OnRowGUI(Rect rowRect, TreeViewItem node, int row, bool selected, bool focused)
        {
            InvokeMethod(nameof(OnRowGUI), BindingFlags.Public | BindingFlags.Instance, rowRect, node, row, selected, focused);
        }
    }
}
