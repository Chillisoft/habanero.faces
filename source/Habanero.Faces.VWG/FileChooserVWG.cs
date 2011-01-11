// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using Habanero.Faces.Base;

namespace Habanero.Faces.VWG
{
    /// <summary>
    /// Provides a means to choose a file and makes the selected
    /// path available to a control that needs it
    /// </summary>
    public class FileChooserVWG : UserControlVWG, IFileChooser 
    {
        private readonly IControlFactory _controlFactory;
        private readonly FileChooserManager _fileChooserManager;
        /// <summary>
        /// Constructor for <see cref="FileChooserVWG"/>
        /// </summary>
        /// <param name="controlFactory"></param>
        public FileChooserVWG(IControlFactory controlFactory)
        {
            this._controlFactory = controlFactory;
            _fileChooserManager = new FileChooserManager(controlFactory,this);
        }

        /// <summary>
        /// Gets or sets the selected file path
        /// </summary>
        public string SelectedFilePath
        {
            get { return _fileChooserManager.SelectedFilePath; }
            set { _fileChooserManager.SelectedFilePath = value; }
        }
    }
}
