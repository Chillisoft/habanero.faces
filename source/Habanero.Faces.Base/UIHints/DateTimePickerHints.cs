using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Base.UIHints
{
    public class DateTimePickerHints : ControlHints
    {
        public string DefaultFormat // redirect to GlobalUIRegistry.DateDisplaySettings
        {
            get 
            { 
                if (GlobalUIRegistry.DateDisplaySettings == null)
                    return null;
                return GlobalUIRegistry.DateDisplaySettings.DateTimePickerDefaultFormat; 
            }
            set
            {
                if (GlobalUIRegistry.DateDisplaySettings == null)
                    GlobalUIRegistry.DateDisplaySettings = new DateDisplaySettings();
                GlobalUIRegistry.DateDisplaySettings.DateTimePickerDefaultFormat = value;
                RunOnHintsChangedHandler();
            }
        }
    }
}
