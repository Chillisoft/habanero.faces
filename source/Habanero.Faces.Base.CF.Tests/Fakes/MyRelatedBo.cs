using System;
using System.Linq;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Faces.Base.CF.Tests
{
    public class MyInheritedType : MyRelatedBo
    {
        public static IClassDef LoadInheritedTypeClassDef()
        {
            MyRelatedBo.LoadSuperClassDef();
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            var classDef = itsLoader.LoadClass(@"
				  <class name=""MyInheritedType"" assembly=""Habanero.Faces.Base.CF.Tests"" table=""MyRelatedBo"">
					<superClass class=""MyRelatedBo"" assembly=""Habanero.Faces.Base.CF.Tests"" orMapping=""SingleTableInheritance"" discriminator=""Discriminator"" />
				  </class>");
            ClassDef.ClassDefs.Add(classDef);
            return classDef;
        }
    }
    public class MyRelatedBo : BusinessObject
    {
        private static IClassDef itsClassDef;

        public MyRelatedBo()
        {
        }

        public MyRelatedBo(ClassDef def)
            : base(def)
        {
        }

        protected override IClassDef ConstructClassDef()
        {
            return itsClassDef;
        }

        public MyBO MyRelationship
        {
            get
            {
                return this.Relationships.GetRelatedObject<MyBO>("MyRelationship");
            }
        }

        public Guid? MyBoID
        {
            get { return (Guid?)this.GetPropertyValue("MyBoID"); }
        }

        public static IClassDef LoadClassDef()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyRelatedBo"" assembly=""Habanero.Faces.Base.CF.Tests"" table=""MyRelatedBo"">
					<property  name=""MyRelatedBoID"" type=""Guid""/>
					<property  name=""MyRelatedTestProp"" />
					<property  name=""MyBoID"" type=""Guid""/>
					<primaryKey>
						<prop name=""MyRelatedBoID"" />
					</primaryKey>
					<relationship name=""MyRelationship"" type=""single"" relatedClass=""MyBO"" relatedAssembly=""Habanero.Faces.Base.CF.Tests"">
						<relatedProperty property=""MyBoID"" relatedProperty=""MyBoID"" />
					</relationship>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadSuperClassDef()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyRelatedBo"" assembly=""Habanero.Faces.Base.CF.Tests"" table=""MyRelatedBo"">
					<property  name=""MyRelatedBoID"" type=""Guid""/>
					<property  name=""MyRelatedTestProp"" />
					<property  name=""MyBoID"" type=""Guid""/>
					<property  name=""Discriminator""/>
					<primaryKey>
						<prop name=""MyRelatedBoID"" />
					</primaryKey>
					<relationship name=""MyRelationship"" type=""single"" relatedClass=""MyBO"" relatedAssembly=""Habanero.Faces.Base.CF.Tests"">
						<relatedProperty property=""MyBoID"" relatedProperty=""MyBoID"" />
					</relationship>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        protected static XmlClassLoader CreateXmlClassLoader()
        {
            return new XmlClassLoader(new DtdLoader(), new DefClassFactory());
        }

        public static IClassDef LoadClassDefWithDifferentTableAndFieldNames()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyRelatedBo"" assembly=""Habanero.Faces.Base.CF.Tests"" table=""My_Related_Bo"">
					<property  name=""MyRelatedBoID"" type=""Guid"" databaseField=""My_Related_Bo_ID""/>
					<property  name=""MyRelatedTestProp"" databaseField=""My_Related_Test_Prop"" />
					<property  name=""MyRelatedTestProp2"" databaseField=""My_Related_Test_Prop2"" />
					<property  name=""MyBoID"" type=""Guid"" databaseField=""My_Bo_ID"" />
					<primaryKey>
						<prop name=""MyRelatedBoID"" />
					</primaryKey>
					<relationship name=""MyRelationship"" type=""single"" relatedClass=""MyBO"" relatedAssembly=""Habanero.Faces.Base.CF.Tests"">
						<relatedProperty property=""MyBoID"" relatedProperty=""MyBoID"" />
					</relationship>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithRelationshipBackToMyBo()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyRelatedBo"" assembly=""Habanero.Faces.Base.CF.Tests"" table=""MyRelatedBo"">
					<property  name=""MyRelatedBoID"" type=""Guid""/>
					<property  name=""MyRelatedTestProp"" />
					<property  name=""MyBoID"" type=""Guid""/>
					<primaryKey>
						<prop name=""MyRelatedBoID"" />
					</primaryKey>
					<relationship name=""MyRelationshipToMyBo"" type=""single"" relatedClass=""MyBO"" relatedAssembly=""Habanero.Faces.Base.CF.Tests"" >
						<relatedProperty property=""MyBoID"" relatedProperty=""MyBoID"" />
					</relationship>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithSingleRelationshipBackToMyBo()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyRelatedBo"" assembly=""Habanero.Faces.Base.CF.Tests"" table=""MyRelatedBo"">
					<property  name=""MyRelatedBoID"" type=""Guid""/>
					<property  name=""MyRelatedTestProp"" />
					<property  name=""MyBoID"" type=""Guid""/>
					<primaryKey>
						<prop name=""MyRelatedBoID"" />
					</primaryKey>
					<relationship name=""MyRelationshipToMyBo"" type=""single"" relatedClass=""MyBO"" relatedAssembly=""Habanero.Faces.Base.CF.Tests"" reverseRelationship=""MyRelationship"">
						<relatedProperty property=""MyBoID"" relatedProperty=""MyBoID"" />
					</relationship>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithMultipleRelationshipBackToMyBo()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyRelatedBo"" assembly=""Habanero.Faces.Base.CF.Tests"" table=""MyRelatedBo"">
					<property  name=""MyRelatedBoID"" type=""Guid""/>
					<property  name=""MyRelatedTestProp"" />
					<property  name=""MyBoID"" type=""Guid""/>
					<primaryKey>
						<prop name=""MyRelatedBoID"" />
					</primaryKey>
					<relationship name=""MyRelationshipToMyBo"" type=""multiple"" relatedClass=""MyBO"" relatedAssembly=""Habanero.Faces.Base.CF.Tests"" reverseRelationship=""MyRelationship"">
						<relatedProperty property=""MyBoID"" relatedProperty=""MyBoID"" />
					</relationship>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithRelationshipBackToMyBoAndGridDef()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyRelatedBo"" assembly=""Habanero.Faces.Base.CF.Tests"" table=""MyRelatedBo"">
					<property  name=""MyRelatedBoID"" type=""Guid""/>
					<property  name=""MyRelatedTestProp"" />
					<property  name=""MyBoID"" type=""Guid""/>
					<primaryKey>
						<prop name=""MyRelatedBoID"" />
					</primaryKey>
					<relationship name=""MyRelationshipToMyBo"" type=""single"" relatedClass=""MyBO"" relatedAssembly=""Habanero.Faces.Base.CF.Tests"">
						<relatedProperty property=""MyBoID"" relatedProperty=""MyBoID"" />
					</relationship>
					<ui>
						<grid>
							<column heading=""My Related Test Prop"" property=""MyRelatedTestProp"" type=""DataGridViewTextBoxColumn"" />
						</grid>
					</ui>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithSingleTableInheritance()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyRelatedBo"" assembly=""Habanero.Faces.Base.CF.Tests"">
					<superClass class=""MyBO"" assembly=""Habanero.Faces.Base.CF.Tests"" 
						orMapping=""SingleTableInheritance"" discriminator=""TestProp"" />
					<property  name=""MyRelatedBoID"" type=""Guid""/>
					<property  name=""MyRelatedTestProp"" />
					<property  name=""MyBoID"" type=""Guid""/>
					<primaryKey>
						<prop name=""MyRelatedBoID"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDef_WithUIDefVirtualProp()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyRelatedBo"" assembly=""Habanero.Faces.Base.CF.Tests"" table=""MyRelatedBo"">
					<property  name=""MyRelatedBoID"" type=""Guid""/>
					<property  name=""MyRelatedTestProp"" />
					<property  name=""MyBoID"" type=""Guid""/>
					<primaryKey>
						<prop name=""MyRelatedBoID"" />
					</primaryKey>
					<relationship name=""MyRelationship"" type=""single"" relatedClass=""MyBO"" relatedAssembly=""Habanero.Faces.Base.CF.Tests"">
						<relatedProperty property=""MyBoID"" relatedProperty=""MyBoID"" />
					</relationship>
					<ui>
						<grid>
							<column heading=""My Related Test Prop"" property=""MyRelatedTestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""My Related Virtual Prop"" property=""MyRelationship.-MyName-"" type=""DataGridViewTextBoxColumn"" />
						</grid>
					</ui>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
    }
}
