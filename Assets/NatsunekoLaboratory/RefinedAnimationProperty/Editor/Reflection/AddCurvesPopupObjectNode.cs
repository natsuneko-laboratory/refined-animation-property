using System;

using UnityEditor.IMGUI.Controls;

using UnityEditorInternal;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class AddCurvesPopupObjectNode : ReflectedTreeViewItem
    {
        private static readonly Type T;

        static AddCurvesPopupObjectNode()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditorInternal.AddCurvesPopupObjectNode");
        }

        internal AddCurvesPopupObjectNode(object instance) : base(instance, T) { }

        public override SearchableTreeViewItem Clone()
        {
            return new AddCurvesPopupObjectNodeInternal { depth = Depth, displayName = DisplayName, icon = Icon, id = Id };
        }


        internal class AddCurvesPopupObjectNodeInternal : SearchableTreeViewItem { }
    }
}