﻿using System;
using System.Collections.Generic;
using System.Xml;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Util;

namespace Habanero.Faces.Base.Tests
{
    /// <summary>
    /// Summary description for MyBO.
    /// </summary>
    [Serializable]
    public class MyBO : BusinessObject
    {
        private readonly List<IBusinessObjectRule> _myRuleList;
        private string _toStringValue = _stdToStringVal;
        private static string _stdToStringVal = "STDVALUE";


        public MyBO(IClassDef def)
            : base(def)
        {
            _myRuleList = new List<IBusinessObjectRule>();
        }

        public MyBO()
        {
            _myRuleList = new List<IBusinessObjectRule>();
        }

        protected MyBO(ConstructForFakes constructForFakes)
            : base(constructForFakes)
        {
        }

        protected override IClassDef ConstructClassDef()
        {
            return _classDef;
        }

        // ReSharper disable UnusedMember.Global

        public Double MyVirtualDoubleProp
        {
            get { return 11.00d; }
        }
        public Double? MyNullableVirtualDoubleProp
        {
            get { return 11.00d; }
        }
        public string MyName
        {
            get { return "MyNameIsMyBo"; }
        }
        // ReSharper restore UnusedMember.Global
        public string TestProp
        {
            get
            {
                return Convert.ToString(this.GetPropertyValue("TestProp"));
            }
            set
            {
                this.SetPropertyValue("TestProp", value);
            }
        }
        public Guid? MyBoID
        {
            get
            {
                return (Guid?)this.GetPropertyValue("MyBoID");
            }
            set { this.SetPropertyValue("MyBoID", value); }
        }

        public BusinessObjectCollection<MyRelatedBo> MyMultipleRelationship
        {
            get
            {
                return this.Relationships.GetRelatedCollection<MyRelatedBo>("MyMultipleRelationship");
            }
        }

        public bool TestBoolean
        {
            get { return (bool)this.GetPropertyValue("TestBoolean"); }
            set { this.SetPropertyValue("TestBoolean", value); }
        }
        public string TestProp2
        {
            get
            {
                return Convert.ToString(this.GetPropertyValue("TestProp2"));
            }
            set
            {
                this.SetPropertyValue("TestProp2", value);
            }
        }

        //TODO brett 30 Mar 2011: CF
        public decimal SampleMoney
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public static IClassDef LoadClassDefs_OneProp()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""  type=""Guid"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        protected static XmlClassLoader CreateXmlClassLoader()
        {
            return new XmlClassLoader(new DtdLoader(), new DefClassFactory());
        }

        public static IClassDef LoadClassDefsNoUIDef()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""  type=""Guid"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefsHasModuleName()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"" moduleName=""MyBOModule"">
					<property  name=""MyBoID""  type=""Guid"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadClassDefs_Integer_PrimaryKey()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""  type=""Int32"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey  isObjectID=""false"">
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadDefaultClassDef()
        {

            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
					<ui name=""Alternate"">
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui> 
					<ui name=""AlternateNoGrid"">
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>   
					<ui name=""AlternateVirtualProp"">
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""-MyTestProp-"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				 
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDef_NonGuidID()
        {

            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey isObjectID=""false"">
						<prop name=""TestProp""  />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
					<ui name=""Alternate"">
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui> 
					<ui name=""AlternateNoGrid"">
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>   
					<ui name=""AlternateVirtualProp"">
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""-MyTestProp-"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				 
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadDefaultClassDefWithFilterDef()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<filter>
								<filterProperty name=""TestProp"" label=""Test Prop:"" />
								<filterProperty name=""TestProp2"" label=""Test Prop2:"" />
							</filter>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;

        }


        public static IClassDef LoadDefaultClassDef_CompulsoryField_TestProp()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid"" />
					<property  name=""TestProp"" compulsory=""true"" />
					<property  name=""TestProp2""  />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadDefaultClassDefWithDifferentTableAndFieldNames()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"" table=""my_bo"" >
					<property  name=""MyBoID""  type=""Guid"" databaseField=""my_bo_id"" />
					<property  name=""TestProp"" databaseField=""test_prop"" />
					<property  name=""TestProp2"" databaseField=""test_prop2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadDefaultClassDefVWG()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" assembly=""Gizmox.WebGUI.Forms"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" assembly=""Gizmox.WebGUI.Forms"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
					<ui name=""Alternate"">
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui> 
					<ui name=""AlternateNoGrid"">
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>    
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefVWG_NonGuidID()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey isObjectID=""false"">
						<prop name=""TestProp""  />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" assembly=""Gizmox.WebGUI.Forms"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" assembly=""Gizmox.WebGUI.Forms"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
					<ui name=""Alternate"">
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui> 
					<ui name=""AlternateNoGrid"">
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>    
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithNoLookup()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewTextBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
					<ui name=""Success1"">
						<grid sortColumn=""TestProp"">
							<column property=""TestProp"" />
						</grid>
					</ui>
					<ui name=""Success2"">
						<grid sortColumn=""TestProp asc"">
							<column property=""TestProp"" />
						</grid>
					</ui>
					<ui name=""Success3"">
						<grid sortColumn=""TestProp desc"">
							<column property=""TestProp"" />
						</grid>
					</ui>
					<ui name=""Success4"">
						<grid sortColumn=""TestProp des"">
							<column property=""TestProp"" />
						</grid>
					</ui>
					<ui name=""Error1"">
						<grid sortColumn=""TestProps"">
							<column property=""TestProp"" />
						</grid>
					</ui>
					<ui name=""Error2"">
						<grid sortColumn=""TestProps desc"">
							<column property=""TestProp"" />
						</grid>
					</ui>
					<ui name=""Error3"">
						<grid sortColumn=""TestProps descs"">
							<column property=""TestProp"" />
						</grid>
					</ui>
				</class>

			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithBoolean()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""  type=""Guid"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<property  name=""TestBoolean"" type=""Boolean"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Boolean"" property=""TestBoolean"" type=""DataGridViewCheckBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Boolean"" property=""TestBoolean"" type=""CheckBox"" mapperType=""CheckBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
				
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadClassDefWith_Grid_1TextboxColumn()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""  type=""Guid"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
						</grid>
					</ui>
				</class>
				
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWith_Grid_2Columns_1stHasZeroWidth()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""  type=""Guid"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" width=""0""/>
							<column heading=""Test Prop 2"" property=""TestProp2""/>
						</grid>
					</ui>
				</class>
				
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWith_Grid_1DateTimeColumn()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""   type=""Guid""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewDateTimeColumn"" />
						</grid>
					</ui>
				</class>
				
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadClassDefWith_Grid_1CheckBoxColumn()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""  type=""Guid"" />
					<property  name=""TestProp"" type=""Boolean""/>
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewCheckBoxColumn"" />
						</grid>
					</ui>
				</class>
				
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWith_Grid_1ComboBoxColumn()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""   type=""Guid""/>
					<property  name=""RelatedID"" type=""Guid""/>
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<relationship name=""MyRelationship"" type=""single"" relatedClass=""MyRelatedBo"" relatedAssembly=""Habanero.Faces.Base.Tests"">
						<relatedProperty property=""RelatedID"" relatedProperty=""MyRelatedBoID"" />
					</relationship>
					<ui>
						<grid>
							<column heading=""Related"" property=""RelatedID"" type=""DataGridViewComboBoxColumn"" />
						</grid>
					</ui>
				</class>
				
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadClassDefWithDateTime()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""  type=""Guid""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<property  name=""TestDateTime"" type=""DateTime"" />
					<property  name=""TestDateTime2"" type=""DateTime"" >
						   <rule name=""TestProp"">
							<add key=""min"" value=""2005/06/08"" />
							<add key=""max"" value=""2005/06/15"" />
						</rule>
					</property>
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test DateTime"" property=""TestDateTime"" type=""DataGridViewDateTimeColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Boolean"" property=""TestDateTime"" type=""DateTimePicker"" mapperType=""DateTimePickerMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
				
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithDateTimeParameterFormat()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""  type=""Guid""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<property  name=""TestDateTime"" type=""DateTime"" />
					<property  name=""TestDateTimeNoFormat"" type=""DateTime"" />					
					<property  name=""TestDateTimeFormat"" type=""DateTime"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test DateTime"" property=""TestDateTime"" />
							<column heading=""Test DateTime"" property=""TestDateTimeNoFormat"" />
							<column heading=""Test DateTime"" property=""TestDateTimeFormat"" >
								<parameter name=""dateFormat"" value=""dd.MMM.yyyy"" />
							</column>
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Boolean"" property=""TestDateTime"" type=""DateTimePicker"" mapperType=""DateTimePickerMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
				
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithCurrencyParameterFormat()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""  type=""Guid""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<property  name=""TestCurrencyNoFormat"" type=""Double"" />					
					<property  name=""TestCurrencyFormat"" type=""Double"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Currency"" property=""TestCurrencyNoFormat"" />
							<column heading=""Test CurrencyFormat"" property=""TestCurrencyFormat"" >
								<parameter name=""currencyFormat"" value=""### ###.##"" />
							</column>
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
				
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadClassDefWithCurrencyParameterFormat_VirtualProp()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""  type=""Guid""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test CurrencyFormat"" property=""-MyVirtualDoubleProp-"" >
								<parameter name=""currencyFormat"" value=""### ###.##"" />
							</column>
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
				
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadClassDefWithLookup()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""  type=""Guid"" compulsory=""true""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" type=""Guid"" >
						<simpleLookupList>
							<item display=""s1"" value=""{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}"" />
							<item display=""s2"" value=""{F428FADC-3740-412c-91A7-ECEB4D414414}"" />
						</simpleLookupList>
					</property>
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
					<ui name=""duplicateColumns"">
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
					</ui>
				</class>


			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadClassDefWithUIAllDataTypes()
        {

            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""  type=""Guid""/>
					<property  name=""TestProp"" type=""Int32"" />
					<property  name=""TestProp1"" type=""DateTime"" />
					<property  name=""TestProp2"" type=""Decimal"" />
					<property  name=""TestProp3"" type=""Double"" />
					<property  name=""TestProp4""  type=""Single"" />
					<property  name=""TestProp5""  type=""TimeSpan"" />
					<property  name=""TestProp6"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column  property=""TestProp1"" type=""DataGridViewTextBoxColumn"" />
							<column  property=""TestProp2"" type=""DataGridViewTextBoxColumn"" />
							<column property=""TestProp3"" type=""DataGridViewTextBoxColumn"" />
							<column  property=""TestProp4"" type=""DataGridViewTextBoxColumn"" />
							<column  property=""TestProp5"" type=""DataGridViewTextBoxColumn"" />
							<column property=""TestProp6"" type=""DataGridViewTextBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>


			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithSimpleIntegerLookup()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""  type=""Guid""/>
					<property  name=""TestProp"" />
					<property name=""TestProp2"" type=""Int32"" default=""1"" compulsory=""true"">
					  <simpleLookupList>
						<item display=""Integer"" value=""2"" />
						<item display=""Selection"" value=""3"" />
						<item display=""Text"" value=""1"" />
					  </simpleLookupList>
					</property>
					<property name=""SimpleLookupNotCompulsory"" type=""Int32"" default=""1"">
					  <simpleLookupList>
						<item display=""Integer"" value=""2"" />
						<item display=""Selection"" value=""3"" />
						<item display=""Text"" value=""1"" />
					  </simpleLookupList>
					</property>
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadClassDefWithStringLookup()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""  type=""Guid""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" >
						<simpleLookupList>
							<item display=""Started"" value=""S"" />
							<item display=""Complete"" value=""C"" />
						</simpleLookupList>
					</property>
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>


			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithBOLookup()
        {
            return LoadClassDefWithBOLookup("");
        }
        public static IClassDef LoadClassDefWithBOLookup(string boLookupCriteria)
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID""  type=""Guid""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" type=""Guid"" >
						<businessObjectLookupList class=""ContactPersonTestBO"" assembly=""Habanero.Faces.Base.Tests.BO"" "
                    + (String.IsNullOrEmpty(boLookupCriteria) ? "" : String.Format(@"criteria=""{0}"" ", ConvertToXmlString(boLookupCriteria, XmlNodeType.Attribute)))
                    + @"/>
					</property>
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>


			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        private static string ConvertToXmlString(string input, XmlNodeType xmlNodeType)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<foo/>");
            string result = input;
            switch (xmlNodeType)
            {
                case XmlNodeType.None:
                    break;
                case XmlNodeType.Element:
                    break;
                case XmlNodeType.Attribute:
                    XmlAttribute newAttribute = doc.CreateAttribute("genre");
                    newAttribute.Value = input;
                    if (doc.DocumentElement != null) doc.DocumentElement.Attributes.Append(newAttribute);
                    result = newAttribute.InnerXml;
                    break;
                case XmlNodeType.Text:
                    if (doc.DocumentElement != null)
                    {
                        doc.DocumentElement.InnerText = input;
                        result = doc.DocumentElement.InnerXml;
                    }
                    break;
                case XmlNodeType.CDATA:
                    break;
                case XmlNodeType.EntityReference:
                    break;
                case XmlNodeType.Entity:
                    break;
                case XmlNodeType.ProcessingInstruction:
                    break;
                case XmlNodeType.Comment:
                    break;
                case XmlNodeType.Document:
                    break;
                case XmlNodeType.DocumentType:
                    break;
                case XmlNodeType.DocumentFragment:
                    break;
                case XmlNodeType.Notation:
                    break;
                case XmlNodeType.Whitespace:
                    break;
                case XmlNodeType.SignificantWhitespace:
                    break;
                case XmlNodeType.EndElement:
                    break;
                case XmlNodeType.EndEntity:
                    break;
                case XmlNodeType.XmlDeclaration:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("xmlNodeType");
            }
            return result;
        }
        public static IClassDef LoadClassDefWithTwoUITabs()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
							<tab name=""Tab2"">
								<columnLayout>
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>


			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        // ReSharper disable UnusedMember.Global These are used by Asset Management
        public static IClassDef LoadClassDefWithThreeUITabs()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
							<tab name=""Tab2"">
								<columnLayout>
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
							<tab name=""Tab3"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>


			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadClassDefWithThreeUITabs_UI_Define()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui name=""Define"">
						<form>
							<tab name=""Collapse Tab 1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
							<tab name=""Collapse Tab 2"">
								<columnLayout>
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
							<tab name=""Collapse Tab 3"">
								<columnLayout>
									<field label=""MyBoID"" property=""MyBoID"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>


			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        // ReSharper restore UnusedMember.Global
        public static IClassDef LoadClassDefWithBOStringLookup()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" >
						<businessObjectLookupList class=""ContactPersonTestBO"" assembly=""Habanero.Faces.Base.Tests.BO"" />
					</property>
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>


			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithStringRule()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid""/>
					<property  name=""TestProp"">
						<rule name=""TestProp"">
							<add key=""maxLength"" value=""5"" />
						</rule>
					</property>
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui />                    
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithIntegerRule()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid""/>
					<property  name=""TestProp2"" type=""Int32""/>
					<property  name=""TestProp"" type=""Int32"">
						<rule name=""TestProp"">
							<add key=""min"" value=""2"" />
							<add key=""max"" value=""5"" />
						</rule>
					</property>
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui />                    
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithRelationship()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" type=""Guid"" >
						<simpleLookupList>
							<item display=""s1"" value=""{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}"" />
							<item display=""s2"" value=""{F428FADC-3740-412c-91A7-ECEB4D414414}"" />
						</simpleLookupList>
					</property>
					<property  name=""RelatedID"" type=""Guid"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<relationship name=""MyRelationship"" type=""single"" relatedClass=""MyRelatedBo"" relatedAssembly=""Habanero.Faces.Base.Tests"">
						<relatedProperty property=""RelatedID"" relatedProperty=""MyRelatedBoID"" />
					</relationship>
					<relationship name=""MyMultipleRelationship"" type=""multiple"" relatedClass=""MyRelatedBo"" relatedAssembly=""Habanero.Faces.Base.Tests"">
						<relatedProperty property=""MyBoID"" relatedProperty=""MyBoID"" />
					</relationship>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field property=""MyRelationship"" type=""ComboBox"" mapperType=""AutoLoadingRelationshipComboBoxMapper"" mapperAssembly=""Habanero.Faces.Base"" editable=""true"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>


			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        // use this along with MyRelatedBo.LoadClassDefWithSingleRelationshipBackToMyBo() or LoadClassDefWithMultipleRelationshipBackToMyBo()
        public static IClassDef LoadClassDefWithSingleRelationshipWithReverseRelationship()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid""/>
					<property  name=""TestProp"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<relationship name=""MyRelationship"" type=""single"" relatedClass=""MyRelatedBo"" relatedAssembly=""Habanero.Faces.Base.Tests"" reverseRelationship=""MyRelationshipToMyBo"">
						<relatedProperty property=""RelatedID"" relatedProperty=""MyRelatedBoID"" />
					</relationship>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }


        public static IClassDef LoadClassDefWithRelationshipAndFormGrid()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" type=""Guid"" >
						<simpleLookupList>
							<item display=""s1"" value=""{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}"" />
							<item display=""s2"" value=""{F428FADC-3740-412c-91A7-ECEB4D414414}"" />
						</simpleLookupList>
					</property>
					<property  name=""RelatedID"" type=""Guid"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<relationship name=""MyRelationship"" type=""single"" relatedClass=""MyRelatedBo"" relatedAssembly=""Habanero.Faces.Base.Tests"">
						<relatedProperty property=""RelatedID"" relatedProperty=""MyRelatedBoID"" />
					</relationship>
					<relationship name=""MyMultipleRelationship"" type=""multiple"" relatedClass=""MyRelatedBo"" relatedAssembly=""Habanero.Faces.Base.Tests"">
						<relatedProperty property=""MyBoID"" relatedProperty=""MyBoID"" />
					</relationship>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
							<tab name=""FormGridTab"">
								<formGrid relationship=""MyMultipleRelationship"" reverseRelationship=""MyRelationshipToMyBo"" />
							</tab>
						</form>
					</ui>
				</class>


			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadClassDefWithRelationship_DifferentTableAndFieldNames()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"" table=""my_bo"" >
					<property  name=""MyBoID"" type=""Guid"" databaseField=""my_bo_id"" />
					<property  name=""TestProp"" databaseField=""test_prop"" />
					<property  name=""TestProp2"" databaseField=""test_prop2"" />
					<property  name=""RelatedID"" type=""Guid"" databaseField=""related_id""  />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<relationship name=""MyRelationship"" type=""single"" relatedClass=""MyRelatedBo"" relatedAssembly=""Habanero.Faces.Base.Tests"">
						<relatedProperty property=""RelatedID"" relatedProperty=""MyRelatedBoID"" />
					</relationship>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }





        public static IClassDef LoadClassDefWithShape_SingleTableInheritance_Relationship()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid""/>
					<property  name=""ShapeID"" type=""Guid"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<relationship name=""Shape"" type=""single"" relatedClass=""Shape"" relatedAssembly=""Habanero.Faces.Base.Tests"">
						<relatedProperty property=""ShapeID"" relatedProperty=""ShapeID"" />
					</relationship>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }


        //public static MyBO Create()
        //{
        //    MyBO bo = new MyBO();
        //    return bo;
        //}


        public static IClassDef LoadWebGuiClassDef()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" assembly=""Gizmox.WebGUI.Forms"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewTextBoxColumn"" assembly=""Gizmox.WebGUI.Forms"" />
						</grid>
					</ui>                 
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        ///<summary>
        ///Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override string ToString()
        {
            if (_toStringValue != _stdToStringVal) return _toStringValue;
            if (Props.Contains("TestProp"))
            {
                return this.TestProp + " - " + this.MyBoID;

            }
            if (this.MyBoID == null)
            {
                return this.ClassDef.ClassNameFull;
            }
            return StringUtilities.GuidToUpper(this.MyBoID.Value);
        }
        public void SetToString(string value)
        {
            _toStringValue = value;
        }

        public static IClassDef GetLoadClassDefsUIDefNoFormDef()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid""/>
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewTextBoxColumn"" />
						</grid>
					</ui>                 
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }


        public static IClassDef LoadClassDefWithDecimalRule()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""MyBoID"" type=""Guid""/>
					<property  name=""TestProp2"" type=""Decimal""/>
					<property  name=""TestProp"" type=""Decimal"">
						<rule name=""TestProp"">
							<add key=""min"" value=""2.00"" />
							<add key=""max"" value=""5.02"" />
						</rule>
					</property>
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui />                    
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithAssociationRelationship()
        {
            IClassDef classDef = LoadClassDefWithRelationship();
            classDef.RelationshipDefCol["MyMultipleRelationship"].RelationshipType = RelationshipType.Association;
            return classDef;
        }

        public static IClassDef LoadClassDefWithAggregationRelationship()
        {
            IClassDef classDef = LoadClassDefWithRelationship();
            classDef.RelationshipDefCol["MyMultipleRelationship"].RelationshipType = RelationshipType.Aggregation;
            return classDef;
        }

        public static IClassDef LoadClassDefWithCompositionRelationship()
        {
            IClassDef classDef = LoadClassDefWithRelationship();
            classDef.RelationshipDefCol["MyMultipleRelationship"].RelationshipType = RelationshipType.Composition;
            return classDef;
        }

        public static IClassDef LoadDefaultClassDefWithDefault(string testPropDefault)
        {
            IClassDef classDef = LoadDefaultClassDef();
            classDef.PropDefcol["TestProp"].DefaultValueString = testPropDefault;
            return classDef;
        }



        public void AddBusinessRule(IBusinessObjectRule businessObjectRuleStub)
        {

            _myRuleList.Add(businessObjectRuleStub);
        }

        protected override void LoadBusinessObjectRules(IList<IBusinessObjectRule> boRules)
        {
            base.LoadBusinessObjectRules(boRules);
            if (_myRuleList == null) return;

            foreach (IBusinessObjectRule rule in _myRuleList)
            {
                boRules.Add(rule);
            }
        }
    }
}