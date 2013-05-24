using System;
using System.Collections.Concurrent;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    [TestFixture]
    public class TestBaseWithDisposing
    {
        private readonly ConcurrentStack<IDisposable> _objectsToDispose = new ConcurrentStack<IDisposable>();

        [TestFixtureTearDown]
        public void TestFixtureTearDown_DisposeObjects()
        {
            //Code that is executed after each and every test is executed in this fixture/class.
            IDisposable disposable;
            while (_objectsToDispose.TryPop(out disposable))
            {
                disposable.Dispose();
            }
        }

        protected void DisposeOnTearDown(object obj)
        {
            var disposable = obj as IDisposable;
            if (disposable != null) _objectsToDispose.Push(disposable);
        }

        protected T GetControlledLifetimeFor<T>(T obj)
        {
            DisposeOnTearDown(obj);
            return obj;
        }
    }
}