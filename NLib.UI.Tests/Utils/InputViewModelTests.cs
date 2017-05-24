using System;
using NUnit.Framework;
using NLib.UI;
using NLib.UI.Bindings;

namespace NLib.UI.Tests.Utils
{
    [TestFixture]
    public class InputViewModelTests
    {
        [Test]
        public void DefaultValueSetTest()
        {
            var input = new InputViewModel<int, string>(Converters.IntStringWithExeption);

            using (var subscriber = new PropertyChangedSubscriber(input))
            {
                input.DefaultModel = 1;

                Assert.AreEqual(1, input.DefaultModel);
                Assert.AreEqual(input.DefaultModel, input.Model);
                Assert.AreEqual(Converters.IntStringWithExeption.Convert(1), input.View);

                CollectionAssert.AreEquivalent(
                    new[] { nameof(input.DefaultModel), nameof(input.Model), nameof(input.View) },
                    subscriber.Sequence);
            }
        }

        [Test]
        public void ModelSetTest()
        {
            var input = new InputViewModel<int, string>(Converters.IntStringWithExeption);
            input.DefaultModel = 1;

            using (var subscriber = new PropertyChangedSubscriber(input))
            {
                input.Model = 2;

                Assert.AreEqual(1, input.DefaultModel);
                Assert.AreEqual(2, input.Model);
                Assert.AreEqual(Converters.IntStringWithExeption.Convert(2), input.View);

                CollectionAssert.AreEquivalent(
                    new[] { nameof(input.Model), nameof(input.View) },
                    subscriber.Sequence);
            }
        }

        [Test]
        public void ViewSetTest()
        {
            var input = new InputViewModel<int, string>(Converters.IntStringWithExeption);
            input.DefaultModel = 1;

            using (var subscriber = new PropertyChangedSubscriber(input))
            {
                input.EditValue(10.ToString());

                Assert.AreEqual(10.ToString(), input.View);
                Assert.AreEqual(1, input.Model);
                Assert.AreEqual(1, input.DefaultModel);

                input.EndEditing();

                Assert.AreEqual(10.ToString(), input.View);
                Assert.AreEqual(10, input.Model);
                Assert.AreEqual(1, input.DefaultModel);

                CollectionAssert.AreEquivalent(
                    new[] { nameof(input.View), nameof(input.Model) },
                    subscriber.Sequence
                );
            }
        }

        [Test]
        public void ModelChangedHandlerTest()
        {
            var input = new InputViewModel<int, string>(Converters.IntStringWithExeption);
            input.Model = 1;

            var newValue = -1;
            var oldValue = -1;
            var count = 0;

            input.HandleModelChanged = (nwv, old) =>
            {
                newValue = nwv;
                oldValue = old;
                count++;
            };

            input.Model = 2;

            Assert.AreEqual(2, newValue);
            Assert.AreEqual(1, oldValue);
            Assert.AreEqual(1, count);

            input.EditValueAndUpdateModel(3.ToString());

            Assert.AreEqual(3, newValue);
            Assert.AreEqual(2, oldValue);
            Assert.AreEqual(2, count);
        }

        [Test]
        public void EditConvertValidationTest()
        {
            var input = new InputViewModel<int, string>(Converters.IntStringWithExeption);
            input.Model = 1;
            input.EditValueAndUpdateModel("error");

            Assert.AreEqual(1, input.Model);
            Assert.IsFalse(input.IsValid);
            Assert.AreEqual("error", input.View);
        }

        [Test]
        public void EditWithValidatorPositiveTest()
        {
            var msg = "test validation error";
            var validator = new TestValidator<int, string>(msg);
            var input = new InputViewModel<int, string>(Converters.IntStringWithExeption, validator);

            input.Model = 1;
            validator.SetIsValid(true);
            input.EditValueAndUpdateModel(3.ToString());

            Assert.AreEqual(3, input.Model);
            Assert.AreEqual(3.ToString(), input.View);
            Assert.IsTrue(input.IsValid);
            Assert.IsEmpty(input.ErrorMessage);
        }

        [Test]
        public void EditWithValidatorNegativeTest()
        {
            var msg = "test validation error";
            var validator = new TestValidator<int, string>(msg);
            var input = new InputViewModel<int, string>(Converters.IntStringWithExeption, validator);

            input.Model = 1;
            validator.SetIsValid(false);
            input.EditValueAndUpdateModel(2.ToString());

            Assert.AreEqual(1, input.Model);
            Assert.AreEqual(2.ToString(), input.View);
            Assert.IsFalse(input.IsValid);
            Assert.AreEqual(msg, input.ErrorMessage);
        }

        [Test]
        public void EditWithReverterTest()
        {
            var msg = "test validation error";
            var defValue = -1;
            var preValue = -1;
            var revertedValue = 10;

            var validator = new TestValidator<int, string>(msg);
            var reverter = new TestReverter<int, string>((def, prev) => {
                defValue = def;
                preValue = prev;

                return revertedValue;
            });

            var input = new InputViewModel<int, string>(Converters.IntStringWithExeption, validator, reverter);
            validator.SetIsValid(true);
            input.Model = 1;
            input.DefaultModel = 11;
            validator.SetIsValid(false);
            input.EditValueAndUpdateModel(2.ToString());

            Assert.AreEqual(revertedValue, input.Model);
            Assert.AreEqual(11, defValue);
            Assert.AreEqual(1, preValue);

            Assert.AreEqual(revertedValue.ToString(), input.View);
            Assert.IsFalse(input.IsValid);
            Assert.AreEqual(msg, input.ErrorMessage);
        }

        class TestReverter<TM, TV> : InputViewModel<TM, TV>.IReverter
        {
            readonly Func<TM, TM, TM> func;

            public TestReverter(Func<TM, TM, TM> func)
            {
                this.func = func;
            }

            public TM Revert(TM defaultValue, TM previousValue)
            {
                return func(defaultValue, previousValue);
            }
        }

        class TestValidator<TM, TV> : InputViewModel<TM, TV>.IValidator
        {
            private bool isValid;
            private string message;

            public TestValidator(string message)
            {
                this.message = message;
            }

            public void SetIsValid(bool isValid)
            {
                this.isValid = isValid;
            }

            public bool IsValid(TM model, out string message)
            {
                message = this.message;
                return isValid;
            }
        }
    }
}
