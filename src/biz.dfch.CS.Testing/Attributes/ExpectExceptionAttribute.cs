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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Testing.Attributes
{
    public class ExpectExceptionAttribute : ExpectedExceptionBaseAttribute
    {
        public Type ExceptionType { get; set; }
        public string MessagePattern { get; set; }

        public ExpectExceptionAttribute()
        {
            // N/A
        }

        public ExpectExceptionAttribute(Type exceptionType, string messagePattern)
        {
            Contract.Requires(null != exceptionType);
            Contract.Requires(null != messagePattern);

            ExceptionType = exceptionType;
            MessagePattern = messagePattern;
        }

        protected override void Verify(Exception exception)
        {
            string message;

            if (null != ExceptionType && exception.GetType().FullName == ExceptionType.FullName)
            {
                if((null == MessagePattern) || Regex.IsMatch(exception.Message, MessagePattern))
                {
                    return;  
                }

                message = string.Format
                (
                    CultureInfo.InvariantCulture,
                    "Test method threw expected exception of type '{0}', but did not match pattern '{1}'. Exception message: {2}",
                    exception.GetType().FullName,
                    MessagePattern,
                    exception.Message
                );
                throw new Exception(message);
            }
            
            base.RethrowIfAssertException(exception);

            message = string.Format
            (
                CultureInfo.InvariantCulture,
                "Test method threw exception of type '{0}', but '{1}' was expected. Exception message: {2}",
                exception.GetType().FullName,
                // ReSharper disable once PossibleNullReferenceException
                ExceptionType.FullName,
                exception.Message
            );
            throw new Exception(message);
        }
    }
}
