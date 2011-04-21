using System;
using System.IO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace TestUtilities
{
/*    public static class ClassDefLoader
    {
        public static void LoadClassDefs()
        {
            ClassDef.ClassDefs.Clear();
            var loadedClassDefs = new XmlClassDefsLoader(GetClassDefsXml(), new DtdLoader()).LoadClassDefs();
            ClassDef.ClassDefs.Add(loadedClassDefs);
        }

        private static string GetClassDefsXml()
        {
            StreamReader classDefStream = new StreamReader(
                typeof(Customer).Assembly.GetManifestResourceStream("Stargate.Handheld.Domain.ClassDefs.xml"));
            return classDefStream.ReadToEnd();
        }
    }*/
}