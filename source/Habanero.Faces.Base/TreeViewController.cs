#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using log4net;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// A controller used to map an <see cref="IBusinessObjectCollection"/> onto an <see cref="ITreeView"/>. Each <see cref="IBusinessObject"/>
    /// is displayed as a node in the treeview, and the multiple relationships of the <see cref="IBusinessObject"/> are displayed as
    /// subnodes.
    /// </summary>
    public class TreeViewController // : ISelectorController
    {
        /// <summary>
        /// Uses for logging 
        /// </summary>
        protected static readonly ILog log = LogManager.GetLogger("Habanero.Faces.Base.TreeViewController");

        private bool _preventSelectionChanged;
        private static int _levelsToDisplay;
        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<IBusinessObject, NodeState> ObjectNodes { get; set; }
        private Dictionary<IRelationship, NodeState> RelationshipNodes { get; set; }
        private Dictionary<IBusinessObjectCollection, NodeState> ChildCollectionNodes { get; set; }

        ///<summary>
        /// The event is fired with a different business object is selected in the tree.
        ///</summary>
        public event EventHandler<BOEventArgs> BusinessObjectSelected;

        ///<summary>
        /// A delegate that is used to Setup the node for a business object
        ///</summary>
        ///<param name="treeNode"></param>
        ///<param name="businessObject"></param>
        public delegate void SetupNodeWithBusinessObjectDelegate(ITreeNode treeNode, IBusinessObject businessObject);
        /// <summary>
        /// The delegeate that is used to setup the node for a relationship.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="relationship"></param>
        public delegate void SetupNodeWithRelationshipDelegate(ITreeNode node, IRelationship relationship);

        ///<summary>
        /// The Getter and setter for seting the <see cref="SetupNodeWithBusinessObjectDelegate"/> delegate
        ///</summary>
        public SetupNodeWithBusinessObjectDelegate SetupNodeWithBusinessObject { get; set; }
        ///<summary>
        /// The Getter and setter for seting the <see cref="SetupNodeWithRelationshipDelegate"/> delegate
        ///</summary>
        public SetupNodeWithRelationshipDelegate SetupNodeWithRelationship { get; set; }

        protected internal class NodeState
        {
            internal ITreeNode Node;
            internal bool IsLoaded;
            internal ITreeNode ParentNode;

            public NodeState(ITreeNode node)
            {
                Node = node;
                IsLoaded = false;
                ParentNode = node.Parent;
            }

            public bool IsConnected()
            {
                return Node.TreeView != null;
            }

            public void Disconnect()
            {
                if (Node.Level == 0) return;
                if (IsConnected()) Node.Remove();
            }

            public void Connect(int index)
            {
                if (!IsConnected())
                {
                    ParentNode.Nodes.Insert(index, Node);
                }
            }
        }

        /// <summary>
        /// Constructs the TreeViewController. 
        /// </summary>
        /// <param name="treeView">The <see cref="ITreeView"/> to control/map to</param>
        public TreeViewController(ITreeView treeView)
        {
            _preventSelectionChanged = false;
            ObjectNodes = new Dictionary<IBusinessObject, NodeState>(20);
            RelationshipNodes = new Dictionary<IRelationship, NodeState>(5);
            ChildCollectionNodes = new Dictionary<IBusinessObjectCollection, NodeState>(5);
            TreeView = treeView;
            TreeView.HideSelection = false;
            TreeView.AfterSelect += TreeView_AfterSelect;
            TreeView.BeforeExpand += TreeView_BeforeExpand;
            _levelsToDisplay = -1;
        }

        /// <summary>
        /// Destructor. Removes the event handlers that the controller sets up on the controlled treeview.
        /// </summary>
        ~TreeViewController()
        {
            TreeView.AfterSelect -= TreeView_AfterSelect;
            TreeView.BeforeExpand -= TreeView_BeforeExpand;
            CleanUp();
        }

        ///<summary>
        /// Returns the Business Object that forms the Root of this Tree.
        ///</summary>
        public IBusinessObject RootNodeBusinessObject { get; private set; }

        ///<summary>
        /// Returns the Tree View being controlled by this controller.
        ///</summary>
        public ITreeView TreeView { get; private set; }

        #region Utility Methods

        private static string GetClassDescription(IBusinessObject businessObject)
        {
            return Convert.ToString(businessObject);
        }

        private void RegisterForBusinessObjectEvents(IBusinessObject businessObject)
        {
            businessObject.Updated += BusinessObject_Updated;
            businessObject.Deleted += BusinessObject_Deleted;
            businessObject.PropertyUpdated += BusinessObject_Prop_Updated;
        }

        private void BusinessObject_Prop_Updated(object sender, BOPropUpdatedEventArgs eventArgs1)
        {
            UpdateBusinessObject(eventArgs1.BusinessObject);
        }

        private void UnRegisterForBusinessObjectEvents(IBusinessObject businessObject)
        {
            businessObject.Updated -= BusinessObject_Updated;
            businessObject.Deleted -= BusinessObject_Deleted;
            businessObject.PropertyUpdated -= BusinessObject_Prop_Updated;
        }

        private static string GetRelationshipDescription(IRelationship relationship)
        {
            Habanero.BO.Relationship multipleRelationship = relationship as Habanero.BO.Relationship;
            if (multipleRelationship != null) return multipleRelationship.RelationshipName;
            return "Unknown";
        }

        private IDictionary<string, IRelationship> GetVisibleRelationships(IBusinessObject businessObject)
        {
            Dictionary<string, IRelationship> relationships = new Dictionary<string, IRelationship>(4);
            foreach (IRelationship relationship in businessObject.Relationships)
            {
                if (MustRelationshipBeVisible(relationship))
                {
                    relationships.Add(relationship.RelationshipName, relationship);
                }
            }
            return relationships;
        }

        /// <summary>
        /// Returns whether the relationship should be shown in the tree view or not.<br/>
        /// By default all Composition and Aggregation relationships will be shown in the 
        /// tree. This method can be overriden to only show the relationships that you want.
        /// </summary>
        /// <param name="relationship"></param>
        /// <returns></returns>
        protected virtual bool MustRelationshipBeVisible(IRelationship relationship)
        {
            IRelationshipDef relationshipDef = relationship.RelationshipDef;
            return relationshipDef.RelationshipType == RelationshipType.Composition
                   || relationshipDef.RelationshipType == RelationshipType.Aggregation;
        }

        private void RegisterForBusinessObjectCollectionEvents(IBusinessObjectCollection businessObjectCollection)
        {
            businessObjectCollection.BusinessObjectAdded += BusinessObjectCollection_ChildAdded;
            businessObjectCollection.BusinessObjectRemoved += BusinessObjectCollection_ChildRemoved;
        }

        private void UnRegisterForBusinessObjectCollectionEvents(IBusinessObjectCollection businessObjectCollection)
        {
            businessObjectCollection.BusinessObjectAdded -= BusinessObjectCollection_ChildAdded;
            businessObjectCollection.BusinessObjectRemoved -= BusinessObjectCollection_ChildRemoved;
        }

        #endregion //Utility Methods

        #region Loading

        ///<summary>
        /// Loads the tree view with business object and all child objects.
        ///</summary>
        ///<param name="businessObject"></param>
        public void LoadTreeView(IBusinessObject businessObject)
        {
            LoadTreeView(businessObject, 0);
        }

        ///<summary>
        /// Loads the tree view with business object and all child objects.
        /// Expands the tree to the specified number of levels deep.
        ///</summary>
        ///<param name="businessObject"></param>
        ///<param name="levelsToExpand"></param>
        public void LoadTreeView(IBusinessObject businessObject, int levelsToExpand)
        {
            CleanUp();
            RootNodeBusinessObject = businessObject;
            //TODO brett 19 Mar 2009: This a hack of casting to a windows control will not work when we port this app to VWG.
           // ControlsHelper.SafeGui
            //    ((Control)_treeView, delegate
              //  {
                    TreeView.BeginUpdate();
                    TreeView.Nodes.Clear();
                    if (businessObject != null)
                        AddBusinessObjectNode(TreeView.Nodes, businessObject);
                    TreeView.EndUpdate();
                    ExpandLevels(TreeView.Nodes, levelsToExpand);
            //    });
           // Application.DoEvents();
        }

        ///<summary>
        /// Loads the tree view with all the child business objects of the business object.
        /// Loads the children of these business objects.
        /// The Tree will be expanded only the specified level.
        /// The tree will be loaded with child objects only to the number of levelsToDisplay.
        ///</summary>
        ///<param name="businessObject"></param>
        ///<param name="levelsToExpand"></param>
        ///<param name="levelsToDisplay"></param>
        public void LoadTreeView(IBusinessObject businessObject, int levelsToExpand, int levelsToDisplay)
        {
            _levelsToDisplay = levelsToDisplay;
            LoadTreeView(businessObject, levelsToExpand);
        }

        ///<summary>
        /// Loads the tree view with all teh business objects held in the businessObjectCollection.
        /// Loads the children of these business objects.
        /// The Tree will be expanded only the specified level.
        /// The tree will be loaded with child objects only to the number of levelsToDisplay.
        ///</summary>
        ///<param name="businessObjectCollection"></param>
        ///<param name="levelsToExpand"></param>
        ///<param name="levelsToDisplay"></param>
        public void LoadTreeView
            (IBusinessObjectCollection businessObjectCollection, int levelsToExpand, int levelsToDisplay)
        {
            _levelsToDisplay = levelsToDisplay;
            LoadTreeView(businessObjectCollection, levelsToExpand);
        }

        ///<summary>
        /// Loads the tree view with all the business objects held in the businessObjectCollection.
        /// Loads the children of these business objects.
        /// The Tree will be expanded only one level.
        ///</summary>
        ///<param name="businessObjectCollection"></param>
        public void LoadTreeView(IBusinessObjectCollection businessObjectCollection)
        {
            LoadTreeView(businessObjectCollection, 0);
        }

        ///<summary>
        /// Loads the tree view with all teh business objects held in the businessObjectCollection.
        /// Loads the children of these business objects.
        /// The Tree will be expanded only the specified level.
        ///</summary>
        ///<param name="businessObjectCollection"></param>
        ///<param name="levelsToExpand"></param>
        public void LoadTreeView(IBusinessObjectCollection businessObjectCollection, int levelsToExpand)
        {
            CleanUp();
           // ControlsHelper.SafeGui
           //     ((IControlHabanero)_treeView, delegate
            //    {
                    TreeView.BeginUpdate();
                    TreeView.Nodes.Clear();
                    if (businessObjectCollection != null)
                    {
                        AddCollectionNode(TreeView.Nodes, businessObjectCollection);
                    }
                    TreeView.EndUpdate();
                    ExpandLevels(TreeView.Nodes, levelsToExpand);
             //   });
        }

        ///<summary>
        /// Loads the Tree with all children of this relationship with no levels expanded.
        ///</summary>
        ///<param name="relationship"></param>
        public void LoadTreeView(IRelationship relationship)
        {
            LoadTreeView(relationship, 0);
        }
        ///<summary>
        /// Loads the Tree with all children of this relationship with specified no levels expanded.
        ///</summary>
        public void LoadTreeView(IRelationship relationship, int levelsToExpand)
        {
            CleanUp();
            //ControlsHelper.SafeGui
             //   ((IControlHabanero)_treeView, delegate
             //   {
                    TreeView.BeginUpdate();
                    TreeView.Nodes.Clear();
                    if (relationship != null)
                        AddRelationshipNode(TreeView.Nodes, relationship);
                    TreeView.EndUpdate();
                    ExpandLevels(TreeView.Nodes, levelsToExpand);
            //    });
            //Application.DoEvents();
        }

        ///<summary>
        /// Loads the tree view for all objects that are related objects of this relationship.
        /// Expands the tree view to the appropriate number of levels.
        /// Loads the relationship only to the number of levels specified by levels to display.
        /// This ensures that the tree loading can be controlled to only load the required number of children objects.
        ///</summary>
        ///<param name="relationship"></param>
        ///<param name="levelsToExpand"></param>
        ///<param name="levelsToDisplay"></param>
        public void LoadTreeView(IRelationship relationship, int levelsToExpand, int levelsToDisplay)
        {
            _levelsToDisplay = levelsToDisplay;
            LoadTreeView(relationship, levelsToExpand);
        }

        private void LoadChildrenNodes(ITreeNodeCollection nodes, IBusinessObject parent)
        {
            foreach (KeyValuePair<string, IRelationship> pair in GetVisibleRelationships(parent))
            {
                IRelationship relationship = pair.Value;
                if (!String.IsNullOrEmpty(pair.Key))
                {
                    ITreeNode node = SetupNode(nodes, relationship);
                    if (node == null) continue;
                    if (node.IsExpanded)
                    {
                        LoadChildNode(relationship);
                    }
                }
            }
        }

        private void LoadChildNode(IRelationship relationship)
        {
            if (relationship == null) return;
            NodeState nodeState = RelationshipNodes[relationship];
            if (nodeState.IsLoaded) return;

            ITreeNodeCollection nodes = nodeState.Node.Nodes;
            nodes.Clear();
            LoadRelationshipNode(relationship, nodes);
            nodeState.IsLoaded = true;
        }

        private void LoadRelationshipNode(IRelationship relationship, ITreeNodeCollection nodes)
        {
            if (relationship is IMultipleRelationship)
            {
                IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
                IBusinessObjectCollection children = multipleRelationship.BusinessObjectCollection;
                foreach (IBusinessObject businessObject in children.Clone())
                {
                    AddBusinessObjectNode(nodes, businessObject);
                }
            }
            else
            {
                ISingleRelationship singleRelationship = (ISingleRelationship)relationship;
                IBusinessObject businessObject = singleRelationship.GetRelatedObject();
                if (businessObject != null)
                {
                    AddBusinessObjectNode(nodes, businessObject);
                }
            }
        }

        private void RemoveRelationshipNode(IRelationship relationship)
        {
            this.RelationshipNodes.Remove(relationship);
            if (relationship is IMultipleRelationship)
            {
                IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
                IBusinessObjectCollection children = multipleRelationship.BusinessObjectCollection;
                ChildCollectionNodes.Remove(children);
                foreach (IBusinessObject businessObject in children.Clone())
                {
                    RemoveBusinessObjectNode(businessObject);
                }
            }
            else
            {
                ISingleRelationship singleRelationship = (ISingleRelationship)relationship;
                IBusinessObject businessObject = singleRelationship.GetRelatedObject();
                if (businessObject != null)
                {
                    RemoveBusinessObjectNode(businessObject);
                }
            }
        }

        private void AddBusinessObjectNode(ITreeNodeCollection nodes, IBusinessObject businessObject)
        {
            ITreeNode newNode = SetupNode(nodes, businessObject);
            if (newNode == null) return;
            if (newNode.IsExpanded)
            {
                LoadObjectNode(businessObject);
            }
        }


        private void AddCollectionNode
            (ITreeNodeCollection nodeCollection, IBusinessObjectCollection businessObjectCollection)
        {
            foreach (IBusinessObject businessObject in businessObjectCollection)
            {
                AddBusinessObjectNode(TreeView.Nodes, businessObject);
            }
        }

        private void AddRelationshipNode(ITreeNodeCollection nodes, IRelationship relationship)
        {
            ITreeNode newNode = SetupNode(nodes, relationship);
            if (newNode == null) return;
            if (newNode.IsExpanded)
            {
                LoadRelationshipNode(relationship, newNode.Nodes);
            }
        }

        private void LoadObjectNode(IBusinessObject businessObject)
        {
            if (businessObject == null) return;
            NodeState nodeState = ObjectNodes[businessObject];
            if (nodeState.IsLoaded) return;
            ITreeNodeCollection nodes = nodeState.Node.Nodes;
            nodes.Clear();
            LoadChildrenNodes(nodes, businessObject);
            nodeState.IsLoaded = true;
        }

        private ITreeNode SetupNode(ITreeNodeCollection nodes, object nodeTag)
        {
            if (nodeTag == null)
            {
                throw new ArgumentNullException("nodeTag");
            }
            IBusinessObject businessObject = nodeTag as IBusinessObject;
            if (businessObject != null) return SetupBusinessObjectNode(businessObject, nodes);

            IRelationship relationship = nodeTag as IRelationship;
            if (relationship != null) return SetupRelationshipNode(relationship, nodes);

            throw new InvalidCastException("'nodeTag' is not of a recognised type.");
        }

        private ITreeNode SetupRelationshipNode(IRelationship relationship, ITreeNodeCollection nodes)
        {
            bool isNewColTag = !RelationshipNodes.ContainsKey(relationship);
            ITreeNode node;
            NodeState nodeState;
            if (isNewColTag)
            {
                node = nodes.Add("");
                node.Collapse(false);
                if (_levelsToDisplay > -1 && node.Level > _levelsToDisplay)
                {
                    nodes.Remove(node);
                    return null;
                }
                nodeState = new NodeState(node);
                RelationshipNodes.Add(relationship, nodeState);
                SetupRelationshipNodeDummy(relationship, nodeState);
            }
            else
            {
                nodeState = RelationshipNodes[relationship];
                node = nodeState.Node;
            }
            DoSetupNodeWithRelationship(node, relationship);
            node.Tag = relationship;
            return node;
        }

        private void SetupRelationshipNodeDummy(IRelationship relationship, NodeState nodeState)
        {
            int childCount = 0;
            if (relationship is IMultipleRelationship)
            {
                IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
                IBusinessObjectCollection businessObjectCollection = multipleRelationship.BusinessObjectCollection;
                ChildCollectionNodes.Add(businessObjectCollection, nodeState);
                RegisterForBusinessObjectCollectionEvents(businessObjectCollection);
                childCount = businessObjectCollection.Count;
            }
            else
            {
                //TODO: Do something decent with Single Relationship Updated Event
                ISingleRelationship singleRelationship = (ISingleRelationship)relationship;
                if (singleRelationship.HasRelatedObject())
                {
                    childCount = 1;
                }
            }
            UpdateNodeDummy(nodeState, childCount);
        }

        private void DoSetupNodeWithRelationship(ITreeNode node, IRelationship relationship)
        {
            node.Text = GetRelationshipDescription(relationship);
            if (SetupNodeWithRelationship != null)
            {
                SetupNodeWithRelationship(node, relationship);
            }
        }

        private ITreeNode SetupBusinessObjectNode(IBusinessObject businessObject, ITreeNodeCollection nodes)
        {
            ITreeNode node;
            NodeState nodeState;
            if (!ObjectNodes.ContainsKey(businessObject))
            {
                node = nodes.Add("");
                node.Collapse(false);
                if (_levelsToDisplay > -1 && node.Level > _levelsToDisplay)
                {
                    nodes.Remove(node);
                    return null;
                }
                nodeState = new NodeState(node);
                nodeState.IsLoaded = false;
                UpdateNodeDummy(nodeState, GetVisibleRelationships(businessObject).Count);

                //LoadChildrenNodes(nodeState.Node.Nodes, businessObject);
                ObjectNodes.Add(businessObject, nodeState);
                //                LoadObjectNode(businessObject);
                RegisterForBusinessObjectEvents(businessObject);
            }
            else
            {
                nodeState = ObjectNodes[businessObject];
                node = nodeState.Node;
            }
            DoSetupNodeWithBusinessObject(node, businessObject);
            node.Tag = businessObject;
            return node;
        }

        private void DoSetupNodeWithBusinessObject(ITreeNode node, IBusinessObject businessObject)
        {
            string description = GetClassDescription(businessObject);
            log.Debug("Business Object Description : " + description);
            node.Text = description;
            if (SetupNodeWithBusinessObject != null)
            {
                SetupNodeWithBusinessObject(node, businessObject);
            }
        }

        private static void UpdateNodeDummy(NodeState nodeState, int childrenCount)
        {
            if (nodeState.IsLoaded) return;
            //Sets up a dummy node so that the tree view will show a + to expand but 
            // does not load the underlying objects untill required thus implementing lazy loading ;)
            if (nodeState.Node.Nodes.Count == 0 && childrenCount > 0)
            {
                //nodeState.Node.Nodes.Add("", "$DUMMY$");
                ITreeNode node = nodeState.Node.Nodes.Add("", "$DUMMY$");
                if (_levelsToDisplay > -1 && node.Level > _levelsToDisplay)
                {
                    nodeState.Node.Nodes.Remove(node);
                }
            }
        }

        #endregion //Loading

        #region Node Changes

        private void AddBusinessObjectToCollectionNode
            (IBusinessObjectCollection businessObjectCollection, IBusinessObject businessObject)
        {
            if (businessObjectCollection != null && ChildCollectionNodes.ContainsKey(businessObjectCollection))
            {
                NodeState nodeState = ChildCollectionNodes[businessObjectCollection];
                if (nodeState.IsLoaded)
                {
                    ITreeNode node = nodeState.Node;
                    AddBusinessObjectNode(node.Nodes, businessObject);
                }
                else
                {
                    UpdateNodeDummy(nodeState, businessObjectCollection.Count);
                }
            }
        }

        private void RemoveBusinessObjectFromCollectionNode
            (IBusinessObjectCollection businessObjectCollection, IBusinessObject businessObject)
        {
            if (businessObjectCollection != null && ChildCollectionNodes.ContainsKey(businessObjectCollection))
            {
                NodeState nodeState = ChildCollectionNodes[businessObjectCollection];
                if (nodeState.IsLoaded)
                {
                    RemoveBusinessObjectNode(businessObject);
                }
                else
                {
                    UpdateNodeDummy(nodeState, businessObjectCollection.Count);
                }
            }
        }

        private void RefreshBusinessObjectNode(IBusinessObject businessObject)
        {
            if (businessObject != null && ObjectNodes.ContainsKey(businessObject))
            {
                NodeState nodeState = ObjectNodes[businessObject];
                ITreeNode node = nodeState.Node;
                DoSetupNodeWithBusinessObject(node, businessObject);
                //node.Text = GetClassDescription(businessObject);
            }
        }

        private void RemoveBusinessObjectNode(IBusinessObject businessObject)
        {
            if (businessObject != null && ObjectNodes.ContainsKey(businessObject))
            {
                NodeState nodeState = ObjectNodes[businessObject];
                ITreeNode node = nodeState.Node;
                ITreeNode parentNode = node.Parent;
                RemoveNode(businessObject, node);

                IDictionary<string, IRelationship> relationships = GetVisibleRelationships(businessObject);
                foreach (KeyValuePair<string, IRelationship> relationshipPair in relationships)
                {
                    IRelationship relationship = relationshipPair.Value;
                    RemoveRelationshipNode(relationship);
                }
                if (parentNode != null)
                {
                    IBusinessObjectCollection businessObjectCollection = parentNode.Tag as IBusinessObjectCollection;
                    if (businessObjectCollection != null)
                    {
                        UpdateNodeDummy(nodeState, businessObjectCollection.Count);
                    }
                }
            }
        }

        private void RemoveNode(IBusinessObject businessObject, ITreeNode node)
        {
            try
            {
                node.Remove();
            }
            catch (System.ObjectDisposedException ex)
            {
                log.Debug
                    ("RemoveNode : cannot remove node for Business Object : " + businessObject.ToString()
                     + Environment.NewLine + " Error :" + ex.Message);
            }
            finally
            {
                try
                {
                    ObjectNodes.Remove(businessObject);
                }
                finally
                {
                    UnRegisterForBusinessObjectEvents(businessObject);
                }
            }
        }

        #endregion //Node Changes

        #region Node Expansion

        private void ExpandLevels(ITreeNodeCollection nodes, int expandLevels)
        {
            if (expandLevels <= 0) return;
            foreach (ITreeNode treeNode in nodes)
            {
                treeNode.Expand();
                ExpandNode(treeNode);
                this.ExpandLevels(treeNode.Nodes, expandLevels - 1);
            }
        }

        #endregion //Node Expansion

        #region Object Events
        /// <summary>
        /// Event handler when a Business Object is added to the business object collection
        /// </summary>
        protected virtual void BusinessObjectCollection_ChildAdded(object sender, BOEventArgs e)
        {
            try
            {
                IBusinessObjectCollection businessObjectCollection = sender as IBusinessObjectCollection;
                IBusinessObject businessObject = e.BusinessObject;

                AddBusinessObjectToCollectionNode(businessObjectCollection, businessObject);
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }
        /// <summary>
        /// Event handler when a Business Object is removed to the business object collection
        /// </summary>
        protected virtual void BusinessObjectCollection_ChildRemoved(object sender, BOEventArgs e)
        {
            try
            {
                IBusinessObjectCollection businessObjectCollection = sender as IBusinessObjectCollection;
                IBusinessObject businessObject = e.BusinessObject;
                TreeView.SelectedNode = null;
                RemoveBusinessObjectFromCollectionNode(businessObjectCollection, businessObject);
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }
        /// <summary>
        /// Event handler when a Business Object is updated in any way.
        /// </summary>
        protected virtual void BusinessObject_Updated(object sender, BOEventArgs e)
        {
            try
            {
                UpdateBusinessObject(e.BusinessObject);
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        private void UpdateBusinessObject(IBusinessObject businessObject)
        {
            RefreshBusinessObjectNode(businessObject);
        }
        /// <summary>
        /// Event handler when a Business Object is deleted
        /// </summary>
        protected virtual void BusinessObject_Deleted(object sender, BOEventArgs e)
        {
            try
            {
                IBusinessObject businessObject = sender as IBusinessObject;
                RemoveBusinessObjectNode(businessObject);
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        #endregion //Object Events

        #region TreeView Events

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ITreeNode newSelectedNode = TreeView.SelectedNode;
            //SetSelectedNode(newSelectedNode);
            IBusinessObject businessObject = newSelectedNode.Tag as IBusinessObject;
            if (businessObject != null)
            {
                FireBusinessObjectSelected(businessObject);
            } //else if (_selectCollectionsFirstViewableChild)
            //{
            //    IBusinessObjectCollection businessObjectCollection = newSelectedNode.Tag as IBusinessObjectCollection;
            //    if (businessObjectCollection != null && businessObjectCollection.Count > 0)
            //    {
            //        businessObject = businessObjectCollection.ChildrenList()[0];
            //        FireBusinessObjectSelected(businessObject);
            //    }
            //}
        }

        //private void SetSelectedNode(ITreeNode newSelectedNode)
        //{
        //    if (_selectedNode != null)
        //    {
        //        _selectedNode.BackColor = _treeView.BackColor;
        //        _selectedNode.ForeColor = _treeView.ForeColor;
        //    }
        //    _selectedNode = newSelectedNode;
        //    if (_selectedNode != null)
        //    {
        //        _selectedNode.BackColor = SystemColors.Highlight;
        //        _selectedNode.ForeColor = SystemColors.HighlightText;
        //    }
        //}

        private void TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            ExpandNode(e.Node);
        }

        private void ExpandNode(ITreeNode node)
        {
            object nodeTag = node.Tag;
            IBusinessObject businessObject = nodeTag as IBusinessObject;
            IRelationship relationship = nodeTag as IRelationship;

            if (relationship != null)
            {
                LoadChildNode(relationship);
            }
            else if (businessObject != null)
            {
                LoadObjectNode(businessObject);
            }
        }

        #endregion //TreeView Events

        private void FireBusinessObjectSelected(IBusinessObject businessObject)
        {
            if (_preventSelectionChanged) return;
            if (BusinessObjectSelected != null)
            {
                BusinessObjectSelected(this, new BOEventArgs(businessObject));
            }
        }

        ///<summary>
        /// Sets the business object's node as the selected node.
        ///</summary>
        ///<param name="businessObject"></param>
        public void SelectObject(IBusinessObject businessObject)
        {
            if (ObjectNodes.ContainsKey(businessObject))
            {
                NodeState nodeState = ObjectNodes[businessObject];
                ITreeNode node = nodeState.Node;
                if (TreeView.SelectedNode != node)
                {
                    _preventSelectionChanged = true;
                    TreeView.SelectedNode = node;
                    _preventSelectionChanged = false;
                }
            }
        }

        ///<summary>
        /// Cleans up the tree view by derigistering for all BusinessObject events and removing 
        /// all nodes.
        ///</summary>
        public void CleanUp()
        {
            foreach (KeyValuePair<IBusinessObject, NodeState> objectNode in ObjectNodes)
            {
                IBusinessObject businessObject = objectNode.Key;
                UnRegisterForBusinessObjectEvents(businessObject);
            }
            ObjectNodes.Clear();
            foreach (KeyValuePair<IBusinessObjectCollection, NodeState> collectionNode in ChildCollectionNodes)
            {
                IBusinessObjectCollection businessObjectCollection = collectionNode.Key;
                UnRegisterForBusinessObjectCollectionEvents(businessObjectCollection);
            }
            ChildCollectionNodes.Clear();
            RelationshipNodes.Clear();
            RootNodeBusinessObject = null;
        }
        /// <summary>
        /// Returns the TreeNode associated with a particular business object.
        /// </summary>
        /// <param name="businessObject"></param>
        /// <returns></returns>
        public ITreeNode GetBusinessObjectTreeNode(IBusinessObject businessObject)
        {
            NodeState nodeState = GetBusinessObjectNodeState(businessObject);
            return nodeState != null ? nodeState.Node : null;
        }

        private NodeState GetBusinessObjectNodeState(IBusinessObject businessObject)
        {
            if (ObjectNodes.ContainsKey(businessObject))
            {
                return ObjectNodes[businessObject];
            }
            return null;
        }

        ///<summary>
        /// Set the visibility for the node associated with a particular business object
        /// in the tree view.
        ///</summary>
        ///<param name="businessObject"></param>
        ///<param name="visible"></param>
        public void SetVisibility(IBusinessObject businessObject, bool visible)
        {
            NodeState nodeState = GetBusinessObjectNodeState(businessObject);
            if (visible)
            {
                int index = FindPositionIndexOf(businessObject);
                nodeState.Connect(index);
            }
            else
            {
                nodeState.Disconnect();
            }
        }

        private int FindPositionIndexOf(IBusinessObject businessObject)
        {
            NodeState nodeState = GetBusinessObjectNodeState(businessObject);
            ITreeNode parentNode = nodeState.ParentNode;
            IBusinessObjectCollection businessObjectCollection = GetNodeCollection(parentNode);
            if (businessObjectCollection == null) return 0;
            int proposedIndex = businessObjectCollection.IndexOf(businessObject);
            int actualIndex = 0;
            for (int i = 0; i < proposedIndex; i++)
            {
                IBusinessObject nodeBo = businessObjectCollection[i];
                NodeState boNodeState = GetBusinessObjectNodeState(nodeBo);
                if (boNodeState.IsConnected()) actualIndex++;
            }
            return actualIndex;
        }

        private IBusinessObjectCollection GetNodeCollection(ITreeNode node)
        {
            foreach (KeyValuePair<IBusinessObjectCollection, NodeState> pair in ChildCollectionNodes)
            {
                if (pair.Value.Node == node)
                {
                    return pair.Key;
                }
            }
            return null;
        }
    }

}
