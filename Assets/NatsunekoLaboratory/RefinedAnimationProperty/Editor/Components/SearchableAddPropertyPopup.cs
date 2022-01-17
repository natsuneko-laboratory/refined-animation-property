// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the License Zero Parity 7.0.0 (see LICENSE-PARITY file) and MIT (contributions, see LICENSE-MIT file) with exception License Zero Patron 1.0.0 (see LICENSE-PATRON file)
// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using HarmonyLib;

using NatsunekoLaboratory.RefinedAnimationProperty.Reflection;

using UnityEditor;
using UnityEditor.IMGUI.Controls;

using UnityEditorInternal;

using UnityEngine;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Components
{
    internal class SearchableAddPropertyPopup : IEditorComponent
    {
        private static AnimationPropertyTreeView _treeView;
        private static string _searchQuery;
        private static bool _isRebuildNeeded;
        private static GameObject _previousActiveRootGameObject;
        private static ScriptableObject _previousActiveRootScriptableObject;
        private static SearchField _search;
        private static SearchField _searchField;
        private static TreeViewState _treeViewState;

        public void Initialize(Harmony harmony)
        {
            var t1 = typeof(AssetStore).Assembly.GetType("UnityEditorInternal.AddCurvesPopup");
            var mOriginal1 = AccessTools.Method(t1, "OnGUI");
            var mPrefix1 = AccessTools.Method(typeof(SearchableAddPropertyPopup), nameof(ReplacedOnGUI));

            harmony.Patch(mOriginal1, new HarmonyMethod(mPrefix1));

            var t2 = typeof(AssetStore).Assembly.GetType("UnityEditorInternal.AnimationWindowHierarchyGUI");
            var mOriginal2 = AccessTools.Method(t2, "RemoveCurvesFromNodes");
            var mPostfix2 = AccessTools.Method(typeof(SearchableAddPropertyPopup), nameof(OnHandleRemoveCurvesFromNodes));

            harmony.Patch(mOriginal2, null, new HarmonyMethod(mPostfix2));
        }

        private static void OnHandleRemoveCurvesFromNodes()
        {
            _isRebuildNeeded = true;
        }

        // ReSharper disable once InconsistentNaming
        private static bool ReplacedOnGUI(object __instance)
        {
            if (Event.current.type == EventType.Layout)
                return false;

            var popup = new AddCurvesPopup(__instance);
            var windowSize = popup.GetWindowSize();

            GUI.Box(new Rect(0, 0, windowSize.x, windowSize.y), GUIContent.none, "grey_border");

            InitIfNeeded();
            ReBuildIfNeeded();

            EditorGUI.BeginChangeCheck();
            _searchQuery = _searchField.OnToolbarGUI(new Rect(1, 1, windowSize.x - 3, 24), _searchQuery); // _treeView.searchString is not working correctly
            _treeView.OnGUI(new Rect(1, 20, windowSize.x - 3, windowSize.y - 30));

            if (EditorGUI.EndChangeCheck())
                _treeView.Reload();

            return false;
        }

        private static void InitIfNeeded()
        {
            if (_treeViewState == null)
                _treeViewState = new TreeViewState();

            if (_treeView == null)
                _treeView = new AnimationPropertyTreeView(_treeViewState);

            if (_searchField == null)
                _searchField = new SearchField();
        }

        private static void ReBuildIfNeeded()
        {
            if (_isRebuildNeeded)
            {
                _isRebuildNeeded = false;
                _treeView.BuildItemsTree();
            }

            if (_previousActiveRootGameObject == AddCurvesPopup.State.ActiveRootGameObject && _previousActiveRootScriptableObject == AddCurvesPopup.State.ActiveScriptableObject)
                return;

            _previousActiveRootGameObject = AddCurvesPopup.State.ActiveRootGameObject;
            _previousActiveRootScriptableObject = AddCurvesPopup.State.ActiveScriptableObject;
            _isRebuildNeeded = false;

            _treeView.BuildItemsTree();
        }

        private class AnimationPropertyTreeView : TreeView
        {
            private TreeViewItem _item;

            public AnimationPropertyTreeView(TreeViewState state) : base(state)
            {
                BuildItemsTree();
            }

            public void BuildItemsTree()
            {
                AddCurvesPopup.Hierarchy?.TreeViewDataSource?.FetchData();

                _item = AddCurvesPopup.Hierarchy?.TreeViewDataSource?.RootItem ?? new TreeViewItem();

                Reload();
            }

            protected override TreeViewItem BuildRoot()
            {
                return new TreeViewItem(0, -1, "ROOT");
            }

            protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
            {
                var rows = GetRows() ?? new List<TreeViewItem>();
                rows.Clear();

                if (string.IsNullOrWhiteSpace(_searchQuery))
                    foreach (var element in _item.children)
                    {
                        var node = Instantiate(element);
                        if (node == null)
                            continue;

                        var item = node.Clone();
                        root.AddChild(item);
                        rows.Add(item);

                        if (element.hasChildren)
                        {
                            if (IsExpanded(item.id))
                                AddItemsRecursive(element, item, rows);
                            else
                                item.children = CreateChildListForCollapsedParent();
                        }
                    }
                else
                    foreach (var element in _item.children)
                    {
                        var node = Instantiate(element);
                        if (node == null)
                            continue;

                        var item = node.Clone();
                        root.AddChild(item);
                        rows.Add(item);

                        if (element.hasChildren)
                        {
                            if (IsExpanded(item.id))
                            {
                                AddItemsRecursive(element, item, rows, _searchQuery);
                            }
                            else
                            {
                                var list = new List<TreeViewItem>();
                                AddItemsRecursive(element, new TreeViewItem(), list, _searchQuery);
                                if (list.Count > 0)
                                    item.children = CreateChildListForCollapsedParent();
                            }
                        }

                        if (string.IsNullOrWhiteSpace(_searchQuery))
                            continue;

                        if (item.children != null && item.children.Count > 0)
                        {
                            if (item.children.Count > 1 || item.children[0] != null)
                                item.children = item.children.Where(w => (w as ReflectedTreeViewItem.SearchableTreeViewItem)?.MarkAsHit == true).ToList();

                            item.MarkAsHit = item.children.Count > 0;
                        }

                        if (item.displayName.ToLower().Contains(_searchQuery.ToLower()))
                            item.MarkAsHit = true;

                        if (!item.MarkAsHit)
                        {
                            // removed
                            root.children = root.children.Where(w => w.id != item.id).ToList();
                            rows.Remove(item);
                        }
                    }


                SetupDepthsFromParentsAndChildren(root);

                return rows;
            }

            protected override void RowGUI(RowGUIArgs args)
            {
                var propertyPathMismatchWithHumanAvatar = false;
                if (args.item is AddCurvesPopupGameObjectNode.AddCurvesPopupGameObjectNodeInternal addCurvesPopupNode)
                    propertyPathMismatchWithHumanAvatar = addCurvesPopupNode.Disabled;

                using (new EditorGUI.DisabledGroupScope(propertyPathMismatchWithHumanAvatar))
                {
                    base.RowGUI(args);
                    DoAddCurveButton(args.rowRect, args.item);
                    HandleContextMenu(args.rowRect, args.item);
                }
            }

            private void DoAddCurveButton(Rect rowRect, TreeViewItem item)
            {
                var hierarchyNode = item as AddCurvesPopupPropertyNode.AddCurvesPopupPropertyNodeInternal;
                if (hierarchyNode == null || hierarchyNode.CurveBindings == null || hierarchyNode.CurveBindings.Length == 0)
                    return;

                var buttonRect = new Rect(rowRect.width - 17, rowRect.yMin, 17, ((GUIStyle)"IconButton").fixedHeight);
                GUI.Box(buttonRect, GUIContent.none, "Tag MenuItem");

                if (GUI.Button(buttonRect, EditorGUIUtility.TrIconContent("Toolbar Plus"), "IconButton"))
                {
                    AddCurvesPopup.AddNewCurve(hierarchyNode.Original);

                    BuildItemsTree();
                }
            }

            private void HandleContextMenu(Rect rowRect, TreeViewItem item)
            {
                if (Event.current.type != EventType.ContextClick)
                    return;

                if (rowRect.Contains(Event.current.mousePosition))
                {
                    var ids = new List<int>(_treeView.GetSelection()) { item.id };
                    _treeView.SetSelection(ids, TreeViewSelectionOptions.None);

                    GenerateMenu().ShowAsContext();
                    Event.current.Use();
                }
            }

            private GenericMenu GenerateMenu()
            {
                var menu = new GenericMenu();
                menu.AddItem(EditorGUIUtility.TrTextContent("Add Properties"), false, AddPropertiesFromSelectedNodes);

                return menu;
            }

            private void AddPropertiesFromSelectedNodes()
            {
                var ids = _treeView.GetSelection();
                foreach (var id in ids)
                {
                    var node = _item.children.FirstOrDefault(w => w.id == id);
                    if (node == null)
                        continue;

                    if (node.GetType() == AddCurvesPopupPropertyNode.ReflectedT)
                        AddCurvesPopup.AddNewCurve(node);
                    else if (node.hasChildren)
                        foreach (var item in node.children)
                            if (item.GetType() == AddCurvesPopupPropertyNode.ReflectedT)
                                AddCurvesPopup.AddNewCurve(item);
                }

                BuildItemsTree();
            }


            private ReflectedTreeViewItem Instantiate(object obj)
            {
                var a = new AddCurvesPopupGameObjectNode(obj);
                if (a.IsValid())
                    return a;

                var b = new AddCurvesPopupObjectNode(obj);
                if (b.IsValid())
                    return b;

                var c = new AddCurvesPopupPropertyNode(obj);
                if (c.IsValid())
                    return c;

                return null;
            }

            private void AddItemsRecursive(TreeViewItem element, TreeViewItem item, IList<TreeViewItem> rows, string searchQuery = null)
            {
                foreach (var childElement in element.children)
                {
                    var obj = Instantiate(childElement);
                    if (obj == null)
                        continue;

                    var childItem = obj.Clone();
                    item.AddChild(childItem);
                    rows.Add(childItem);

                    if (childElement.hasChildren)
                    {
                        if (IsExpanded(childItem.id))
                        {
                            AddItemsRecursive(childElement, childItem, rows, searchQuery);
                        }
                        else
                        {
                            var list = new List<TreeViewItem>();
                            AddItemsRecursive(childElement, new TreeViewItem(), list, searchQuery);
                            if (list.Count > 0)
                                childItem.children = CreateChildListForCollapsedParent();
                        }
                    }

                    if (string.IsNullOrWhiteSpace(searchQuery))
                        continue;

                    if (childItem.children != null && childItem.children.Count > 0)
                    {
                        if (childItem.children.Count > 1 || childItem.children[0] != null)
                            childItem.children = childItem.children.Where(w => (w as ReflectedTreeViewItem.SearchableTreeViewItem)?.MarkAsHit == true).ToList();

                        childItem.MarkAsHit = childItem.children.Count > 0;
                    }

                    if (childItem.displayName.ToLower().Contains(searchQuery.ToLower()))
                        childItem.MarkAsHit = true;

                    if (!childItem.MarkAsHit)
                    {
                        // removed
                        item.children = item.children.Where(w => w.id != childItem.id).ToList();
                        rows.Remove(childItem);
                    }
                }
            }
        }
    }
}