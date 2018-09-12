/**
 * Copyright 2014-2016 d-fens GmbH
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
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Testing.Attributes
{
    public class ExpectedContractExceptionAttribute : ExpectedExceptionBaseAttribute
    {
        private const string CONTRACT_EXCEPTION_FULLNAME = "System.Diagnostics.Contracts.__ContractsRuntime+ContractException";
        
        public string MessagePattern { get; set; }

        protected override void Verify(Exception exception)
        {
            string message;

            if (exception.GetType().FullName == CONTRACT_EXCEPTION_FULLNAME)
            {
                if((null == MessagePattern) || Regex.IsMatch(exception.Message, MessagePattern))
                {
                    return;  
                }

                message = string.Format
                (
                    CultureInfo.InvariantCulture,
                    "Test method threw contract exception, but did not match pattern '{0}'. Exception message: {1}",
                    MessagePattern,
                    exception.Message
                );
                throw new Exception(message);
            }
            
            base.RethrowIfAssertException(exception);

            message = string.Format
            (
                CultureInfo.InvariantCulture,
                "Test method threw exception {0}, but contract exception was expected. Exception message: {1}",
                exception.GetType().FullName,
                exception.Message
            );
            throw new Exception(message);
        }
    }
}
