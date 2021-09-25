using System;
using System.Reflection;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions;

using UnityEditor.IMGUI.Controls;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class TreeViewController : ReflectionClass
    {
        private static readonly Type T;

        static TreeViewController()
        {
            T = typeof(TreeViewItem).Assembly.GetType("UnityEditor.IMGUI.Controls.TreeViewController");
        }

        internal TreeViewController(object instance) : base(instance, T) { }

        public AddCurvesPopupHierarchyGUI Gui
        {
            get
            {
                var obj = InvokeProperty<object>("gui", BindingFlags.Public | BindingFlags.Instance);
                return new AddCurvesPopupHierarchyGUI(obj);
            }
        }
    }
}