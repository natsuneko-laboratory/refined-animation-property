/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Reflection;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection.Expressions;

using UnityEditor.IMGUI.Controls;

using UnityEngine;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Reflection
{
    internal abstract class ReflectedTreeViewItem : ReflectionClass
    {
        public int Id => InvokeProperty<int>("id", BindingFlags.Public | BindingFlags.Instance);

        public string DisplayName => InvokeProperty<string>("displayName", BindingFlags.Public | BindingFlags.Instance);

        public int Depth => InvokeProperty<int>("depth", BindingFlags.Public | BindingFlags.Instance);

        public Texture2D Icon => InvokeProperty<Texture2D>("icon", BindingFlags.Public | BindingFlags.Instance);

        protected ReflectedTreeViewItem(object instance, Type type) : base(instance, type) { }

        public abstract SearchableTreeViewItem Clone();

        public class SearchableTreeViewItem : TreeViewItem
        {
            public bool MarkAsHit { get; set; }
        }
    }
}