// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the License Zero Parity 7.0.0 (see LICENSE-PARITY file) and MIT (contributions, see LICENSE-MIT file) with exception License Zero Patron 1.0.0 (see LICENSE-PATRON file)
// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

using System;

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