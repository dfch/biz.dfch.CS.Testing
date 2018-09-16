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
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using biz.dfch.CS.Testing.PowerShell;
using biz.dfch.CS.Testing.Tests.PowerShell.PSCmdlets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Testing.Tests.PowerShell
{
    [TestClass]
    public class PsCmdletAssertTest
    {
        public const string SCRIPT_FILE = "PsCmdletAssertTest.ps1";

        [TestMethod]
        public void InvokeWithGlobalScriptBlockAndCmdletCallSucceeds()
        {
            // Arrange
            var fileName = Path.Combine(GetSourceDirectory(), SCRIPT_FILE);
            var fileInfo = new FileInfo(fileName);
            Contract.Assert(fileInfo.Exists, fileInfo.FullName);

            var content = File.ReadAllText(fileInfo.FullName);
            Contract.Assert(!string.IsNullOrWhiteSpace(content), fileInfo.FullName);

            const string PARAMETERS = @"-RequiredStringParameter 'Cmdlet2' -OptionalStringParameter $tralala;";

            // Act
            var results = PsCmdletAssert.Invoke( new [] { typeof(TestCmdlet2), typeof(TestCmdlet1) } , PARAMETERS, scriptDefinition: content);
            
            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results[1].BaseObject.ToString().StartsWith("Cmdlet2"));
            Assert.IsTrue(results[1].BaseObject.ToString().EndsWith(results[0].BaseObject.ToString()));
            
            for (var c = 0; c < results.Count; c++)
            {
                var psObject = results[c];

                var message = string.Format("{0}: '{1}'", c, psObject ?? "<null>");
                System.Diagnostics.Trace.WriteLine(message);
            }
        }

        [TestMethod]
        public void InvokeCmdletWithParametersSucceeds()
        {
            // Arrange
            var requiredStringParameter = "Cmdlet2";
            var optionalStringParameter = "Arbitrary";

            var parameters = new Dictionary<string, object>
            {
                { nameof(TestCmdlet2.RequiredStringParameter), requiredStringParameter },
                { nameof(TestCmdlet2.OptionalStringParameter), optionalStringParameter }
            };

            // Act
            var results = PsCmdletAssert.Invoke(typeof(TestCmdlet2), parameters);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results[0].BaseObject.ToString().StartsWith(requiredStringParameter));
            Assert.IsTrue(results[0].BaseObject.ToString().EndsWith(optionalStringParameter));
        }

        public static string GetSourceDirectory([System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
        {
            Contract.Ensures(null != Contract.Result<string>());

            var sourceFileInfo = new FileInfo(sourceFilePath);
            return sourceFileInfo.DirectoryName;
        }
    }
}
