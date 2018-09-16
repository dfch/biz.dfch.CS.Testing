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
using System.Collections.Generic;
using System.Management.Automation;
using biz.dfch.CS.Testing.Attributes;
using biz.dfch.CS.Testing.PowerShell;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Testing.Tests.PowerShell.PSCmdlets
{
    [Obsolete("Use TestPsCmdletBehaviour2Test instead.")]
    [TestClass]
    public class TestPsCmdletBehaviourTest
    {
        [TestMethod]
        [ExpectParameterBindingValidationException(MessagePattern = "RequiredStringParameter")]
        public void InvokeWithEmptyStringPropertyValueThrowsParameterBindingValidationException()
        {
            var parameters = @"-RequiredStringParameter ''";
            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), parameters);
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
            var value = "arbitrary-RequiredStringParameter-value";
            var parameters = string.Format(@"-RequiredStringParameter '{0}'", value);
            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), parameters);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            var result = results[0].ToString();
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void InvokeWithValidStringPropertyValueWithAliasSucceeds()
        {
            var value = "arbitrary-RequiredStringParameter-value";
            var parameters = string.Format(@"-name '{0}'", value);
            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), parameters);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            var result = results[0].ToString();
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void TestAliasesSucceeds()
        {
            PsCmdletAssert.HasAlias(typeof(TestPsCmdletBehaviour), "Test-PsCmdletBehaviourWithAnAlias1");
            PsCmdletAssert.HasAlias(typeof(TestPsCmdletBehaviour), "Test-PsCmdletBehaviourWithAnAlias2");
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void TestInexistentAliasThrowsAssertFailedException()
        {
            PsCmdletAssert.HasAlias(typeof(TestPsCmdletBehaviour), "Test-PsCmdletBehaviourWithAnAlias3");
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void TestInexistentAliasThrowsAssertFailedExceptionWithMessage()
        {
            var alias = "Test-PsCmdletBehaviourWithAnAlias4";
            var message = "Alias does not exist or is not defined.";
            try
            {
                PsCmdletAssert.HasAlias(typeof(TestPsCmdletBehaviour), alias, message);
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
            var value = "arbitrary-RequiredStringParameter-value";
            var parameters = string.Format(@"-RequiredStringParameter '{0}'", value);

            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), parameters);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);

            var result = results[0].BaseObject;
            Assert.IsTrue(result is string);
            Assert.AreEqual(value, result);

            PsCmdletAssert.HasOutputType(typeof(TestPsCmdletBehaviour), result.GetType(), TestPsCmdletBehaviour.ParametersSets.DEFAULT);
        }

        [TestMethod]
        public void ParameterSetNameDefaultDefinesOutputTypeString()
        {
            PsCmdletAssert.HasOutputType(typeof(TestPsCmdletBehaviour), typeof(string), TestPsCmdletBehaviour.ParametersSets.DEFAULT);
        }

        [TestMethod]
        public void ParameterSetNameDefaultDefinesOutputTypeFloat()
        {
            PsCmdletAssert.HasOutputType(typeof(TestPsCmdletBehaviour), typeof(float), TestPsCmdletBehaviour.ParametersSets.DEFAULT);
        }

        [TestMethod]
        public void ParameterSetNameDefaultDefinesOutputTypeDouble()
        {
            PsCmdletAssert.HasOutputType(typeof(TestPsCmdletBehaviour), typeof(double), TestPsCmdletBehaviour.ParametersSets.DEFAULT);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void ParameterSetNameDefaultDefinesThrowsAssertionFailedException()
        {
            try
            {
                PsCmdletAssert.HasOutputType(typeof(TestPsCmdletBehaviour), typeof(int), TestPsCmdletBehaviour.ParametersSets.DEFAULT);
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
            PsCmdletAssert.HasOutputType(typeof(TestPsCmdletBehaviour), typeof(double));
        }
            
        [TestMethod]
        public void ParameterSetNameValueDefinesOutputTypeLong()
        {
            PsCmdletAssert.HasOutputType(typeof(TestPsCmdletBehaviour), typeof(long), TestPsCmdletBehaviour.ParametersSets.VALUE);
        }
            
        [TestMethod]
        public void ParameterSetNameValueDefinesOutputTypeFloat()
        {
            PsCmdletAssert.HasOutputType(typeof(TestPsCmdletBehaviour), typeof(float), TestPsCmdletBehaviour.ParametersSets.VALUE);
        }
            
        [TestMethod]
        [ExpectParameterBindingValidationException(MessagePattern = "RequiredIntParameter")]
        public void InvokeWithValue0ThrowsParameterBindingException()
        {
            var value = 0;
            var parameters = string.Format(@"-RequiredIntParameter {0}", value);

            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), parameters);
        }
            
        [TestMethod]
        public void InvokeWithValue1ReturnsInt()
        {
            var value = 1;
            var parameters = string.Format(@"-RequiredIntParameter {0}", value);

            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), parameters);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);

            var result = results[0].BaseObject;
            Assert.IsTrue(result is long);
            Assert.AreEqual((long) value * 2, result);

            PsCmdletAssert.HasOutputType(typeof(TestPsCmdletBehaviour), result.GetType(), TestPsCmdletBehaviour.ParametersSets.VALUE);
        }
    
        [TestMethod]
        [ExpectAssertFailedException(@"PsCmdletAssert.IsOutputType FAILED. ExpectedType 'System.String' not defined for ParameterSetName 'value'")]
        public void InvokeWithValue8ReturnsString()
        {
            var value = 8;
            var parameters = string.Format(@"-RequiredIntParameter {0}", value);

            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), parameters);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);

            var result = results[0].BaseObject;
            Assert.IsTrue(result is string);
            Assert.AreEqual(value.ToString(), result);

            PsCmdletAssert.HasOutputType(typeof(TestPsCmdletBehaviour), result.GetType(), TestPsCmdletBehaviour.ParametersSets.VALUE);
        }
    
        [TestMethod]
        public void InvokeWithValue15ReturnsLong()
        {
            var value = 15;
            var parameters = string.Format(@"-RequiredIntParameter {0}", value);

            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), parameters);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);

            var result = results[0].BaseObject;
            Assert.IsTrue(result is long);
            Assert.AreEqual((long) value * 2, result);

            PsCmdletAssert.HasOutputType(typeof(TestPsCmdletBehaviour), result.GetType(), TestPsCmdletBehaviour.ParametersSets.VALUE);
        }
    
        [TestMethod]
        public void InvokeWithValue15ReturnsLongAndGeneratesErrorRecord()
        {
            var value = 15;
            var parameters = string.Format(@"-RequiredIntParameter {0}", value);

            Action<IList<ErrorRecord>> errorHandler = errorRecords =>
            {
                Assert.AreEqual(1, errorRecords.Count);
                var errorRecord = errorRecords[0];
                Assert.AreEqual(value, (int) errorRecord.TargetObject);
                Assert.IsInstanceOfType(errorRecord.Exception, typeof(ArgumentException));
                
                return;
            };
            
            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), parameters, null, errorHandler);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);

            var result = results[0].BaseObject;
            Assert.IsTrue(result is long);
            Assert.AreEqual((long) value * 2, result);

            PsCmdletAssert.HasOutputType(typeof(TestPsCmdletBehaviour), result.GetType(), TestPsCmdletBehaviour.ParametersSets.VALUE);
        }
    
        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void InvokeWithValue42ThrowsCmdletInvocationException()
        {
            var value = 42;
            var parameters = string.Format(@"-RequiredIntParameter {0}", value);

            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), parameters);
        }
    
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvokeWithValue42ThrowsArgumentException()
        {
            var value = 42;
            var parameters = string.Format(@"-RequiredIntParameter {0}", value);

            Func<Exception, Exception> exceptionHandler = exceptionThrown =>
            {
                Assert.IsInstanceOfType(exceptionThrown, typeof(ArgumentException));
                Assert.IsTrue(exceptionThrown.Message.Contains("RequiredIntParameter"));
                Assert.IsTrue(exceptionThrown.Message.Contains("Invalid int value"));

                return exceptionThrown;
            };
            
            var results = PsCmdletAssert.Invoke(typeof(TestPsCmdletBehaviour), parameters, exceptionHandler);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);

            var result = results[0].BaseObject;
            Assert.IsTrue(result is long);
            Assert.AreEqual((long) value * 2, result);

            PsCmdletAssert.HasOutputType(typeof(TestPsCmdletBehaviour), result.GetType(), TestPsCmdletBehaviour.ParametersSets.VALUE);
        }
    
    }
}
