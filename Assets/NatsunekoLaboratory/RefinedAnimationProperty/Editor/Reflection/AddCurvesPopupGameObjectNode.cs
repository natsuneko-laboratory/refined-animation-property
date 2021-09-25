using System;
using System.Reflection;

using UnityEditor.IMGUI.Controls;

using UnityEditorInternal;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class AddCurvesPopupGameObjectNode : ReflectedTreeViewItem
    {
        private static readonly Type T;

        public bool Disabled => InvokeField<bool>("disabled", BindingFlags.NonPublic | BindingFlags.Instance);

        static AddCurvesPopupGameObjectNode()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditorInternal.AddCurvesPopupGameObjectNode");
        }

        internal AddCurvesPopupGameObjectNode(object instance) : base(instance, T) { }

        public override SearchableTreeViewItem Clone()
        {
            return new AddCurvesPopupGameObjectNodeInternal { depth = Depth, displayName = DisplayName, icon = Icon, id = Id, Disabled = Disabled};
        }

        internal class AddCurvesPopupGameObjectNodeInternal : SearchableTreeViewItem
        {
            public bool Disabled { get; set; }
        }
    }
}