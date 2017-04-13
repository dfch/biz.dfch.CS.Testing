/**
 * Copyright 2015-2016 d-fens GmbH
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Testing.Tests.Attributes
{
    [TestClass]
    public class ExpectContractFailureAttributeTest
    {
        [TestMethod]
        public void RunningTestWithTrueSucceeds()
        {
            var sut = new CodeContractsTest();

            var result = sut.CallingMeWithTrueReturns42ThrowsContractExceptionOtherwise(true);

            Assert.AreEqual(42, result);
        }

        [TestMethod]
        [ExpectContractFailure]
        public void RunningTestWithFalseThrowsCodeContractException()
        {
            var sut = new CodeContractsTest();

            var result = sut.CallingMeWithTrueReturns42ThrowsContractExceptionOtherwise(false);

            Assert.AreEqual(42, result);
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "Pre.*itMustBeTrue")]
        public void RunningTestWithFalseThrowsCodeContractExceptionWithExpectedMessage()
        {
            var sut = new CodeContractsTest();

            var result = sut.CallingMeWithTrueReturns42ThrowsContractExceptionOtherwise(false);

            Assert.AreEqual(42, result);
        }

        // this test is supposed to fail, therefore we skip it on build server
        [TestCategory("SkipOnTeamCity")]
        [TestMethod]
        [ExpectContractFailure(MessagePattern = "invalid-pattern")]
        public void RunningTestWithFalseThrowsCodeContractExceptionWithInvalidPattern()
        {
            var sut = new CodeContractsTest();

            // this test is supposed to fail, therefore we skip it on build server
            var result = sut.CallingMeWithTrueReturns42ThrowsContractExceptionOtherwise(false);
            Assert.AreEqual(42, result);
        }
    }
}
