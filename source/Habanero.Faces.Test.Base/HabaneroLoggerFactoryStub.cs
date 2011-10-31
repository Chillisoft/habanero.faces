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
using System;
using Habanero.Base;
using Habanero.Base.Logging;

namespace Habanero.Faces.Test.Base
{
    public class HabaneroLoggerFactoryStub : IHabaneroLoggerFactory
    {
        private readonly HabaneroLoggerStub _habaneroLoggerStub = new HabaneroLoggerStub();

        public IHabaneroLogger GetLogger(string contextName)
        {
            return _habaneroLoggerStub;
        }

        public IHabaneroLogger GetLogger(Type type)
        {
            return _habaneroLoggerStub;
        }
    }

    public class HabaneroLoggerStub : IHabaneroLogger
    {
        public void Log(string message)
        {

        }

        public void Log(string message, LogCategory logCategory)
        {

        }

        public void Log(Exception exception)
        {
        }

        public void Log(string message, Exception exception)
        {
        }

        public void Log(string message, Exception exception, LogCategory logCategory)
        {
        }

    	public bool IsLogging(LogCategory logCategory)
    	{
    		return false;
    	}

    	public string ContextName
        {
            get { return "HabaneroLoggerStub (logger for testing only)"; }
        }
    }
}