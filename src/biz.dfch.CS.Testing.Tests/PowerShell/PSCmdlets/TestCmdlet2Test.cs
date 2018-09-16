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
    [Obsolete("Use TestCmdlet3Test instead.")]
    [TestClass]
    public class TestCmdlet2Test
    {
        [TestMethod]
        [ExpectParameterBindingException(MessagePattern = @"RequiredStringParameter")]
        public void InvokeWithoutParametersThrowsParameterBindingException()
        {
            var parameters = @";";
            var results = PsCmdletAssert.Invoke(typeof(TestCmdlet2), parameters);
        }

        [TestMethod]
        public void InvokeWithRequiredStringParameterSucceeds()
        {
            var param1 = "arbitrary-value1";
            var parameters = string.Format(@"-RequiredStringParameter '{0}'", param1);
            var results = PsCmdletAssert.Invoke(typeof(TestCmdlet2), parameters);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            var result = results[0].ToString();
            Assert.AreEqual(param1, result);
        }

        [TestMethod]
        public void InvokeWithRequiredStringParameterAndOptionalStringParameterSucceeds()
        {
            var param1 = "arbitrary-value1";
            var param2 = "arbitrary-value2";
            var parameters = string.Format(@"-RequiredStringParameter '{0}' -OptionalStringParameter '{1}'", param1, param2);
            var results = PsCmdletAssert.Invoke(typeof(TestCmdlet2), parameters);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            var result = results[0].ToString();
            Assert.AreEqual(string.Format("{0}-{1}", param1, param2), result);
        }
    }
}
