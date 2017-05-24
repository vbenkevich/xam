//using System;
//using NUnit.Framework;
//using NLib.UI.Bindings;

//namespace NLib.UI.Tests.Bindings
//{
//    [TestFixture]
//    public class BindingStorageTests
//    {
//        [Test]
//        public void SimpleSet()
//        {
//            var context = new TestContext<int>() { Property = 1 };
//            var view = new TestView<int>() { Value = 0 };

//            BindingStorage.SetBinding(view, view,
//                new OneWayBinding<TestView<int>, TestContext<int>, int>(
//                    nameof(context.Property),
//                    vm => vm.Property,
//                    (v, val) => v.Value = val));

//            Assert.AreEqual(0, view.Value);

//            BindingStorage.SetContext(view, context);

//            Assert.AreEqual(context.Property, view.Value);

//            context.Property = 2;

//            Assert.AreEqual(context.Property, view.Value);
//        }
//    }
//}
