using System;
using NLib.UI.Bindings;
using NLib.UI.Tests.Utils;
using NUnit.Framework;
using NLib.UI.MVVM;
using System.ComponentModel;
using System.Threading.Tasks;

namespace NLib.UI.Tests.Bindings
{
    [TestFixture]
    public class BindingCollectionTests
    {
        [Test]
        public async Task DontKeepOwnerTest()
        {
            var view = new View();
            var collection = new BindingCollection<Context>(view);

            Assert.IsTrue(collection.IsAlive);

            var reference = GCHelper.Kill(ref view);

            await GCHelper.ForceCollect();

            Assert.IsFalse(collection.IsAlive);
            Assert.IsFalse(reference.IsAlive);
        }

        [Test]
        public async Task KeepContextTest()
        {
            var view = new View();
            var collection = new BindingCollection<Context>(view);
            var context = new Context();

            collection.TrySetContext(context);

            var reference = GCHelper.Kill(ref context);

            await GCHelper.ForceCollect();

            Assert.IsTrue(reference.IsAlive);
        }

        [Test]
        public void PropertyChangedTriggerUpdate()
        {
            var view = new TestView<int>();
            var collection = new BindingCollection<Context>(view);

            //var binding = 
        }

        class View : TestView<int>
        {
        }

        class Context : TestContext<int>
        {
        }
    }
}
