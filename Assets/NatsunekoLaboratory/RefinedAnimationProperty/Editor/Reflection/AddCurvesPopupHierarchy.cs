using System;
using System.Reflection;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions;

using UnityEditorInternal;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class AddCurvesPopupHierarchy : ReflectionClass
    {
        private static readonly Type T;

        public TreeViewController TreeView
        {
            get
            {
                var obj = InvokeField<object>("m_TreeView", BindingFlags.NonPublic | BindingFlags.Instance);
                return new TreeViewController(obj);
            }
        }

        public AddCurvesPopupHierarchyDataSource TreeViewDataSource
        {
            get
            {
                var obj = InvokeField<object>("m_TreeViewDataSource", BindingFlags.NonPublic | BindingFlags.Instance);
                return new AddCurvesPopupHierarchyDataSource(obj);
            }
        }

        static AddCurvesPopupHierarchy()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditorInternal.AddCurvesPopupHierarchy");
        }

        internal AddCurvesPopupHierarchy(object instance) : base(instance, T) { }
    }
}