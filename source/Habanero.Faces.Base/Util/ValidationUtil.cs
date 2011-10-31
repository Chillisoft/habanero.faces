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
#region Copyright � 2005 Noogen Technologies Inc.
// Author:
//	Tommy Noogen (tom@noogen.net)
//
// (C) 2005 Noogen Technologies Inc. (http://www.noogen.net)
// 
// MIT X.11 LICENSE
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
#endregion

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace Habanero.Faces.Base
{
	/// <summary>
	/// Provide comparison of string data.  This class currently
	/// implements System.Web.UI.WebControls validation so that
	/// we don't have to write more code.  Eventually, we may want
	/// to implement out own code.
	/// </summary>
	internal static class ValidationUtil
	{
	    private static MyValidator _validator;

	    /// <summary>
		/// Disable default ctor.
		/// </summary>
//		private ValidationUtil(){}

        static ValidationUtil()
	    {
	        _validator = new MyValidator();
	    }

	    /// <summary>
		/// Compare two values using provided operator and data type.
		/// </summary>
		/// <param name="leftText"></param>
		/// <param name="rightText"></param>
		/// <param name="op"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool CompareValues(string leftText, string rightText, ValidationCompareOperator op, ValidationDataType type)
		{
			System.Web.UI.WebControls.ValidationCompareOperator vco = 
				(System.Web.UI.WebControls.ValidationCompareOperator)Enum.Parse(
					typeof(System.Web.UI.WebControls.ValidationCompareOperator), 
					op.ToString());

			System.Web.UI.WebControls.ValidationDataType vdt = 
				(System.Web.UI.WebControls.ValidationDataType)Enum.Parse(
				typeof(System.Web.UI.WebControls.ValidationDataType), 
				type.ToString());

            //if(rightText=="" && op==ValidationCompareOperator.Equal)
            //{
            //    return String.Equals("", leftText);
            //}
            return MyValidator.CompareValues(leftText, rightText, vco, vdt);
		}

        public static bool CompareTypes(string leftText, ValidationDataType type)
        {
            

            System.Web.UI.WebControls.ValidationDataType vdt =
                (System.Web.UI.WebControls.ValidationDataType)Enum.Parse(
                typeof(System.Web.UI.WebControls.ValidationDataType),
                type.ToString());

            return BaseCompareValidator.CanConvert(leftText, vdt);
        }

		/// <summary>
		/// Utility method validation regular expression.
		/// </summary>
		/// <param name="valueText"></param>
		/// <param name="patternText"></param>
		/// <returns></returns>
		public static bool ValidateRegEx(string valueText, string patternText)
		{
			Match m = Regex.Match(valueText, patternText);
			return m.Success;
		}


		#region "public static Xml Serializations"

		/// <summary>
		/// Get object from an xml string.
		/// </summary>
		/// <param name="xmlString"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static object XmlStringToObject(string xmlString, System.Type type)
		{
			System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(type);
			TextReader r = null;
			object retObj = null;
			try 
			{
				r = new StringReader( xmlString );
				retObj = x.Deserialize(r);
			}
			finally
			{
				if (r != null)
					r.Close();
			}
			return retObj;
		}

		/// <summary>
		/// Write object to xml string.
		/// </summary>
		/// <param name="obj"></param>
		public static string ObjectToXmlString(object obj)
		{
			System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(obj.GetType());
			TextWriter w = null;
			string sReturn = string.Empty;

			try 
			{
				w = new StringWriter(); 
				x.Serialize(w, obj);
				sReturn = w.ToString();
			} 
			finally
			{
				if (w != null)
					w.Close();
			}

			return sReturn;
		}
		#endregion

		#region "public static FileToString"

		/// <summary>
		/// Load the entire text file into a string.
		/// </summary>
		/// <param name="sFile">Full pathname of file to read.</param>
		/// <returns>String content of the text file.</returns>
		public static string FileToString(string sFile)
		{
			string sText = string.Empty;
			using (StreamReader sr = new StreamReader(sFile))
			{
				sText = sr.ReadToEnd();
			}
			return sText;
		}

		/// <summary>
		/// Load the text file with specified size as return text.
		/// </summary>
		/// <param name="sFile">File to read from.</param>
		/// <param name="size">Number of char to read.</param>
		/// <returns></returns>
		public static string FileToString(string sFile, int size)
		{
			char[]  cToRead = new char[size];
			string sText = string.Empty;
			using(StreamReader sr = new StreamReader(sFile))
			{
				sr.Read(cToRead, 0, size);
				sText = new string(cToRead);
			}
			return sText;
		}
		#endregion

		#region "public static StringToFile"

		/// <summary>
		/// Save a string to file.
		/// </summary>
		/// <param name="strValue">String value to save.</param>
		/// <param name="strFileName">File name to save to.</param>
		/// <param name="bAppendToFile">True - to append string to file.  Default false - overwrite file.</param>
		public static void StringToFile(string strValue, string strFileName, bool bAppendToFile)
		{
			using(StreamWriter sw = new StreamWriter(strFileName, bAppendToFile))
			{
				sw.Write(strValue);
			}
		}

		/// <summary>
		/// Save a string to file.
		/// </summary>
		public static void StringToFile(string strValue, string strFileName)
		{
			StringToFile(strValue, strFileName, false);
		}
		#endregion

	    public static bool CanConvert(string text, System.Web.UI.WebControls.ValidationDataType vdt)
	    {
	        return BaseCompareValidator.CanConvert(text, vdt);
	    }
	}
    internal class MyValidator:BaseCompareValidator
    {
        protected override bool EvaluateIsValid()
        {
            return false;
        }

        public static bool CompareValues(string leftText, string rightText, System.Web.UI.WebControls.ValidationCompareOperator vco, System.Web.UI.WebControls.ValidationDataType vdt)
        {
            return BaseCompareValidator.Compare(leftText, rightText, vco, vdt);
        }
    }
}
