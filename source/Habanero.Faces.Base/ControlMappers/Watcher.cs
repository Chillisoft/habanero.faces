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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Base;

namespace Habanero.Faces.Base.ControlMappers
{
/*    internal interface IWatcher<T>
    {
        T Value { get; set; }
        event EventHandler ValueChanged;
    }

    internal interface ICompositeWatcher<TPathWatcher, TNodeWatcher> : IWatcher<TNodeWatcher>
    {
        IWatcher<TPathWatcher> PathWatcher { get; set; }
    }*/

  /*  internal class Watcher<T> : IWatcher<T>
    {
        public Watcher(string valueSource)
        {
        }

        public T Value { get; set; }
        public event EventHandler ValueChanged;
    }*/

/*    internal class CompositeWatcher<TPathWatcher, TValue> : ICompositeWatcher<TPathWatcher, TValue>
    {
        public IWatcher<TPathWatcher> PathWatcher { get; set; }
        public IWatcher<TValue> NodeWatcher { get; set; }

        public CompositeWatcher(IWatcher<TPathWatcher> pathWatcher, IWatcher<TValue> nodeWatcher)
        {
            PathWatcher = pathWatcher;
            NodeWatcher = nodeWatcher;
        }

        public TValue Value { get; set; }
        public event EventHandler ValueChanged;
    }*/

    //internal static class bob
    //{
    //    public static IWatcher<TValue> Then<TPathWatcher, TValue>(this IWatcher<TPathWatcher> pathWatcher, IWatcher<TValue> nodeWatcher)
    //    {
    //        return null;
    //    }
    //}

// ReSharper disable UnusedMember.Global
    //internal class Tester

    //{
    //    void Test()
    //    {
    //        string totalPath = "MyRel.MyRel2.MyRel3.MyRel4.MyProp";
            
    //        IWatcher<IRelationship> watcher1 = new Watcher<IRelationship>("MyRel");
    //        IWatcher<IRelationship> compositeWatcher1 = new CompositeWatcher<IRelationship, IRelationship>(watcher1, new Watcher<IRelationship>("MyRel2"));
    //        IWatcher<IRelationship> compositeWatcher2 = new CompositeWatcher<IRelationship, IRelationship>(compositeWatcher1, new Watcher<IRelationship>("MyRel3"));
    //        IWatcher<IRelationship> compositeWatcher3 = new CompositeWatcher<IRelationship, IRelationship>(compositeWatcher2, new Watcher<IRelationship>("MyRel4"));
    //        IWatcher<IBOProp> compositeWatcher4 = new CompositeWatcher<IRelationship, IBOProp>(compositeWatcher3, new Watcher<IBOProp>("MyProp"));

    //        IBOProp boProp = compositeWatcher4.Value;


    //        //IWatcher<IBOProp> watcherX = new Watcher<IRelationship>("MyRel").Then(new Watcher<IRelationship>("MyRel2")).Then(new Watcher<IBOProp>("MyProp"));
    //    }
    //}
    // ReSharper restore UnusedMember.Global
}
 