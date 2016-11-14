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
using biz.dfch.CS.Testing.Attributes;
using biz.dfch.CS.Testing.PowerShell;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Testing.Tests.PowerShell.PSCmdlets
{
    [TestClass]
    public class TestPsCmdletBehaviourTest
    {
        [TestMethod]
        [ExpectParameterBindingValidationException(MessagePattern = "RequiredStringParameter")]
        public void InvokeWithEmptyStringPropertyValueThrowsParameterBindingValidationException()
        {
            var commandText = @"-RequiredStringParameter ''";
            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), commandText);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        [ExpectParameterBindingValidationException]
        public void InvokeWithNullStringPropertyValueThrowsParameterBindingValidationException()
        {
            var parameters = @"-RequiredStringParameter $null";
            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), parameters);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void InvokeWithValidStringPropertyValueSucceeds()
        {
            var stringPropertyValue = "arbitrary-RequiredStringParameter-value";
            var parameters = string.Format(@"-RequiredStringParameter '{0}'", stringPropertyValue);
            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), parameters);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            var result = results[0].ToString();
            Assert.AreEqual(stringPropertyValue, result);
        }

        [TestMethod]
        public void InvokeWithValidStringPropertyValueWithAliasSucceeds()
        {
            var stringPropertyValue = "arbitrary-RequiredStringParameter-value";
            var parameters = string.Format(@"-name '{0}'", stringPropertyValue);
            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), parameters);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            var result = results[0].ToString();
            Assert.AreEqual(stringPropertyValue, result);
        }

        [TestMethod]
        public void TestAliasesSucceeds()
        {
            PsCmdletAssert.IsAliasDefined(typeof(TestPsCmdletBehaviour), "Test-PsCmdletBehaviourWithAnAlias1");
            PsCmdletAssert.IsAliasDefined(typeof(TestPsCmdletBehaviour), "Test-PsCmdletBehaviourWithAnAlias2");
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void TestInexistentAliasThrowsAssertFailedException()
        {
            PsCmdletAssert.IsAliasDefined(typeof(TestPsCmdletBehaviour), "Test-PsCmdletBehaviourWithAnAlias3");
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void TestInexistentAliasThrowsAssertFailedExceptionWithMessage()
        {
            var alias = "Test-PsCmdletBehaviourWithAnAlias4";
            var message = "Alias does not exist or is not defined.";
            try
            {
                PsCmdletAssert.IsAliasDefined(typeof(TestPsCmdletBehaviour), alias, message);
            }
            catch (AssertFailedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("PsCmdletAssert.IsAliasDefined"));
                Assert.IsTrue(ex.Message.Contains(alias));
                Assert.IsTrue(ex.Message.Contains(message));

                throw;
            }
        }

        [TestMethod]
        public void InvokeWithParameterSetNameDefaultReturnsString()
        {
            var stringPropertyValue = "arbitrary-RequiredStringParameter-value";
            var parameters = string.Format(@"-RequiredStringParameter '{0}'", stringPropertyValue);

            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), parameters);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);

            var result = results[0].BaseObject;
            Assert.IsTrue(result is string);
            Assert.AreEqual(stringPropertyValue, result);

            PsCmdletAssert.IsOutputType(typeof(TestPsCmdletBehaviour), result.GetType(), TestPsCmdletBehaviour.ParametersSets.DEFAULT);
        }

        [TestMethod]
        public void ParameterSetNameDefaultDefinesOutputTypeString()
        {
            PsCmdletAssert.IsOutputType(typeof(TestPsCmdletBehaviour), typeof(string),
                TestPsCmdletBehaviour.ParametersSets.DEFAULT);
        }

        [TestMethod]
        public void ParameterSetNameDefaultDefinesOutputTypeFloat()
        {
            PsCmdletAssert.IsOutputType(typeof(TestPsCmdletBehaviour), typeof(float),
                TestPsCmdletBehaviour.ParametersSets.DEFAULT);
        }

        [TestMethod]
        public void ParameterSetNameDefaultDefinesOutputTypeDouble()
        {
            PsCmdletAssert.IsOutputType(typeof(TestPsCmdletBehaviour), typeof(double),
                TestPsCmdletBehaviour.ParametersSets.DEFAULT);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void ParameterSetNameDefaultDefinesThrowsAssertionFailedException()
        {
            try
            {
                PsCmdletAssert.IsOutputType(typeof(TestPsCmdletBehaviour), typeof(int), TestPsCmdletBehaviour.ParametersSets.DEFAULT);
            }
            catch (AssertFailedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("PsCmdletAssert.IsOutputType"));
                Assert.IsTrue(ex.Message.Contains(typeof(int).FullName));
                Assert.IsTrue(ex.Message.Contains(TestPsCmdletBehaviour.ParametersSets.DEFAULT));                
                
                throw;
            }
        }
            
        [TestMethod]
        public void ParameterSetNameAllDefinesOutputTypeDouble()
        {
            PsCmdletAssert.IsOutputType(typeof(TestPsCmdletBehaviour), typeof(double));
        }
            
        [TestMethod]
        public void ParameterSetNameValueDefinesOutputTypeLong()
        {
            PsCmdletAssert.IsOutputType(typeof(TestPsCmdletBehaviour), typeof(long), TestPsCmdletBehaviour.ParametersSets.VALUE);
        }
            
        [TestMethod]
        public void ParameterSetNameValueDefinesOutputTypeFloat()
        {
            PsCmdletAssert.IsOutputType(typeof(TestPsCmdletBehaviour), typeof(float), TestPsCmdletBehaviour.ParametersSets.VALUE);
        }
            
    }
}
