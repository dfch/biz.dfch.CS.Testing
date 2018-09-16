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

using biz.dfch.CS.Testing.Attributes;
using biz.dfch.CS.Testing.PowerShell;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Testing.Tests.PowerShell
{
    [TestClass]
    public class PsCmdletAssertContractException2Test
    {
        [TestMethod]
        public void Invoke0ReturnsNull()
        {
            var parameters = "-Id 0;";
            var results = new PsCmdletAssert2().Invoke(typeof(TestContractException), parameters);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            var result = results[0];
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Invoke1Returns1()
        {
            var parameters = "-Id 1;";
            var results = new PsCmdletAssert2().Invoke(typeof(TestContractException), parameters);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            var result = results[0];
            Assert.AreEqual("1", result.BaseObject);
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "Assertion.+42.+Id")]
        public void Invoke42ThrowsContractException()
        {
            var parameters = "-Id 42;";
            var results = new PsCmdletAssert2().Invoke(typeof(TestContractException), parameters, ex => ex);

            Assert.Fail("An exception should have been thrown before!");
        }
    }
}
