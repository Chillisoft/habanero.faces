using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Base.UIHints
{
    public class UIStyleHints
    {
        public ControlHints ButtonHints { get; set; }
        public ControlHints LabelHints { get; set; }
        public ControlHints TextBoxHints { get; set; }
        public DateTimePickerHints DateTimePickerHints { get; set; }
        public ComboBoxHints ComboBoxHints { get; set; }
        public ControlHints CheckBoxHints { get; set; }
        public GridHints GridHints { get; set; }

        public LayoutHints LayoutHints { get; set; }

        public UIStyleHints()
        {
            this.ButtonHints = new ControlHints();
            this.LabelHints = new ControlHints();
            this.LayoutHints = new LayoutHints();
            this.DateTimePickerHints = new DateTimePickerHints();
            this.ComboBoxHints = new ComboBoxHints();
            this.TextBoxHints = new ControlHints();
            this.CheckBoxHints = new ControlHints();
            this.GridHints = new GridHints();
        }

        public void SetAllControlHints(ControlHints setter)
        {
            var settings = new ControlHints[] {
                this.ButtonHints,
                this.LabelHints,
                this.TextBoxHints,
                this.DateTimePickerHints,
                this.ComboBoxHints,
                this.CheckBoxHints
            };

            foreach (var s in settings)
            {
                s.Clone(setter);
            }
        }
    }
}
