using System;
using NUnit.Framework;
using NLib.UI.Bindings;

namespace NLib.UI.Tests.Bindings
{
    [TestFixture]
    public class BindingFlowMainCasesTests
    {
        private string viewInitial = "view inital value";
        private string contextInitial = "context initial value";
        private string contextUpdated = "context updated";
        private string viewUpdated = "view value updated";
        private string contextPropertyUpdated = "next context value";

        [Test]
        public void SetBindingTest()
        {
            var context = new TestContext<string> { Property = contextInitial };
            var view = new TestView<string> { Value = viewInitial };

            view.Bind(view)
                .ViewProperty(v => v.Value)
                .To<TestContext<string>>(ctx => ctx.Property);

            Assert.AreEqual(contextInitial, context.Property);
            Assert.AreEqual(viewInitial, view.Value);
        }

        [Test]
        public void SetContextTest()
        {
            var context = new TestContext<string> { Property = contextInitial };
            var view = new TestView<string> { Value = viewInitial };

            view.Bind(view)
                .ViewProperty(v => v.Value)
                .To<TestContext<string>>(ctx => ctx.Property);

            BindingStorage.SetContext(view, context);

            Assert.AreEqual(contextInitial, context.Property);
            Assert.AreEqual(contextInitial, view.Value);
        }

        [Test]
        public void UpdateContextValueTest()
        {
            var context = new TestContext<string> { Property = contextInitial };
            var view = new TestView<string> { Value = viewInitial };

            view.Bind(view)
                .ViewProperty(v => v.Value)
                .To<TestContext<string>>(ctx => ctx.Property);
            
            BindingStorage.SetContext(view, context);

            context.Property = contextUpdated;

            Assert.AreEqual(contextUpdated, context.Property);
            Assert.AreEqual(contextUpdated, view.Value);
        }

        [Test]
        public void SwitchContextUpdateOnlyViewTest()
        {
            var context = new TestContext<string> { Property = contextInitial };
            var context2 = new TestContext<string> { Property = contextUpdated };
            var view = new TestView<string> { Value = viewInitial };

            view.Bind(view)
                .ViewProperty(v => v.Value)
                .To<TestContext<string>>(ctx => ctx.Property);

            BindingStorage.SetContext(view, context);
            BindingStorage.SetContext(view, context2);

            Assert.AreEqual(contextInitial, context.Property);
            Assert.AreEqual(contextUpdated, context2.Property);
            Assert.AreEqual(contextUpdated, view.Value);
        }

        [Test]
        public void DetachedContextDoesNotAffectViewTest()
        {
            var context = new TestContext<string> { Property = contextInitial };
            var context2 = new TestContext<string> { Property = contextUpdated };
            var view = new TestView<string> { Value = viewInitial };

            view.Bind(view)
                .ViewProperty(v => v.Value)
                .To<TestContext<string>>(ctx => ctx.Property);

            BindingStorage.SetContext(view, context);
            BindingStorage.SetContext(view, context2);

            context.Property = contextPropertyUpdated;

            Assert.AreEqual(contextPropertyUpdated, context.Property);
            Assert.AreEqual(contextUpdated, context2.Property);
            Assert.AreEqual(contextUpdated, view.Value);
        }

        [Test]
        public void ViewUpdateContextValueTest()
        {
            var context = new TestContext<string> { Property = contextInitial };
            var view = new TestView<string> { Value = viewInitial };

            view.Bind(view)
                .ViewProperty(v => v.Value)
                .ViewEvent(nameof(view.ValueChanged))
                .To<TestContext<string>>(ctx => ctx.Property);
            
            BindingStorage.SetContext(view, context);

            view.Value = viewUpdated;

            Assert.AreEqual(viewUpdated, context.Property);
            Assert.AreEqual(viewUpdated, view.Value);
        }

        [Test]
        public void ContextUpdateViewValueTest()
        {
            var context = new TestContext<string> { Property = contextInitial };
            var view = new TestView<string> { Value = viewInitial };

            view.Bind(view)
                .ViewProperty(v => v.Value)
                .ViewEvent(nameof(view.ValueChanged))
                .To<TestContext<string>>(ctx => ctx.Property);

            BindingStorage.SetContext(view, context);
            context.Property = contextUpdated;

            Assert.AreEqual(contextUpdated, context.Property);
            Assert.AreEqual(contextUpdated, view.Value);
        }

        [Test]
        public void ContextCanUpdateManyViewsTest()
        {
            var context = new TestContext<string> { Property = contextInitial };
            var view = new TestView<string> { Value = viewInitial };
            var view2 = new TestView<string> { Value = viewInitial };

            view.Bind(view)
                .ViewProperty(v => v.Value)
                .To<TestContext<string>>(ctx => ctx.Property);

            view.Bind(view2)
                .ViewProperty(v => v.Value)
                .To<TestContext<string>>(ctx => ctx.Property);

            BindingStorage.SetContext(view, context);
            BindingStorage.SetContext(view2, context);

            context.Property = contextUpdated;

            Assert.AreEqual(contextUpdated, context.Property);
            Assert.AreEqual(contextUpdated, view.Value);
            Assert.AreEqual(contextUpdated, view2.Value);
        }

        [Test]
        public void SwitchedContextDoesNotUpdatedTest()
        {
            var context = new TestContext<string> { Property = contextInitial };
            var context2 = new TestContext<string> { Property = contextUpdated };
            var view = new TestView<string> { Value = viewInitial };

            view.Bind(view)
                .ViewProperty(v => v.Value)
                .ViewEvent(nameof(view.ValueChanged))
                .To<TestContext<string>>(ctx => ctx.Property);

            BindingStorage.SetContext(view, context);
            BindingStorage.SetContext(view, context2);

            view.Value = viewUpdated;

            Assert.AreEqual(contextInitial, context.Property);
            Assert.AreEqual(viewUpdated, context2.Property);
            Assert.AreEqual(viewUpdated, view.Value);
        }

        [Test]
        public void OneWayBindingFullFlowTest()
        {
           var context = new TestContext<string> { Property = contextInitial };
            var view = new TestView<string> { Value = viewInitial };

            // set binding => nothing should be cnahged
            view.Bind(view)
                .ViewProperty(v => v.Value)
                .To<TestContext<string>>(ctx => ctx.Property);

            Assert.AreEqual(contextInitial, context.Property);
            Assert.AreEqual(viewInitial, view.Value);

            // set context => view should be updated
            BindingStorage.SetContext(view, context);

            Assert.AreEqual(contextInitial, context.Property);
            Assert.AreEqual(contextInitial, view.Value);

            // context update => view should be updated
            context.Property = contextUpdated;

            Assert.AreEqual(contextUpdated, context.Property);
            Assert.AreEqual(contextUpdated, view.Value);
        }

        [Test]
        public void TwoWayBindingFullFlowTest()
        {
            var context = new TestContext<string> { Property = contextInitial };
            var view = new TestView<string> { Value = viewInitial };

            // set binding => nothing should be cnahged
            view.Bind(view)
                .ViewProperty(v => v.Value)
                .ViewEvent(nameof(view.ValueChanged))
                .To<TestContext<string>>(ctx => ctx.Property);

            Assert.AreEqual(contextInitial, context.Property);
            Assert.AreEqual(viewInitial, view.Value);

            // set context => view should be updated
            BindingStorage.SetContext(view, context);

            Assert.AreEqual(contextInitial, context.Property);
            Assert.AreEqual(contextInitial, view.Value);

            // view updated > context shold be updated
            view.Value = viewUpdated;

            Assert.AreEqual(viewUpdated, context.Property);
            Assert.AreEqual(viewUpdated, view.Value);

            // context update => view should be updated
            context.Property = contextUpdated;

            Assert.AreEqual(contextUpdated, context.Property);
            Assert.AreEqual(contextUpdated, view.Value);
        }

        [Test]
        public void BindingConverterFullFlowTest()
        {
            var viewInitialLocal = "100";
            var contextInitialLocal = 200;
            var viewUpdatedLocal = "300";
            var contextUpdatedLocal = 400;

            var context = new TestContext<int> { Property = contextInitialLocal };
            var view = new TestView<string> { Value = viewInitialLocal };

            // set binding => nothing should be cnahged
            view.Bind(view)
                .ViewProperty(v => v.Value)
                .ViewEvent(nameof(view.ValueChanged))
                .Converter(Converters.IntString)
                .To<TestContext<int>>(ctx => ctx.Property);

            Assert.AreEqual(contextInitialLocal, context.Property);
            Assert.AreEqual(viewInitialLocal, view.Value);

            // set context => view should be updated
            BindingStorage.SetContext(view, context);

            Assert.AreEqual(contextInitialLocal, context.Property);
            Assert.AreEqual(Converters.IntString.Convert(contextInitialLocal), view.Value);

            // view updated > context shold be updated
            view.Value = viewUpdatedLocal;

            Assert.AreEqual(Converters.IntString.ConvertBack(viewUpdatedLocal), context.Property);
            Assert.AreEqual(viewUpdatedLocal, view.Value);

            // context update => view should be updated
            context.Property = contextUpdatedLocal;

            Assert.AreEqual(contextUpdatedLocal, context.Property);
            Assert.AreEqual(Converters.IntString.Convert(contextUpdatedLocal), view.Value);
        }
    }
}