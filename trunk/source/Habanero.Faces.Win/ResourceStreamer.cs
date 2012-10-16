using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Win
{
    public class ResourceStreamer
    {
        public static Stream GetResourceStreamByName(string name)
        {
            var splitChars = new char[] { '.' };
            var splitName = name.Split(splitChars);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var rName in asm.GetManifestResourceNames())
                {
                    var parts = rName.Split(splitChars);
                    if (ResourceStreamer.RightMatchStringArray(parts, splitName))
                    {
                        return asm.GetManifestResourceStream(rName);
                    }
                }
            }
            return null;
        }

        public static bool RightMatchStringArray(string[] haystack, string[] needles)
        {
            if (needles.Length > haystack.Length) return false;
            var nlen = needles.Length;
            var hlen = haystack.Length;
            for (var offset = 1; offset <= needles.Length; offset++)
            {
                if (haystack[hlen - offset] != needles[nlen - offset])
                    return false;
            }
            return true;
        }
    }
}
