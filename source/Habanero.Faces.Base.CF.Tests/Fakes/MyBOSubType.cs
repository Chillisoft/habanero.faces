using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Faces.Base.CF.Tests
{
    public class MyBOSubType : MyBO
    {


        public static IClassDef LoadInheritedTypeClassDef()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            var classDef =
                itsLoader.LoadClass(
                    @"<class name=""MyBOSubType"" assembly=""Habanero.Faces.Base.CF.Tests"">
						<superClass class=""MyBO"" assembly=""Habanero.Faces.Base.CF.Tests"" orMapping=""SingleTableInheritance"" discriminator=""MyBOType"" />
					  </class>");
            ClassDef.ClassDefs.Add(classDef);
            return classDef;
        }
    }
}