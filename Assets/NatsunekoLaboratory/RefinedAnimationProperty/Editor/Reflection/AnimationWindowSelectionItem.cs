using System;
using System.Reflection;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions;

using UnityEditorInternal;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class AnimationWindowSelectionItem : ReflectionClass
    {
        private static readonly Type T;

        static AnimationWindowSelectionItem()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditorInternal.AnimationWindowSelectionItem");
        }

        public AnimationWindowSelectionItem(object instance) : base(instance, T) { }

        public bool CanAddCurves => InvokeProperty<bool>("canAddCurves", BindingFlags.Public | BindingFlags.Instance);
    }
}
