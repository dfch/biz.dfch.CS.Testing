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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Testing.Tests.Attributes
{
    [TestClass]
    public class ExpectExceptionAttributeTest
    {
        [TestMethod]
        [ExpectException(typeof(ExpectedException), "expected-value")]
        public void ExpectExceptionSucceeds()
        {
            throw new ExpectedException("expected-value");
        }

        // this test is supposed to fail, therefore we skip it on build server
        [TestMethod]
        [TestCategory("SkipOnTeamCity")]
        [ExpectException(typeof(ExpectedException), "^expected-value")]
        public void ExpectExceptionDoesNotMatchMessagePattern()
        {
            // this test is supposed to fail, therefore we skip it on build server
            throw new ExpectedException("unexpected-value");
        }

        // this test is supposed to fail, therefore we skip it on build server
        [TestMethod]
        [TestCategory("SkipOnTeamCity")]
        [ExpectException(typeof(ExpectedException), "expected-value")]
        public void ExpectExceptionThrowsUnexpectedException()
        {
            // this test is supposed to fail, therefore we skip it on build server
            throw new UnexpectedException("expected-value");
        }
    }

    public class UnexpectedException : ArgumentException
    {
        public UnexpectedException(string message)
            : base(message)
        {
            // N/A
        }
    }

    public class ExpectedException : ArgumentException
    {
        public ExpectedException(string message)
            : base(message)
        {
            // N/A
        }
    }
}
