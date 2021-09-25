using System;
using System.Reflection;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions;

using UnityEditor.IMGUI.Controls;

using UnityEditorInternal;

using UnityEngine;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class AddCurvesPopupHierarchyDataSource : ReflectionClass
    {
        private static readonly Type T;

        static AddCurvesPopupHierarchyDataSource()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditorInternal.AddCurvesPopupHierarchyDataSource");
        }

        internal AddCurvesPopupHierarchyDataSource(object instance) : base(instance, T) { }

        public void FetchData()
        {
            InvokeMethod(nameof(FetchData), BindingFlags.Public | BindingFlags.Instance);
        }

        public TreeViewItem RootItem => InvokeField<TreeViewItem>("m_RootItem", BindingFlags.NonPublic | BindingFlags.Instance);
    }
}
