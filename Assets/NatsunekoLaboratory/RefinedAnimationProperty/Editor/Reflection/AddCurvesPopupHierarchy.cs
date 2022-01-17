// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the License Zero Parity 7.0.0 (see LICENSE-PARITY file) and MIT (contributions, see LICENSE-MIT file) with exception License Zero Patron 1.0.0 (see LICENSE-PATRON file)
// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions;

using UnityEditorInternal;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class AddCurvesPopupHierarchy : ReflectionClass
    {
        private static readonly Type T;

        public AddCurvesPopupHierarchyDataSource TreeViewDataSource
        {
            get
            {
                var obj = InvokeField<object>("m_TreeViewDataSource", BindingFlags.NonPublic | BindingFlags.Instance);
                return obj != null ? new AddCurvesPopupHierarchyDataSource(obj) : null;
            }
        }

        static AddCurvesPopupHierarchy()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditorInternal.AddCurvesPopupHierarchy");
        }

        internal AddCurvesPopupHierarchy(object instance) : base(instance, T) { }
    }
}