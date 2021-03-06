﻿/**
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
    public class EnterServerTest
    {
        [TestMethod]
        [ExpectParameterBindingException(MessagePattern = @"'Uri'.+'System\.Uri'")]
        public void InvokeWithoutParametersThrowsParameterBindingValidationExceptionObsolete()
        {
            Func<Exception, Exception> exceptionHandler = exception => exception;
            var parameters = @"-Uri ";
            var results = PsCmdletAssert.Invoke(typeof(EnterServer), parameters, exceptionHandler);
        }

        [TestMethod]
        [ExpectParameterBindingException(MessagePattern = @"'Uri'.+'System\.Uri'")]
        public void InvokeWithoutParametersThrowsParameterBindingValidationException()
        {
            Func<Exception, Exception> exceptionHandler = exception => exception;
            var parameters = @"-Uri ";
            var results = new PsCmdletAssert2().Invoke(typeof(EnterServer), parameters, exceptionHandler);
        }
    }
}
