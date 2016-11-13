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
        public void InvokeWithEmptyStringPropertyValue()
        {
            var commandText = @"-RequiredStringParameter ''";
            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), commandText);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        [ExpectParameterBindingValidationException]
        public void InvokeWithNullStringPropertyValue()
        {
            var parameters = @"-RequiredStringParameter $null";
            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), parameters);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void InvokeWithValidStringPropertyValue()
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
        public void InvokeWithValidStringPropertyValueWithAlias()
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
        public void TestAliases()
        {
            Assert.IsTrue(PsCmdletAssert.HasAlias(typeof(TestPsCmdletBehaviour), "Test-PsCmdletBehaviourWithAnAlias1"));
            Assert.IsTrue(PsCmdletAssert.HasAlias(typeof(TestPsCmdletBehaviour), "Test-PsCmdletBehaviourWithAnAlias2"));
            Assert.IsFalse(PsCmdletAssert.HasAlias(typeof(TestPsCmdletBehaviour), "Test-PsCmdletBehaviourWithAnAlias3"));
        }
    }
}
