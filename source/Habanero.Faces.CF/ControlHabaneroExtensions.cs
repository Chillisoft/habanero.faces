using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.CF.Adapters;

namespace Habanero.Faces.CF
{// ReSharper disable SuspiciousTypeConversion.Global
    /// <summary>
    /// Extension method for IControl Habanero that make it easier to work with ControlAdapters.
    /// </summary>
    public static class ControlHabaneroExtensions
    {
        public static Control GetControl(this IControlHabanero control)
        {

            var myControl = control as Control;
            if (myControl != null) return myControl;

            var controlAdapter = control as IWinFormsControlAdapter;
            if (controlAdapter != null)
            {
                return controlAdapter.WrappedControl;
            }
            return null;
        }
        public static Control GetNextControl(this Control control, Control currentControl, bool moveForward)
        {
            return currentControl;
            /*            var myControl = control as Control;
                        if (myControl != null) return myControl;

                        var controlAdapter = control as IWinFormsControlAdapter;
                        if (controlAdapter != null)
                        {
                            return controlAdapter.WrappedControl;
                        }
                        return null;*/
        }
        // ReSharper restore SuspiciousTypeConversion.Global
        public static Control GetControl(this IControlMapper controlMapper)
        {
            return controlMapper.Control == null ? null : controlMapper.Control.GetControl();
        }
    }

/*
    /// <summary>
    /// Extension method for IControl Habanero that make it easier to work with ControlAdapters.
    /// </summary>
    internal static class ControlHabaneroExtensions
    {
        internal static Control GetControl(this IControlHabanero control)
        {
            var myControl = control as Control;
            if (myControl != null) return myControl;

            var controlAdapter = control as IWinFormsControlAdapter;
            if (controlAdapter != null)
            {
                return controlAdapter.WrappedControl;
            }
            return null;
        }

        internal static Control GetControl(this IControlMapper controlMapper)
        {
            return controlMapper.Control == null ? null : controlMapper.Control.GetControl();
        }
    }*/
}
