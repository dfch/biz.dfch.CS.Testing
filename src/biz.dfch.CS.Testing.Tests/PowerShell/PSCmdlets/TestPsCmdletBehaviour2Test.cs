/**
 * Copyright 2016 d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
using biz.dfch.CS.Testing.Attributes;
using biz.dfch.CS.Testing.PowerShell;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Testing.Tests.PowerShell.PSCmdlets
{
    [TestClass]
    public class TestPsCmdletBehaviour22Test
    {
        [TestMethod]
        [ExpectParameterBindingValidationException(MessagePattern = "RequiredStringParameter")]
        public void InvokeWithEmptyStringPropertyValueThrowsParameterBindingValidationException()
        {
            var parameters = @"-RequiredStringParameter ''";
            var results = new PsCmdletAssert2().Invoke(typeof(TestPsCmdletBehaviour2), parameters);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        [ExpectParameterBindingValidationException]
        public void InvokeWithNullStringPropertyValueThrowsParameterBindingValidationException()
        {
            var parameters = @"-RequiredStringParameter $null";
            var results = new PsCmdletAssert2().Invoke(typeof(TestPsCmdletBehaviour2), parameters);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void InvokeWithValidStringPropertyValueSucceeds()
        {
            var value = "arbitrary-RequiredStringParameter-value";
            var parameters = string.Format(@"-RequiredStringParameter '{0}'", value);
            var results = new PsCmdletAssert2().Invoke(typeof(TestPsCmdletBehaviour2), parameters);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            var result = results[0].ToString();
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void InvokeWithValidStringPropertyValueWithAliasSucceeds()
        {
            var value = "arbitrary-RequiredStringParameter-value";
            var parameters = string.Format(@"-name '{0}'", value);
            var results = new PsCmdletAssert2().Invoke(typeof(TestPsCmdletBehaviour2), parameters);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            var result = results[0].ToString();
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void TestAliasesSucceeds()
        {
            new PsCmdletAssert2().HasAlias(typeof(TestPsCmdletBehaviour2), "Test-PsCmdletBehaviourWithAnAlias1");
            new PsCmdletAssert2().HasAlias(typeof(TestPsCmdletBehaviour2), "Test-PsCmdletBehaviourWithAnAlias2");
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void TestInexistentAliasThrowsAssertFailedException()
        {
            new PsCmdletAssert2().HasAlias(typeof(TestPsCmdletBehaviour2), "Test-PsCmdletBehaviourWithAnAlias3");
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void TestInexistentAliasThrowsAssertFailedExceptionWithMessage()
        {
            var alias = "Test-PsCmdletBehaviourWithAnAlias4";
            var message = "Alias does not exist or is not defined.";
            try
            {
                new PsCmdletAssert2().HasAlias(typeof(TestPsCmdletBehaviour2), alias, message);
            }
            catch (AssertFailedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("PsCmdletAssert2.IsAliasDefined"));
                Assert.IsTrue(ex.Message.Contains(alias));
                Assert.IsTrue(ex.Message.Contains(message));

                throw;
            }
        }

        [TestMethod]
        public void InvokeWithParameterSetNameDefaultReturnsString()
        {
            var value = "arbitrary-RequiredStringParameter-value";
            var parameters = string.Format(@"-RequiredStringParameter '{0}'", value);

            var results = new PsCmdletAssert2().Invoke(typeof(TestPsCmdletBehaviour2), parameters);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);

            var result = results[0].BaseObject;
            Assert.IsTrue(result is string);
            Assert.AreEqual(value, result);

            new PsCmdletAssert2().HasOutputType(typeof(TestPsCmdletBehaviour2), result.GetType(), TestPsCmdletBehaviour2.ParametersSets.DEFAULT);
        }

        [TestMethod]
        public void ParameterSetNameDefaultDefinesOutputTypeString()
        {
            new PsCmdletAssert2().HasOutputType(typeof(TestPsCmdletBehaviour2), typeof(string), TestPsCmdletBehaviour2.ParametersSets.DEFAULT);
        }

        [TestMethod]
        public void ParameterSetNameDefaultDefinesOutputTypeFloat()
        {
            new PsCmdletAssert2().HasOutputType(typeof(TestPsCmdletBehaviour2), typeof(float), TestPsCmdletBehaviour2.ParametersSets.DEFAULT);
        }

        [TestMethod]
        public void ParameterSetNameDefaultDefinesOutputTypeDouble()
        {
            new PsCmdletAssert2().HasOutputType(typeof(TestPsCmdletBehaviour2), typeof(double), TestPsCmdletBehaviour2.ParametersSets.DEFAULT);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void ParameterSetNameDefaultDefinesThrowsAssertionFailedException()
        {
            try
            {
                new PsCmdletAssert2().HasOutputType(typeof(TestPsCmdletBehaviour2), typeof(int), TestPsCmdletBehaviour2.ParametersSets.DEFAULT);
            }
            catch (AssertFailedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("PsCmdletAssert2.IsOutputType"));
                Assert.IsTrue(ex.Message.Contains(typeof(int).FullName));
                Assert.IsTrue(ex.Message.Contains(TestPsCmdletBehaviour2.ParametersSets.DEFAULT));

                throw;
            }
        }

        [TestMethod]
        public void ParameterSetNameAllDefinesOutputTypeDouble()
        {
            new PsCmdletAssert2().HasOutputType(typeof(TestPsCmdletBehaviour2), typeof(double));
        }

        [TestMethod]
        public void ParameterSetNameValueDefinesOutputTypeLong()
        {
            new PsCmdletAssert2().HasOutputType(typeof(TestPsCmdletBehaviour2), typeof(long), TestPsCmdletBehaviour2.ParametersSets.VALUE);
        }

        [TestMethod]
        public void ParameterSetNameValueDefinesOutputTypeFloat()
        {
            new PsCmdletAssert2().HasOutputType(typeof(TestPsCmdletBehaviour2), typeof(float), TestPsCmdletBehaviour2.ParametersSets.VALUE);
        }

        [TestMethod]
        [ExpectParameterBindingValidationException(MessagePattern = "RequiredIntParameter")]
        public void InvokeWithValue0ThrowsParameterBindingException()
        {
            var value = 0;
            var parameters = string.Format(@"-RequiredIntParameter {0}", value);

            var results = new PsCmdletAssert2().Invoke(typeof(TestPsCmdletBehaviour2), parameters);
        }

        [TestMethod]
        public void InvokeWithValue1ReturnsInt()
        {
            var value = 1;
            var parameters = string.Format(@"-RequiredIntParameter {0}", value);

            var results = new PsCmdletAssert2().Invoke(typeof(TestPsCmdletBehaviour2), parameters);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);

            var result = results[0].BaseObject;
            Assert.IsTrue(result is long);
            Assert.AreEqual((long)value * 2, result);

            new PsCmdletAssert2().HasOutputType(typeof(TestPsCmdletBehaviour2), result.GetType(), TestPsCmdletBehaviour2.ParametersSets.VALUE);
        }

        [TestMethod]
        [ExpectAssertFailedException(@"PsCmdletAssert2.HasOutputType FAILED. ExpectedType 'System.String' not defined for ParameterSetName 'value'")]
        public void InvokeWithValue8ReturnsString()
        {
            var value = 8;
            var parameters = string.Format(@"-RequiredIntParameter {0}", value);

            var results = new PsCmdletAssert2().Invoke(typeof(TestPsCmdletBehaviour2), parameters);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);

            var result = results[0].BaseObject;
            Assert.IsTrue(result is string);
            Assert.AreEqual(value.ToString(), result);

            new PsCmdletAssert2().HasOutputType(typeof(TestPsCmdletBehaviour2), result.GetType(), TestPsCmdletBehaviour2.ParametersSets.VALUE);
        }

        [TestMethod]
        public void InvokeWithValue15ReturnsLong()
        {
            var value = 15;
            var parameters = string.Format(@"-RequiredIntParameter {0}", value);

            var results = new PsCmdletAssert2().Invoke(typeof(TestPsCmdletBehaviour2), parameters);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);

            var result = results[0].BaseObject;
            Assert.IsTrue(result is long);
            Assert.AreEqual((long)value * 2, result);

            new PsCmdletAssert2().HasOutputType(typeof(TestPsCmdletBehaviour2), result.GetType(), TestPsCmdletBehaviour2.ParametersSets.VALUE);
        }

        [TestMethod]
        public void InvokeWithValue15ReturnsLongAndGeneratesErrorRecord()
        {
            var value = 15;
            var parameters = string.Format(@"-RequiredIntParameter {0}", value);

            Action<IList<ErrorRecord>> errorHandler = errorRecords =>
            {
                Assert.AreEqual(1, errorRecords.Count);
                var errorRecord = errorRecords[0];
                Assert.AreEqual(value, (int)errorRecord.TargetObject);
                Assert.IsInstanceOfType(errorRecord.Exception, typeof(ArgumentException));

                return;
            };

            var results = new PsCmdletAssert2().Invoke(typeof(TestPsCmdletBehaviour2), parameters, null, errorHandler);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);

            var result = results[0].BaseObject;
            Assert.IsTrue(result is long);
            Assert.AreEqual((long)value * 2, result);

            new PsCmdletAssert2().HasOutputType(typeof(TestPsCmdletBehaviour2), result.GetType(), TestPsCmdletBehaviour2.ParametersSets.VALUE);
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void InvokeWithValue42ThrowsCmdletInvocationException()
        {
            var value = 42;
            var parameters = string.Format(@"-RequiredIntParameter {0}", value);

            var results = new PsCmdletAssert2().Invoke(typeof(TestPsCmdletBehaviour2), parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvokeWithValue42ThrowsArgumentException()
        {
            var value = 42;
            var parameters = string.Format(@"-RequiredIntParameter {0}", value);

            Func<Exception, Exception> exceptionHandler = exceptionThrown =>
            {
                Assert.IsInstanceOfType(exceptionThrown, typeof(ArgumentException));
                Assert.IsTrue(exceptionThrown.Message.Contains("RequiredIntParameter"));
                Assert.IsTrue(exceptionThrown.Message.Contains("Invalid int value"));

                return exceptionThrown;
            };

            var results = new PsCmdletAssert2().Invoke(typeof(TestPsCmdletBehaviour2), parameters, exceptionHandler);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);

            var result = results[0].BaseObject;
            Assert.IsTrue(result is long);
            Assert.AreEqual((long)value * 2, result);

            new PsCmdletAssert2().HasOutputType(typeof(TestPsCmdletBehaviour2), result.GetType(), TestPsCmdletBehaviour2.ParametersSets.VALUE);
        }

    }
}
