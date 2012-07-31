using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Base.ControlMappers
{
    public class ExtendedTextBoxMapperReadOnly : ExtendedTextBoxMapper
    {
        public ExtendedTextBoxMapperReadOnly(IExtendedTextBox ctl, string propName, bool isReadOnly, IControlFactory controlFactory) 
            : base(ctl, propName, isReadOnly, controlFactory)
        {
            this.EnableEditing = false;
        }
    }
}
