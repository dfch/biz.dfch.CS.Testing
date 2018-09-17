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

using System.Collections.Generic;
using biz.dfch.CS.Testing.Attributes;
using biz.dfch.CS.Testing.PowerShell;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Testing.Tests.PowerShell.PSCmdlets
{
    [TestClass]
    public class TestCmdlet5Test
    {
        [TestMethod]
        [ExpectParameterBindingException(MessagePattern = @"RequiredComplexParameter")]
        public void InvokeWithoutParametersThrowsParameterBindingException()
        {
            var parameters = @";";
            var results = new PsCmdletAssert2().Invoke(typeof(TestCmdlet5), parameters);
        }

        [TestMethod]
        public void InvokeWithRequiredComplexParameterSucceeds()
        {
            var param1 = "tralala";
            var param2 = 42;
            var parameters = new Dictionary<string, object>
            {
                {
                    nameof(TestCmdlet5.RequiredComplexParameter), new ArbitraryClass
                        {
                            ArbitraryStringParameter = param1
                            ,
                            ArbitraryIntParameter = param2
                        }
                }
            };
            var results = new PsCmdletAssert2().Invoke(typeof(TestCmdlet5), parameters);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            var result = results[0].ToString();
            Assert.IsTrue(result.StartsWith(param1));
            Assert.IsTrue(result.EndsWith(param2.ToString()));
        }
    }
}
