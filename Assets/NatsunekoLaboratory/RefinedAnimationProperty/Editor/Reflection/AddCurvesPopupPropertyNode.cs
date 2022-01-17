// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the License Zero Parity 7.0.0 (see LICENSE-PARITY file) and MIT (contributions, see LICENSE-MIT file) with exception License Zero Patron 1.0.0 (see LICENSE-PATRON file)
// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;

using UnityEditor;
using UnityEditor.IMGUI.Controls;

using UnityEditorInternal;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class AddCurvesPopupPropertyNode : ReflectedTreeViewItem
    {
        private static readonly Type T;

        public static Type ReflectedT => T;

        public EditorCurveBinding[] CurveBindings => InvokeField<EditorCurveBinding[]>("curveBindings", BindingFlags.Public | BindingFlags.Instance);

        static AddCurvesPopupPropertyNode()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditorInternal.AddCurvesPopupPropertyNode");
        }

        internal AddCurvesPopupPropertyNode(object instance) : base(instance, T) { }

        public override SearchableTreeViewItem Clone()
        {
            return new AddCurvesPopupPropertyNodeInternal { depth = Depth, displayName = DisplayName, icon = Icon, id = Id, CurveBindings = CurveBindings, Original = RawInstance };
        }

        internal class AddCurvesPopupPropertyNodeInternal : SearchableTreeViewItem
        {
            public object Original { get; set; }

            public EditorCurveBinding[] CurveBindings { get; set; }

            public override int CompareTo(TreeViewItem other)
            {
                if (other is AddCurvesPopupPropertyNodeInternal otherNode)
                {
                    if (displayName.Contains("Rotation") && otherNode.displayName.Contains("Position"))
                        return 1;
                    if (displayName.Contains("Position") && otherNode.displayName.Contains("Rotation"))
                        return -1;
                }

                return base.CompareTo(other);
            }
        }
    }
}