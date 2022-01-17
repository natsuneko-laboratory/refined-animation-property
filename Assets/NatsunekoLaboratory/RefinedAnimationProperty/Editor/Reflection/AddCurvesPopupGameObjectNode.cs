// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the License Zero Parity 7.0.0 (see LICENSE-PARITY file) and MIT (contributions, see LICENSE-MIT file) with exception License Zero Patron 1.0.0 (see LICENSE-PATRON file)
// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;

using UnityEditorInternal;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class AddCurvesPopupGameObjectNode : ReflectedTreeViewItem
    {
        private static readonly Type T;

        private bool Disabled => InvokeField<bool>("disabled", BindingFlags.NonPublic | BindingFlags.Instance);

        static AddCurvesPopupGameObjectNode()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditorInternal.AddCurvesPopupGameObjectNode");
        }

        internal AddCurvesPopupGameObjectNode(object instance) : base(instance, T) { }

        public override SearchableTreeViewItem Clone()
        {
            return new AddCurvesPopupGameObjectNodeInternal { depth = Depth, displayName = DisplayName, icon = Icon, id = Id, Disabled = Disabled };
        }

        internal class AddCurvesPopupGameObjectNodeInternal : SearchableTreeViewItem
        {
            public bool Disabled { get; set; }
        }
    }
}