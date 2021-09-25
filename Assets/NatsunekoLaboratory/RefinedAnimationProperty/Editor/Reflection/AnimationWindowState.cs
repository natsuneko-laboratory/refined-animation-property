using System;
using System.Reflection;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions;

using UnityEditorInternal;

using UnityEngine;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal class AnimationWindowState : ReflectionClass
    {

        private static readonly Type T;
        static AnimationWindowState()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditorInternal.AnimationWindowState");
        }

        public AnimationWindowState(object instance) : base(instance, T) { }

        public AnimationWindowSelectionItem Selection
        {
            get
            {
                var obj = InvokeProperty<object>("selection", BindingFlags.Public | BindingFlags.Instance);
                return new AnimationWindowSelectionItem(obj);
            }
        }

        public AnimationClip ActiveAnimationClip => InvokeProperty<AnimationClip>("activeAnimationClip", BindingFlags.Public | BindingFlags.Instance);

        public GameObject ActiveRootGameObject => InvokeProperty<GameObject>("activeRootGameObject", BindingFlags.Public | BindingFlags.Instance);

        public ScriptableObject ActiveScriptableObject => InvokeProperty<ScriptableObject>("activeScriptableObject", BindingFlags.Public | BindingFlags.Instance);
    }
}
