using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Faces.Base.Tests
{
    /// <summary>
    /// Summary description for Sample.
    /// </summary>
    public class Sample : BusinessObject
    {
        private static Dictionary<string, string> itsLookupCollection;
        private static Dictionary<string, string> itsBOLookupCollection;

        public Sample()
        {
        }

        public Sample(ClassDef classDef)
            : this()
        {
            _classDef = classDef;
        }

        public static IClassDef GetClassDef()
        {

            return ClassDef.ClassDefs[typeof(Sample)];
        }

        protected override IClassDef ConstructClassDef()
        {
            _classDef = (ClassDef)GetClassDef();
            return _classDef;
        }

        public static IClassDef CreateClassDefWithTwoPropsOneCompulsory()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""Sample"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""SampleID"" type=""Guid"" />
					<property  name=""SampleText"" />
					<property  name=""SampleText2"" compulsory=""true""/>
					<primaryKey>
						<prop name=""SampleID"" />
					</primaryKey>
					<ui>
						<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field label=""CompulsorySampleText:"" property=""SampleText2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""SampleTextNotCompulsory:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef CreateClassDefWithTwoPropsOneNotEditable()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""Sample"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""SampleID"" type=""Guid"" />
					<property  name=""SampleText"" />
					<property  name=""SampleText2"" />
					<primaryKey>
						<prop name=""SampleID"" />
					</primaryKey>
					<ui>
						<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field label=""EditableFieldSampleText:"" property=""SampleText2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""SampleTextNotEditableField:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" editable=""false"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef CreateClassDefWithAGuidProp()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""Sample"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""SampleID"" type=""Guid"" />
					<property  name=""GuidProp"" type=""Guid"" />
					<primaryKey>
						<prop name=""SampleID"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef CreateClassDefWithTwoPropsOneWithToolTipText()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""Sample"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""SampleID"" type=""Guid"" />
					<property  name=""SampleText"" description=""Test tooltip text""/>
					<property  name=""SampleText2"" />
					<primaryKey>
						<prop name=""SampleID"" />
					</primaryKey>
					<ui>
						<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field property=""SampleText2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef CreateClassDefWithTwoPropsOneInteger()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""Sample"" assembly=""Habanero.Faces.Base.Tests"">
					<property  name=""SampleID"" type=""Guid"" />
					<property  name=""SampleText"" />
					<property  name=""SampleInt"" type=""Int32"" />
					<primaryKey>
						<prop name=""SampleID"" />
					</primaryKey>
					<ui>
                        <form>
							<tab name=""Tab1"">
								<columnLayout width=""200"">
                                    <field label=""SampleText:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                    <field label=""SampleInt:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" editable=""false"" />
                                </columnLayout>
							</tab>
						</form>
					</ui>
					<ui name=""TwoColumns"">
                        <form>
							<columnLayout width=""200"">
                                <field label=""SampleText1:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                <field label=""SampleInt1:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" editable=""false"" />
                            </columnLayout>
							<columnLayout width=""200"">
                                <field label=""SampleText2:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                <field label=""SampleInt2:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" editable=""false"" />
                            </columnLayout>
						</form>
					</ui>
					<ui name=""ThreeColumns"">
                        <form>
							<columnLayout width=""200"">
                                <field label=""SampleText1:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                <field label=""SampleInt1:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" editable=""false"" />
                            </columnLayout>
							<columnLayout width=""200"">
                                <field label=""SampleText2:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                <field label=""SampleInt2:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" editable=""false"" />
                            </columnLayout>
							<columnLayout width=""200"">
                                <field label=""SampleText3:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                <field label=""SampleInt3:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" editable=""false"" />
                            </columnLayout>
						</form>
					</ui>
					<ui name=""TwoTabs"">
                        <form>
							<tab name=""Tab1"">
								<columnLayout width=""200"">
                                    <field label=""SampleText:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                </columnLayout>
							</tab>
						<tab name=""Tab1"">
								<columnLayout width=""200"">
                                    <field label=""SampleInt:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" editable=""false"" />
                                </columnLayout>
							</tab>
						</form>
					</ui>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static ClassDef CreateDefaultClassDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            lPropDefCol.Add(
                new PropDef("SampleText", typeof(String), PropReadWriteRule.ReadWrite, "SampleText", null));
            lPropDefCol.Add(
                new PropDef("SampleText2", typeof(String), PropReadWriteRule.ReadWrite, "SampleText2", null));
            lPropDefCol.Add(
                new PropDef("SampleTextPrivate", typeof(String), PropReadWriteRule.ReadWrite, "SampleTextPrivate", null,
                            false, false, int.MaxValue, null, null));
            lPropDefCol.Add(
                new PropDef("SampleTextDescribed", typeof(String), PropReadWriteRule.ReadWrite, "SampleTextDescribed", null,
                            false, false, int.MaxValue, "SampleTextDescribed", "This is a sample text property that has a description."));
            lPropDefCol.Add(
                new PropDef("SampleDate", typeof(DateTime), PropReadWriteRule.ReadWrite, "SampleDate", null));
            lPropDefCol.Add(
                new PropDef("SampleDateNullable", typeof(DateTime), PropReadWriteRule.ReadWrite, "SampleDate", null));
            lPropDefCol.Add(
                new PropDef("SampleBoolean", typeof(Boolean), PropReadWriteRule.ReadWrite, "SampleBoolean", null));
            lPropDefCol.Add(
                new PropDef("SampleLookupID", typeof(Guid), PropReadWriteRule.ReadWrite, "SampleLookupID", null));
            lPropDefCol.Add(
                new PropDef("SampleInt", typeof(int), PropReadWriteRule.ReadWrite, "SampleInt", 0));
            lPropDefCol.Add(
                new PropDef("SampleMoney", typeof(Decimal), PropReadWriteRule.ReadWrite, "SampleInt", new Decimal(0)));
            PropDef propDef = new PropDef("SampleLookup2ID", typeof(Guid), PropReadWriteRule.ReadWrite, "SampleLookup2ID", null);
            itsLookupCollection = new Dictionary<string, string>();
            itsLookupCollection.Add("Test1", new Guid("{6E8B3DDB-1B13-4566-868D-57478C1F4BEE}").ToString());
            itsLookupCollection.Add("Test2", new Guid("{7209B956-96A0-4720-8E49-DE154FA0E096}").ToString());
            itsLookupCollection.Add("Test3", new Guid("{F45DE850-C693-44d8-AC39-8CEE5435B21A}").ToString());
            propDef.LookupList = new SimpleLookupList(itsLookupCollection);
            lPropDefCol.Add(propDef);
            lPropDefCol.Add(new PropDef("SampleLookup3ID", typeof(String), PropReadWriteRule.ReadWrite, "SampleLookup3ID",
                                        null));
            PropDef def = new PropDef("SampleID", typeof(Guid), PropReadWriteRule.WriteOnce, null);
            lPropDefCol.Add(def);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsGuidObjectID = true;
            primaryKey.Add(lPropDefCol["SampleID"]);
            KeyDefCol keysCol = new KeyDefCol();
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            return new ClassDef(typeof(Sample), primaryKey, lPropDefCol, keysCol, relDefCol);
        }

        public static Dictionary<string, string> LookupCollection
        {
            get { return itsLookupCollection; }
        }

        public static Dictionary<string, string> BOLookupCollection
        {
            get
            {
                if (itsBOLookupCollection == null)
                {
                    Sample sample1 = new Sample();
                    sample1.Save();
                    Sample sample2 = new Sample();
                    sample2.Save();
                    Sample sample3 = new Sample();
                    sample3.Save();
                    itsBOLookupCollection = new Dictionary<string, string>
                                                {
                                                    {"Test3", sample3.ID.GetAsValue().ToString()},
                                                    {"Test2", sample2.ID.GetAsValue().ToString()},
                                                    {"Test1", sample1.ID.GetAsValue().ToString()}
                                                };
                }
                return itsBOLookupCollection;
            }
            set { itsBOLookupCollection = value; }
        }

        public DateTime SampleDate
        {
            get { return (DateTime)this.GetPropertyValue("SampleDate"); }
            set { this.SetPropertyValue("SampleDate", value); }
        }

        public DateTime? SampleDateNullable
        {
            get { return (DateTime?)this.GetPropertyValue("SampleDateNullable"); }
            set { this.SetPropertyValue("SampleDateNullable", value); }
        }

        public Guid? SampleLookupID
        {
            get { return (Guid?)this.GetPropertyValue("SampleLookupID"); }
            set { this.SetPropertyValue("SampleLookupID", value); }
        }

        public string SampleText
        {
            get { return (string)this.GetPropertyValue("SampleText"); }
            set { this.SetPropertyValue("SampleText", value); }
        }

        public bool SampleBoolean
        {
            get { return (bool)this.GetPropertyValue("SampleBoolean"); }
            set { this.SetPropertyValue("SampleBoolean", value); }
        }

        public int SampleInt
        {
            set { this.SetPropertyValue("SampleInt", value); }
            get { return (int)this.GetPropertyValue("SampleInt"); }
        }

        public decimal SampleMoney
        {
            get { return (decimal)this.GetPropertyValue("SampleMoney"); }
            set { this.SetPropertyValue("SampleMoney", value); }
        }

        #region Old methods for the purpose of not breaking other tests

        #endregion

    }
}