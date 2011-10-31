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
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Faces.Test.VWG.HabaneroControls;
using Habanero.Faces.VWG;
using Habanero.Faces.VWG.Grid;
using Habanero.Test;
using Habanero.Test.BO;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.VWG.Grid
{
    [TestFixture]
    public class TestBOGridAndEditorControlVWG
    {
        private const string CUSTOM_UIDEF_NAME = "custom1";
        private static IClassDef GetCustomClassDef()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDef_NoOrganisationRelationship();
            IClassDef classDef = OrganisationTestBO.LoadDefaultClassDef();
            IUIGrid originalGridDef = classDef.UIDefCol["default"].UIGrid;
            UIGrid extraGridDef = ((UIGrid)originalGridDef).Clone();
            // UIGridColumn extraColumn = originalGridDef[0].Clone();
            // extraGridDef.Add(extraColumn);
            extraGridDef.Remove(extraGridDef[extraGridDef.Count - 1]);
            // UIGridColumn extraColumn = new UIGridColumn("HABANERO_OBJECTID", "ProjectAssemblyInfoID", typeof(System.VWGdows.Forms.DataGridViewTextBoxColumn), true, 100, UIGridColumn.PropAlignment.right, null);
            // extraGridDef.Add(extraColum
            IUIForm originalformDef = classDef.UIDefCol["default"].UIForm;
            IUIForm formDef = ((UIForm)originalformDef).Clone();
            UIDef extraUIDef = new UIDef(CUSTOM_UIDEF_NAME, formDef, extraGridDef);
            classDef.UIDefCol.Add(extraUIDef);
            return classDef;
        }

        private static BOGridAndEditorControlVWG<TBusinessObject> CreateGridAndBOEditorControlVWGCP<TBusinessObject>() where TBusinessObject : class, IBusinessObject
        {
            IBOEditorControl iboEditorControl = new BOEditorControlStubVWG();
            return new BOGridAndEditorControlVWG<TBusinessObject>(GetControlFactory(), iboEditorControl);
        }
        private static BOGridAndEditorControlVWG<OrganisationTestBO> CreateGridAndBOEditorControlVWG()
        {
            IBOEditorControl iboEditorControl = new BOEditorControlStubVWG();
            return new BOGridAndEditorControlVWG<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
        }

        //private static BOGridAndEditorControlVWG<ProjectAssemblyInfo> CreateGridAndBOEditorControlVWG_ProjectAssemblyInfos()
        //{
        //    IBOEditorControl businessObjectControl = new TestComboBox.BOEditorControlStubVWG();
        //    return new BOGridAndEditorControlVWG<ProjectAssemblyInfo>(GetControlFactory(), businessObjectControl);
        //}

        private static void AssertSelectedBusinessObject
            (OrganisationTestBO businessObjectInfo,
             BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG)
        {
            Assert.AreSame(businessObjectInfo, andBOGridAndEditorControlVWG.GridControl.SelectedBusinessObject);
            Assert.AreSame
                (businessObjectInfo, andBOGridAndEditorControlVWG.IBOEditorControl.BusinessObject,
                 "Selected BO in Grid should be loaded in the BoControl");
            Assert.AreSame(businessObjectInfo, andBOGridAndEditorControlVWG.CurrentBusinessObject);
        }

        private static IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void TestConstructor_FailsIfBOControlNull()
        {
            // ---------------Set up test pack-------------------
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new BOGridAndEditorControlVWG<OrganisationTestBO>
                    (GetControlFactory(), (IBOEditorControl)null);

                Assert.Fail("Null BOControl should be prevented");
            }
            //    ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("iboEditorControl", ex.ParamName);
            }
            //---------------Test Result -----------------------
        }

        [Test]
        public void TestConstructor_FailsIfControlFactoryNull()
        {
            //---------------Set up test pack-------------------
            IBOEditorControl iboEditorControl = new BOEditorControlStubVWG();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOGridAndEditorControlVWG<OrganisationTestBO>(null, iboEditorControl);

                Assert.Fail("Null ControlFactory should be prevented");
            }
            // ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("controlFactory", ex.ParamName);
            }
            //   ---------------Test Result -----------------------
        }

        [Test]
        [Ignore("Brett is working on this : Brett 03 Mar 2009:")] //TODO Brett 03 Mar 2009: Brett is working on this
        public void TestConstructor_NonGeneric()
        {
            //---------------Set up test pack-------------------
            ClassDef def = (ClassDef)GetCustomClassDef();
            IBOEditorControl iboEditorControl = new BOEditorControlStubVWG();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(def.GetUIDef(CUSTOM_UIDEF_NAME));
            Assert.IsNotNull(def.GetUIDef(CUSTOM_UIDEF_NAME).UIForm);
            //---------------Execute Test ----------------------
            IBOGridAndEditorControl iboGridAndEditorControlVWG = new BOGridAndEditorControlVWG
                (GetControlFactory(), def, CUSTOM_UIDEF_NAME);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, iboGridAndEditorControlVWG.Controls.Count);
            Assert.IsInstanceOf(typeof(IUserControlHabanero), iboGridAndEditorControlVWG);
            Assert.IsInstanceOf
                (typeof(IBOEditorControl), iboGridAndEditorControlVWG.Controls[0]);
            Assert.IsInstanceOf(typeof(IReadOnlyGridControl), iboGridAndEditorControlVWG.Controls[1]);
            Assert.IsInstanceOf(typeof(IButtonGroupControl), iboGridAndEditorControlVWG.Controls[2]);
            Assert.AreSame(iboEditorControl, iboGridAndEditorControlVWG.IBOEditorControl);
            Assert.IsFalse(iboEditorControl.Enabled);
        }

        [Test]
        public void TestConstructor_NonGeneric_NullClassDef_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOGridAndEditorControlVWG(GetControlFactory(), (ClassDef)null, CUSTOM_UIDEF_NAME);
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("classDef", ex.ParamName);
            }
        }

        [Test]
        public void TestConstructor_NonGeneric_NullControlFactory_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IClassDef def = GetCustomClassDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOGridAndEditorControlVWG(null, def, CUSTOM_UIDEF_NAME);
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("controlFactory", ex.ParamName);
            }
        }

        [Test]
        [Ignore("Brett is working on this : Brett 03 Mar 2009:")] //TODO Brett 03 Mar 2009: Brett is working on this
        public void TestConstructor_NonGeneric_NoFormDefDefinedForUIDef_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IClassDef def = GetCustomClassDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOGridAndEditorControlVWG(GetControlFactory(), def, CUSTOM_UIDEF_NAME);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                string expectedDeveloperMessage = "The 'BusinessObjectControl";
                StringAssert.Contains(expectedDeveloperMessage, ex.Message);
                expectedDeveloperMessage = "' could not be created since the the uiDef '" + CUSTOM_UIDEF_NAME
                                           + "' in the classDef '" + def.ClassNameFull
                                           + "' does not have a UIForm defined";
                StringAssert.Contains(expectedDeveloperMessage, ex.Message);
                //
                //                string expectedDeveloperMessage = "The 'BusinessObjectControl' could not be created since the the uiDef '" + CUSTOM_UIDEF_NAME +
                //                                        "' in the classDef '" + def.ClassNameFull + "' does not have a UIForm defined";
                //                StringAssert.Contains(expectedDeveloperMessage, ex.Message);
            }
        }

        [Ignore(" In the process of removing this class")] //TODO  15 Mar 2009:
        [Test]
        public void TestConstructor_NonGeneric_DefinedUIDefDoesNotExistForDef_ShouldRiaseError()
        {
            //---------------Set up test pack-------------------
            IClassDef def = GetCustomClassDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string uidDefDoesnotexist = "";
            try
            {
                uidDefDoesnotexist = "DoesNotExist";
                new BOGridAndEditorControlVWG(GetControlFactory(), def, uidDefDoesnotexist);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                string expectedDeveloperMessage = "The 'BusinessObjectControl";
                StringAssert.Contains(expectedDeveloperMessage, ex.Message);
                expectedDeveloperMessage = "' could not be created since the the uiDef '" + uidDefDoesnotexist
                                           + "' does not exist in the classDef for '" + def.ClassNameFull + "'";
                StringAssert.Contains(expectedDeveloperMessage, ex.Message);
            }
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            IBOEditorControl iboEditorControl = new BOEditorControlStubVWG();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG =
                new BOGridAndEditorControlVWG<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, andBOGridAndEditorControlVWG.Controls.Count);
            Assert.IsInstanceOf<IUserControlHabanero>(andBOGridAndEditorControlVWG);
            Assert.IsInstanceOf<IBOEditorControl>(andBOGridAndEditorControlVWG.Controls[0]);
            Assert.IsInstanceOf<IReadOnlyGridControl>(andBOGridAndEditorControlVWG.Controls[1]);
            Assert.IsInstanceOf<IButtonGroupControl>(andBOGridAndEditorControlVWG.Controls[2]);
            Assert.AreSame(iboEditorControl, andBOGridAndEditorControlVWG.IBOEditorControl);
            Assert.IsFalse(iboEditorControl.Enabled);
        }

        [Test]
        public void TestConstructor_UsingCustomUIDefName()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            IBOEditorControl iboEditorControl = new BOEditorControlStubVWG();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG =
                new BOGridAndEditorControlVWG<OrganisationTestBO>
                    (GetControlFactory(), iboEditorControl, CUSTOM_UIDEF_NAME);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, andBOGridAndEditorControlVWG.Controls.Count);
            Assert.IsInstanceOf<IUserControlHabanero>(andBOGridAndEditorControlVWG);
            Assert.IsInstanceOf<IBOEditorControl>(andBOGridAndEditorControlVWG.Controls[0]);
            Assert.IsInstanceOf<IReadOnlyGridControl>(andBOGridAndEditorControlVWG.Controls[1]);
            Assert.IsInstanceOf<IButtonGroupControl>(andBOGridAndEditorControlVWG.Controls[2]);
            Assert.AreSame(iboEditorControl, andBOGridAndEditorControlVWG.IBOEditorControl);
        }

        [Test]
        public void Test_Construct()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = GetCustomClassDef();
            IBOEditorControl iboEditorControl = GetControlFactory().CreateBOEditorControl(classDef);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            BOGridAndEditorControlVWG control =
                new BOGridAndEditorControlVWG(GetControlFactory(), iboEditorControl, classDef, "default");
            //---------------Test Result -----------------------
            Assert.AreEqual(2, control.Controls.Count);
            Assert.IsInstanceOf<IPanel>(control.Controls[0]);
            Assert.IsInstanceOf<IReadOnlyGridControl>(control.Controls[1]);
        }

        [Test]
        public void Test_Panel_Construct()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = GetCustomClassDef();
            IBOEditorControl iboEditorControl = GetControlFactory().CreateBOEditorControl(classDef);
            BOGridAndEditorControlVWG control =
                new BOGridAndEditorControlVWG(GetControlFactory(), iboEditorControl, classDef, "default");
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<IPanel>(control.Controls[0]);
            //---------------Execute Test ----------------------
            IPanel panel = (IPanel)control.Controls[0];
            //---------------Test Result -----------------------
            Assert.AreEqual(2, panel.Controls.Count);
            Assert.IsInstanceOf<IBOEditorControl>(panel.Controls[0]);
            Assert.IsInstanceOf<IButtonGroupControl>(panel.Controls[1]);
        }

        public static int GetGridWidthToFitColumns(IGridBase grid)
        {
            int width = 0;
            if (grid.RowHeadersVisible)
            {
                width = grid.RowHeadersWidth;
            }
            foreach (IDataGridViewColumn column in grid.Columns)
            {
                if (column.Visible) width += column.Width;
            }
            return width;
        }

        [Test]
        public void TestGridConstruction()
        {
            //  ---------------Set up test pack-------------------
            GetCustomClassDef();
            IBOEditorControl iboEditorControl = new BOEditorControlStubVWG();
            // ---------------Assert Precondition----------------

            // ---------------Execute Test ----------------------
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG =
                new BOGridAndEditorControlVWG<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
            //  ---------------Test Result -----------------------
            IGridControl readOnlyGridControl = andBOGridAndEditorControlVWG.GridControl;
            Assert.IsNotNull(readOnlyGridControl);
            Assert.IsFalse(readOnlyGridControl.Buttons.Visible);
            Assert.IsFalse(readOnlyGridControl.FilterControl.Visible);
            Assert.IsNull(readOnlyGridControl.Grid.BusinessObjectCollection);
            int expectedWidth = GetGridWidthToFitColumns(readOnlyGridControl.Grid) + 2;
            Assert.AreEqual(expectedWidth, readOnlyGridControl.Width);
        }

        [Test]
        public void TestGridWithCustomClassDef()
        {
            //  ---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IClassDef classDef = GetCustomClassDef();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            organisationTestBOS.ClassDef = classDef;
            IBOEditorControl iboEditorControl = new BOEditorControlStubVWG();

            // ---------------Assert Precondition----------------
            Assert.IsTrue(classDef.UIDefCol.Count >= 2);
            // ---------------Execute Test ----------------------
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG =
                new BOGridAndEditorControlVWG<OrganisationTestBO>
                    (GetControlFactory(), iboEditorControl, CUSTOM_UIDEF_NAME);
            //---------------Test Result -----------------------
            Assert.AreEqual(CUSTOM_UIDEF_NAME, andBOGridAndEditorControlVWG.GridControl.UiDefName);
        }


        private static BusinessObjectCollection<OrganisationTestBO> CreateSavedOrganisationTestBOSCollection()
        {
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS =
                new BusinessObjectCollection<OrganisationTestBO>();
            organisationTestBOS.Add(OrganisationTestBO.CreateSavedOrganisation());
            organisationTestBOS.Add(OrganisationTestBO.CreateSavedOrganisation());
            organisationTestBOS.Add(OrganisationTestBO.CreateSavedOrganisation());
            organisationTestBOS.Add(OrganisationTestBO.CreateSavedOrganisation());
            return organisationTestBOS;
        }

        [Test]
        public void TestButtonControlConstruction()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            IBOEditorControl iboEditorControl = new BOEditorControlStubVWG();
            //---------------Assert Precondition----------------

            //  ---------------Execute Test ----------------------
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG =
                new BOGridAndEditorControlVWG<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
            //  ---------------Test Result -----------------------
            IButtonGroupControl buttonGroupControl = andBOGridAndEditorControlVWG.ButtonGroupControl;
            Assert.IsNotNull(buttonGroupControl);
            Assert.AreEqual(4, buttonGroupControl.Controls.Count);
            Assert.AreEqual("Cancel", buttonGroupControl.Controls[0].Text);
            Assert.AreEqual("Save", buttonGroupControl.Controls[1].Text);
            Assert.AreEqual("Delete", buttonGroupControl.Controls[2].Text);
            Assert.AreEqual("New", buttonGroupControl.Controls[3].Text);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_InitialSelection_NoItems()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
                new BusinessObjectCollection<OrganisationTestBO>();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            // ---------------Assert Precondition----------------
            Assert.AreEqual(0, businessObjectInfos.Count);
            // ---------------Execute Test ----------------------
            andBOGridAndEditorControlVWG.BusinessObjectCollection = businessObjectInfos;
            // ---------------Test Result -----------------------
            IGridControl readOnlyGridControl = andBOGridAndEditorControlVWG.GridControl;
            Assert.AreEqual(businessObjectInfos.Count, readOnlyGridControl.Grid.Rows.Count);
            AssertSelectedBusinessObject(null, andBOGridAndEditorControlVWG);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_FirstItemIsSelectedAndControlGetsBO()
        {
            // ---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GetCustomClassDef();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            // ---------------Execute Test ----------------------
            andBOGridAndEditorControlVWG.BusinessObjectCollection = businessObjectInfos;
            // ---------------Test Result -----------------------
            AssertSelectedBusinessObject(businessObjectInfos[0], andBOGridAndEditorControlVWG);
        }

        [Test]
        public void Test_SelectBusinessObject_ChangesBOInBOControl()
        {
            //   ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            IGridControl readOnlyGridControl = andBOGridAndEditorControlVWG.GridControl;
            andBOGridAndEditorControlVWG.BusinessObjectCollection = businessObjectInfos;
            //   ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            AssertSelectedBusinessObject(businessObjectInfos[0], andBOGridAndEditorControlVWG);
            // ---------------Execute Test ----------------------
            readOnlyGridControl.SelectedBusinessObject = businessObjectInfos[1];
            //  ---------------Test Result -----------------------
            AssertSelectedBusinessObject(businessObjectInfos[1], andBOGridAndEditorControlVWG);
        }

        [Test]
        public void Test_SetBusinessObjectCollection()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            //---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            Assert.AreEqual(0, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            andBOGridAndEditorControlVWG.BusinessObjectCollection = businessObjectInfos;
            //---------------Test Result -----------------------
            Assert.AreEqual(businessObjectInfos.Count, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            Assert.AreNotEqual(0, andBOGridAndEditorControlVWG.GridControl.Grid.Columns.Count);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_ToNull()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(0, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            // ---------------Execute Test ----------------------
            try
            {
                andBOGridAndEditorControlVWG.BusinessObjectCollection = null;
                //   ---------------Test Result -----------------------
                Assert.Fail("Error should have been thrown");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("value", ex.ParamName);
            }
        }

        [Test]
        public void TestBOControlDisabledWhenGridIsCleared()
        {
            //  ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            andBOGridAndEditorControlVWG.BusinessObjectCollection = businessObjectInfos;
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            //   ---------------Execute Test ----------------------
            andBOGridAndEditorControlVWG.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            //  ---------------Test Result -----------------------
            Assert.AreEqual(0, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            AssertSelectedBusinessObject(null, andBOGridAndEditorControlVWG);
            Assert.IsFalse(andBOGridAndEditorControlVWG.IBOEditorControl.Enabled);
        }

        [Test]
        public void TestBOControlEnabledWhenSelectedBOIsChanged()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            andBOGridAndEditorControlVWG.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            AssertSelectedBusinessObject(null, andBOGridAndEditorControlVWG);
            Assert.IsFalse(andBOGridAndEditorControlVWG.IBOEditorControl.Enabled);
            //  ---------------Execute Test ----------------------
            andBOGridAndEditorControlVWG.BusinessObjectCollection = businessObjectInfos;
            //  ---------------Test Result -----------------------
            Assert.IsTrue(andBOGridAndEditorControlVWG.IBOEditorControl.Enabled);
        }

        [Test]
        public void TestNewButtonDisabledUntilCollectionSet()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            IButton newButton = andBOGridAndEditorControlVWG.ButtonGroupControl["New"];
            //  ---------------Assert Precondition----------------
            Assert.IsFalse(newButton.Enabled);
            //  ---------------Execute Test ----------------------
            andBOGridAndEditorControlVWG.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            //  ---------------Test Result -----------------------
            Assert.IsTrue(newButton.Enabled);
        }

        [Test]
        public void TestNewButtonClickedCreatesBO_EmptyCollection()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
                new BusinessObjectCollection<OrganisationTestBO>();
            andBOGridAndEditorControlVWG.BusinessObjectCollection = businessObjectInfos;
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(0, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            Assert.AreEqual(0, businessObjectInfos.Count);
            //  ---------------Execute Test ----------------------
            andBOGridAndEditorControlVWG.ButtonGroupControl["New"].PerformClick();
            // ---------------Test Result -----------------------
            Assert.AreEqual(1, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            Assert.AreEqual(1, businessObjectInfos.Count);
            AssertSelectedBusinessObject(businessObjectInfos[0], andBOGridAndEditorControlVWG);
            Assert.IsTrue(andBOGridAndEditorControlVWG.IBOEditorControl.Enabled);
            Assert.IsTrue(andBOGridAndEditorControlVWG.ButtonGroupControl["Cancel"].Enabled);
        }

        [Ignore("Does not work for tests in VWG")] //TODO Brett 07 Jul 2010: Ignored Test - Does not work for tests in VWG
        [Test]
        public void TestNewButtonClickedCreatesBO_ExistingCollection()
        {
            //---------------Set up test pack-------------------
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            IFormHabanero form = GetControlFactory().CreateForm();
            form.Controls.Add(andBOGridAndEditorControlVWG);
            form.Show();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            andBOGridAndEditorControlVWG.BusinessObjectCollection = organisationTestBOS;
            //   ---------------Assert Precondition----------------
            Assert.AreEqual(4, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            Assert.AreEqual(4, organisationTestBOS.Count);
            Assert.IsFalse(andBOGridAndEditorControlVWG.IBOEditorControl.Focused);
            //  ---------------Execute Test ----------------------
            andBOGridAndEditorControlVWG.ButtonGroupControl["New"].PerformClick();

            // ---------------Test Result -----------------------
            Assert.AreEqual(5, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            Assert.AreEqual(5, organisationTestBOS.Count);
            Assert.IsTrue(organisationTestBOS[4].Status.IsNew);
            AssertSelectedBusinessObject(organisationTestBOS[4], andBOGridAndEditorControlVWG);
            Assert.IsTrue(andBOGridAndEditorControlVWG.IBOEditorControl.Enabled);
            // TODO: this line passes on PC's, but not when run on the server
            //Assert.IsTrue(andBOGridAndEditorControlVWG.IBOEditorControl.Focused);
            //Assert.IsTrue(andBOGridAndEditorControlVWG.ButtonGroupControl["Cancel"].Enabled);
        }

        [Test]
        public void TestDeleteButtonDisabledAtConstruction()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IBOEditorControl iboEditorControl = new BOEditorControlStubVWG();
            //  ---------------Assert Precondition----------------

            // ---------------Execute Test ----------------------
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG =
                new BOGridAndEditorControlVWG<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
            //---------------Test Result -----------------------
            IButton deleteButton = andBOGridAndEditorControlVWG.ButtonGroupControl["Delete"];
            Assert.IsFalse(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonEnabledWhenBOSelected()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            andBOGridAndEditorControlVWG.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            IButton deleteButton = andBOGridAndEditorControlVWG.ButtonGroupControl["Delete"];
            //---------------Assert Precondition----------------
            Assert.IsFalse(deleteButton.Enabled);
            //---------------Execute Test ----------------------
            andBOGridAndEditorControlVWG.BusinessObjectCollection = businessObjectInfos;
            //---------------Test Result -----------------------
            AssertSelectedBusinessObject(businessObjectInfos[0], andBOGridAndEditorControlVWG);
            Assert.IsTrue(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonDisabledWhenControlHasNoBO()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            andBOGridAndEditorControlVWG.BusinessObjectCollection = businessObjectInfos;
            IButton deleteButton = andBOGridAndEditorControlVWG.ButtonGroupControl["Delete"];
            // ---------------Assert Precondition----------------
            Assert.IsTrue(deleteButton.Enabled);
            //  ---------------Execute Test ----------------------
            andBOGridAndEditorControlVWG.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            // ---------------Test Result -----------------------
            AssertSelectedBusinessObject(null, andBOGridAndEditorControlVWG);
            Assert.IsFalse(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonDisabledWhenNewObjectAdded()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            andBOGridAndEditorControlVWG.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            IButton deleteButton = andBOGridAndEditorControlVWG.ButtonGroupControl["Delete"];
            IButton newButton = andBOGridAndEditorControlVWG.ButtonGroupControl["New"];
            // ---------------Assert Precondition----------------
            Assert.AreEqual(0, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            Assert.IsFalse(deleteButton.Enabled);
            //  ---------------Execute Test ----------------------
            newButton.PerformClick();
            //  ---------------Test Result -----------------------
            Assert.AreEqual(1, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            Assert.IsFalse(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonEnabledWhenOldObjectSelected()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            IButton deleteButton = andBOGridAndEditorControlVWG.ButtonGroupControl["Delete"];
            // ---------------Assert Precondition----------------
            Assert.AreEqual(0, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            Assert.IsFalse(deleteButton.Enabled);
            //  ---------------Execute Test ----------------------
            andBOGridAndEditorControlVWG.BusinessObjectCollection = businessObjectInfos;
            //  ---------------Test Result -----------------------
            Assert.AreEqual(4, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            Assert.IsTrue(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonDeletesCurrentBO()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            andBOGridAndEditorControlVWG.BusinessObjectCollection = organisationTestBOS;
            IButton deleteButton = andBOGridAndEditorControlVWG.ButtonGroupControl["Delete"];
            OrganisationTestBO currentBO = organisationTestBOS[0];
            //---------------Assert Precondition----------------
            AssertSelectedBusinessObject(currentBO, andBOGridAndEditorControlVWG);
            Assert.IsFalse(currentBO.Status.IsDeleted);
            Assert.AreEqual(4, organisationTestBOS.Count);
            //---------------Execute Test ----------------------
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(currentBO.Status.IsDeleted);
            Assert.IsFalse(currentBO.Status.IsDirty);
            Assert.AreEqual(3, organisationTestBOS.Count);
            Assert.IsFalse(organisationTestBOS.Contains(currentBO));
        }

        [Test]
        public void TestDeleteButton_ControlsUpdated()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            andBOGridAndEditorControlVWG.BusinessObjectCollection = organisationTestBOS;
            IButton deleteButton = andBOGridAndEditorControlVWG.ButtonGroupControl["Delete"];
            OrganisationTestBO currentBO = organisationTestBOS[0];
            OrganisationTestBO otherBO = organisationTestBOS[1];
            //---------------Assert Precondition----------------
            AssertSelectedBusinessObject(currentBO, andBOGridAndEditorControlVWG);
            Assert.AreEqual(4, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            AssertSelectedBusinessObject(otherBO, andBOGridAndEditorControlVWG);
            Assert.AreEqual(3, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
        }

        //         Tests a unique set of circumstances
        [Test]
        public void TestDeleteSelectsPreviousRow_NewTypeNewCancelDelete()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            andBOGridAndEditorControlVWG.BusinessObjectCollection = organisationTestBOS;
            IButton newButton = andBOGridAndEditorControlVWG.ButtonGroupControl["New"];
            IButton deleteButton = andBOGridAndEditorControlVWG.ButtonGroupControl["Delete"];
            IButton cancelButton = andBOGridAndEditorControlVWG.ButtonGroupControl["Cancel"];
#pragma warning disable 168
            OrganisationTestBO currentBO = andBOGridAndEditorControlVWG.CurrentBusinessObject;
#pragma warning restore 168
            //---------------Assert Precondition----------------
            Assert.AreEqual(4, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            organisationTestBOS.SaveAll();
            //---------------Execute Test ----------------------
            newButton.PerformClick();
            newButton.PerformClick();
            cancelButton.PerformClick();
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(4, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            // Assert.AreSame(currentBO, BOGridAndEditorControlVWG.CurrentBusinessObject);
        }

        [Test]
        public void TestCancelButton_DisabledOnConstruction()
        {
            //  ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            andBOGridAndEditorControlVWG.BusinessObjectCollection = businessObjectInfos;
            IButton cancelButton = andBOGridAndEditorControlVWG.ButtonGroupControl["Cancel"];
            OrganisationTestBO currentBO = businessObjectInfos[0];
            //---------------Assert Precondition----------------
            Assert.IsFalse(currentBO.Status.IsDirty);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(cancelButton.Enabled);
        }

        [Ignore(" This is currently not doing anything - what should it be doing: Brett 03 Mar 2009:")] //Brett  27 Feb 2009:
        [Test]
        public void TestCancelButton_EnabledWhenObjectEdited()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            andBOGridAndEditorControlVWG.BusinessObjectCollection = organisationTestBOS;
            //            IButton cancelButton = BOGridAndEditorControlVWG.ButtonGroupControl["Cancel"];
            OrganisationTestBO currentBO = organisationTestBOS[0];
            // ---------------Assert Precondition----------------
            Assert.IsFalse(currentBO.Status.IsDirty);
            // ---------------Execute Test ----------------------
            //currentBO.BusinessObjectName = TestUtils.GetRandomString();
            // ---------------Test Result -----------------------
            //Assert.IsTrue(currentBO.Status.IsDirty);
            //            Assert.IsTrue(cancelButton.Enabled);
        }

        [Test]
        public void TestCancelButton_ClickRestoresSavedObject()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            andBOGridAndEditorControlVWG.BusinessObjectCollection = businessObjectInfos;
            IButton cancelButton = andBOGridAndEditorControlVWG.ButtonGroupControl["Cancel"];

            OrganisationTestBO currentBO = businessObjectInfos[0];

            //  ---------------Execute Test ----------------------
            cancelButton.PerformClick();
            // ---------------Test Result -----------------------
            Assert.IsFalse(currentBO.Status.IsDirty);
            Assert.IsFalse(cancelButton.Enabled);
        }

        [Test]
        public void TestCancelButton_ClickRemovesNewObject_OnlyItemInGrid()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            BusinessObjectCollection<OrganisationTestBO> collection = new BusinessObjectCollection<OrganisationTestBO>();
            andBOGridAndEditorControlVWG.BusinessObjectCollection = collection;
            IButton cancelButton = andBOGridAndEditorControlVWG.ButtonGroupControl["Cancel"];
            IButton newButton = andBOGridAndEditorControlVWG.ButtonGroupControl["New"];

            newButton.PerformClick();
            //---------------Assert Precondition----------------
            Assert.IsTrue(cancelButton.Enabled);
            Assert.AreEqual(1, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            Assert.IsTrue(andBOGridAndEditorControlVWG.IBOEditorControl.Enabled);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, collection.Count, "The new item should be removed from the collection");
            Assert.AreEqual(0, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            Assert.IsFalse(andBOGridAndEditorControlVWG.IBOEditorControl.Enabled);
        }

        [Ignore("Does not work for tests in VWG")] //TODO Brett 07 Jul 2010: Ignored Test - Does not work for tests in VWG
        [Test]
        public void TestCancelButton_ClickRemovesNewObject_OnlyItemInGrid_CompositionRelationship()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef organisationClassDef = OrganisationTestBO.LoadDefaultClassDef();
            IRelationshipDef cpRelationship = organisationClassDef.RelationshipDefCol["ContactPeople"];
            cpRelationship.RelationshipType = RelationshipType.Composition;
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();

            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlVWG<ContactPersonTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWGCP<ContactPersonTestBO>();
            BusinessObjectCollection<ContactPersonTestBO> people = new OrganisationTestBO().ContactPeople;
            andBOGridAndEditorControlVWG.BusinessObjectCollection = people;
            IButton cancelButton = andBOGridAndEditorControlVWG.ButtonGroupControl["Cancel"];
            IButton newButton = andBOGridAndEditorControlVWG.ButtonGroupControl["New"];

            newButton.PerformClick();
            //---------------Assert Precondition----------------
            Assert.IsTrue(cancelButton.Enabled);
            Assert.AreEqual(1, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            Assert.IsTrue(andBOGridAndEditorControlVWG.IBOEditorControl.Enabled);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, people.Count, "The cancelled item should be removed from the collection");
            Assert.AreEqual(0, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            Assert.IsFalse(andBOGridAndEditorControlVWG.IBOEditorControl.Enabled);
        }

        [Test]
        public void TestCancelButton_ClickRemovesNewObject_TwoItemsInGrid()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            andBOGridAndEditorControlVWG.BusinessObjectCollection = organisationTestBOS;
            IButton cancelButton = andBOGridAndEditorControlVWG.ButtonGroupControl["Cancel"];
            IButton newButton = andBOGridAndEditorControlVWG.ButtonGroupControl["New"];

            newButton.PerformClick();
            // ---------------Assert Precondition----------------
            Assert.IsTrue(cancelButton.Enabled);
            Assert.AreEqual(5, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            Assert.IsTrue(andBOGridAndEditorControlVWG.IBOEditorControl.Enabled);
            //   ---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //  ---------------Test Result -----------------------
            Assert.AreEqual(4, andBOGridAndEditorControlVWG.GridControl.Grid.Rows.Count);
            Assert.IsTrue(andBOGridAndEditorControlVWG.IBOEditorControl.Enabled);
            Assert.IsNotNull(andBOGridAndEditorControlVWG.GridControl.SelectedBusinessObject);
        }

        [Test]
        public void Test_ObjectSavesWhenNewButtonClicked()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            BOEditorControlStubVWG boControl =
                (BOEditorControlStubVWG)andBOGridAndEditorControlVWG.IBOEditorControl;
            andBOGridAndEditorControlVWG.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            IButton newButton = andBOGridAndEditorControlVWG.ButtonGroupControl["New"];
            newButton.PerformClick();
            OrganisationTestBO currentBO =
                (OrganisationTestBO)andBOGridAndEditorControlVWG.IBOEditorControl.BusinessObject;

            //---------------Assert Precondition----------------
            Assert.IsTrue(currentBO.Status.IsNew);
            Assert.IsTrue(currentBO.Status.IsValid());
            //  ---------------Execute Test ----------------------
            newButton.PerformClick();
            // ---------------Test Result -----------------------
            Assert.AreNotSame(currentBO, andBOGridAndEditorControlVWG.IBOEditorControl.BusinessObject);
            Assert.IsFalse(currentBO.Status.IsDirty);
            Assert.IsFalse(currentBO.Status.IsNew);
            Assert.IsFalse(currentBO.Status.IsDeleted);
            Assert.IsFalse(boControl.DisplayErrorsCalled);
        }

        [Test]
        public void Test_ObjectSavesWhenGridRowChanged()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            BOEditorControlStubVWG boControl =
                (BOEditorControlStubVWG)andBOGridAndEditorControlVWG.IBOEditorControl;
            andBOGridAndEditorControlVWG.BusinessObjectCollection = organisationTestBOS;
            OrganisationTestBO firstBO = organisationTestBOS[0];
            OrganisationTestBO secondBO = organisationTestBOS[1];
            //---------------Assert Precondition----------------
            Assert.IsFalse(firstBO.Status.IsNew);
            Assert.IsFalse(secondBO.Status.IsDirty);
            Assert.IsFalse(secondBO.Status.IsNew);
            Assert.AreEqual(0, andBOGridAndEditorControlVWG.GridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(firstBO, andBOGridAndEditorControlVWG.GridControl.Grid.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            andBOGridAndEditorControlVWG.GridControl.Grid.SelectedBusinessObject = secondBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, andBOGridAndEditorControlVWG.GridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(secondBO, andBOGridAndEditorControlVWG.GridControl.Grid.SelectedBusinessObject);
            Assert.IsFalse(firstBO.Status.IsDirty);
            Assert.IsFalse(boControl.DisplayErrorsCalled);
        }

        [Test]
        public void Test_ObjectSavesWhenSaveButtonClicked()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlVWG<OrganisationTestBO> andBOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
            BOEditorControlStubVWG boControl =
                (BOEditorControlStubVWG)andBOGridAndEditorControlVWG.IBOEditorControl;
            andBOGridAndEditorControlVWG.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            IButton saveButton = andBOGridAndEditorControlVWG.ButtonGroupControl["Save"];
            andBOGridAndEditorControlVWG.ButtonGroupControl["New"].PerformClick();
            OrganisationTestBO currentBO =
                (OrganisationTestBO)andBOGridAndEditorControlVWG.IBOEditorControl.BusinessObject;

            //---------------Assert Precondition----------------
            Assert.IsNotNull(currentBO);
            Assert.IsTrue(currentBO.Status.IsNew);
            Assert.IsTrue(currentBO.Status.IsValid());
            //  ---------------Execute Test ----------------------
            saveButton.PerformClick();
            // ---------------Test Result -----------------------
            Assert.AreSame(currentBO, andBOGridAndEditorControlVWG.IBOEditorControl.BusinessObject);
            Assert.IsFalse(currentBO.Status.IsDirty);
            Assert.IsFalse(currentBO.Status.IsNew);
            Assert.IsFalse(currentBO.Status.IsDeleted);
            Assert.IsFalse(boControl.DisplayErrorsCalled);
        }

        //        [Test]
        //        public void Test_CannotChangeGridRowIfCurrentObjectInvalid()
        //        {
        //            ---------------Set up test pack-------------------
        //            BusinessObjectCollection<BusinessObjectInfo> businessObjectInfos = TestUtils.CreateSavedBusinessObjectInfosCollection();
        //            BOGridAndEditorControlVWG<BusinessObjectInfo> BOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
        //           TestComboBox.BOEditorControlStubVWG boControl = (BOEditorControlStubVWG)BOGridAndEditorControlVWG.BusinessObjectControl;
        //            BOGridAndEditorControlVWG.SetBusinessObjectCollection(businessObjectInfos);
        //            BusinessObjectInfo firstBO = businessObjectInfos[0];
        //            firstBO.BusinessObjectName = null;
        //            BusinessObjectInfo secondBO = businessObjectInfos[1];
        //            ---------------Assert Precondition----------------
        //            Assert.IsTrue(firstBO.Status.IsDirty);
        //            Assert.IsFalse(firstBO.Status.IsNew);
        //            Assert.IsFalse(firstBO.IsValid());
        //            Assert.AreEqual(0, BOGridAndEditorControlVWG.GridControl.Grid.SelectedRows[0].Index);
        //            Assert.AreSame(firstBO, BOGridAndEditorControlVWG.GridControl.Grid.SelectedBusinessObject);
        //            Assert.IsFalse(boControl.DisplayErrorsCalled);
        //            ---------------Execute Test ----------------------
        //            BOGridAndEditorControlVWG.GridControl.Grid.SelectedBusinessObject = secondBO;
        //            ---------------Test Result -----------------------
        //            Assert.AreEqual(0, BOGridAndEditorControlVWG.GridControl.Grid.SelectedRows[0].Index);
        //            Assert.AreSame(firstBO, BOGridAndEditorControlVWG.GridControl.Grid.SelectedBusinessObject);
        //            Assert.IsTrue(firstBO.Status.IsDirty);
        //            Assert.IsTrue(boControl.DisplayErrorsCalled);
        //        }

        //        [Test]
        //        public void Test_DisplayErrorsNotCalledWhenNewButtonClicked()
        //        {
        //            ---------------Set up test pack-------------------
        //            BOGridAndEditorControlVWG<BusinessObjectInfo> BOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
        //           TestComboBox.BOEditorControlStubVWG boControl = (BOEditorControlStubVWG)BOGridAndEditorControlVWG.BusinessObjectControl;
        //            BOGridAndEditorControlVWG.SetBusinessObjectCollection(new BusinessObjectCollection<BusinessObjectInfo>());
        //            IButton newButton = BOGridAndEditorControlVWG.ButtonGroupControl["New"];
        //            ---------------Assert Precondition----------------
        //            Assert.IsFalse(boControl.DisplayErrorsCalled);
        //            ---------------Execute Test ----------------------
        //            newButton.PerformClick();
        //            ---------------Test Result -----------------------
        //            Assert.IsFalse(boControl.DisplayErrorsCalled);
        //        }

        //        [Test]
        //        public void Test_ClearErrorsWhenNewObjectAdded()
        //        {
        //            ---------------Set up test pack-------------------
        //            BOGridAndEditorControlVWG<BusinessObjectInfo> BOGridAndEditorControlVWG = CreateGridAndBOEditorControlVWG();
        //           TestComboBox.BOEditorControlStubVWG boControl = (BOEditorControlStubVWG)BOGridAndEditorControlVWG.BusinessObjectControl;
        //            BOGridAndEditorControlVWG.SetBusinessObjectCollection(new BusinessObjectCollection<BusinessObjectInfo>());
        //            IButton newButton = BOGridAndEditorControlVWG.ButtonGroupControl["New"];
        //            ---------------Assert Precondition----------------
        //            Assert.IsFalse(boControl.ClearErrorsCalled);
        //            ---------------Execute Test ----------------------
        //            newButton.PerformClick();
        //            ---------------Test Result -----------------------
        //            Assert.IsTrue(boControl.ClearErrorsCalled);
        //        }
    }


    internal class BOEditorControlStubVWG : UserControlVWG, IBOEditorControl
    {
        public bool DisplayErrorsCalled { get; private set; }

        public bool ClearErrorsCalled { get; private set; }

        public IBusinessObject BusinessObject { get; set; }

        public void DisplayErrors()
        {
            DisplayErrorsCalled = true;
        }

        public void ClearErrors()
        {
            ClearErrorsCalled = true;
        }

        #region Implementation of IBOEditorControl

        /// <summary>
        /// Applies any changes that have occured in any of the Controls on this control's to their related
        /// Properties on the Business Object.
        /// </summary>
        public void ApplyChangesToBusinessObject()
        {

        }

        /// <summary>
        /// Does the business object controlled by this control or any of its Aggregate or Composite children have and Errors.
        /// </summary>
        public bool HasErrors
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Does the Business Object controlled by this control or any of its Aggregate or Composite children have and warnings.
        /// </summary>
        public bool HasWarning
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        ///  Returns a list of all warnings for the business object controlled by this control or any of its children.
        /// </summary>
        public ErrorList Errors
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Does the business object being managed by this control have any edits that have not been persisted.
        /// </summary>
        /// <returns></returns>
        public new bool IsDirty
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Returns a list of all warnings for the business object controlled by this control or any of its children.
        /// </summary>
        /// <returns></returns>
        public ErrorList Warnings
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        #region Implementation of IBusinessObjectPanel

        /// <summary>
        /// Gets and sets the PanelInfo object created by the control
        /// </summary>
        public IPanelInfo PanelInfo
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}