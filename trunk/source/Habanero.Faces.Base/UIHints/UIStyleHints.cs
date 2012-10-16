using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Base.UIHints
{
    public class UIStyleHints : HintsBase
    {
        // override base so we can re-attach sub-hints
        public new EventHandler OnHintsChanged
        {
            get { return base.OnHintsChanged; }
            set
            {
                this.UnBindHintsChangedEventHandlers();
                base.OnHintsChanged = value;
                this.BindHintsChangedEventHandlers();
            }
        }

        private void UnBindHintsChangedEventHandlers()
        {
            if (this.OnHintsChanged != null)
            {
                foreach (var hb in this.GetSubHints())
                {
                    hb.OnHintsChanged -= this.OnHintsChanged;
                }
            }
        }
        private void BindHintsChangedEventHandlers()
        {
            if (this.OnHintsChanged != null)
            {
                foreach (var hb in this.GetSubHints())
                {
                    hb.OnHintsChanged += this.OnHintsChanged;
                }
            }
        }

        private HintsBase[] GetSubHints()
        {
            return new HintsBase[]
                {
                    this.ButtonHints,
                    this.LabelHints,
                    this.TextBoxHints,
                    this.DateTimePickerHints,
                    this.ComboBoxHints,
                    this.CheckBoxHints,
                    this.GridHints,
                    this.FormHints,
                    this.StaticDataEditorManagerHints,
                    this.LayoutHints
                };
        }

        public ControlHints ButtonHints 
        { 
            get { return _buttonHints; } 
            set { _buttonHints = value; OnHintsObjectChanged(_buttonHints); } 
        }
        private ControlHints _buttonHints;

        public ControlHints LabelHints { 
            get { return _labelHints; } 
            set { _labelHints = value; OnHintsObjectChanged(_labelHints); } 
        }
        private ControlHints _labelHints;

        public ControlHints TextBoxHints 
        { 
            get { return _textBoxHints; } 
            set { _textBoxHints = value; OnHintsObjectChanged(_textBoxHints); } 
        }
        private ControlHints _textBoxHints;

        public DateTimePickerHints DateTimePickerHints 
        { 
            get { return _dateTimePickerHints; } 
            set { _dateTimePickerHints = value; OnHintsObjectChanged(_dateTimePickerHints); } 
        }
        private DateTimePickerHints _dateTimePickerHints;

        public ComboBoxHints ComboBoxHints 
        { 
            get { return _comboBoxHints; } 
            set { _comboBoxHints = value; OnHintsObjectChanged(_comboBoxHints); } 
        }
        private ComboBoxHints _comboBoxHints;

        public ControlHints CheckBoxHints 
        { 
            get { return _checkBoxHints; } 
            set { _checkBoxHints = value; OnHintsObjectChanged(_checkBoxHints); } 
        }
        private ControlHints _checkBoxHints;

        public GridHints GridHints 
        { 
            get { return _gridHints; } 
            set { _gridHints = value; OnHintsObjectChanged(_gridHints); } 
        }
        private GridHints _gridHints;

        public FormHints FormHints 
        { 
            get { return _formHints; } 
            set { _formHints = value; OnHintsObjectChanged(_formHints); } 
        }
        private FormHints _formHints;

        public StaticDataEditorManagerHints StaticDataEditorManagerHints 
        { 
            get { return _staticDataEditorManagerHints; } 
            set { _staticDataEditorManagerHints = value; OnHintsObjectChanged(_staticDataEditorManagerHints); } 
        }
        private StaticDataEditorManagerHints _staticDataEditorManagerHints;

        public LayoutHints LayoutHints 
        { 
            get { return _layoutHints; } 
            set { _layoutHints = value; OnHintsObjectChanged(_layoutHints); } 
        }
        private LayoutHints _layoutHints;

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
            this.FormHints = new FormHints();
            this.StaticDataEditorManagerHints = new StaticDataEditorManagerHints();
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

        private void OnHintsObjectChanged(HintsBase obj)
        {
            if (this.OnHintsChanged == null) return;
            obj.OnHintsChanged += this.OnHintsChanged;
            this.RunOnHintsChangedHandler();
        }
    }
}
