/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Reflection;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions;

using UnityEditor.IMGUI.Controls;

using UnityEditorInternal;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class AddCurvesPopupHierarchyDataSource : ReflectionClass
    {
        private static readonly Type T;

        public TreeViewItem RootItem => InvokeField<TreeViewItem>("m_RootItem", BindingFlags.NonPublic | BindingFlags.Instance);

        static AddCurvesPopupHierarchyDataSource()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditorInternal.AddCurvesPopupHierarchyDataSource");
        }

        internal AddCurvesPopupHierarchyDataSource(object instance) : base(instance, T) { }

        public void FetchData()
        {
            InvokeMethod(nameof(FetchData), BindingFlags.Public | BindingFlags.Instance);
        }
    }
}