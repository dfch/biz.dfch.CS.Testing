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
using System.Diagnostics.Contracts;
using System.IO;
using System.Management.Automation;
using System.Reflection;
using biz.dfch.CS.Testing.PowerShell;
using biz.dfch.CS.Testing.Tests.PowerShell.PSCmdlets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Testing.Tests.PowerShell
{
    [TestClass]
    public class PsCmdletAssertTest
    {
        public const string SCRIPT_PATH = "PowerShell";
        public const string SCRIPT_FILE = "script1.ps1";

        [TestMethod]
        [TestCategory("SkipOnTeamCity")]
        public void InvokeCmdletWithGlobalScriptBlockSucceeds()
        {
            var fileName = Path.Combine(AssemblyDirectory, SCRIPT_PATH, SCRIPT_FILE);
            var fileInfo = new FileInfo(fileName);
            Contract.Assert(fileInfo.Exists, fileInfo.FullName);

            var content = File.ReadAllText(fileInfo.FullName);
            Contract.Assert(!string.IsNullOrWhiteSpace(content), fileInfo.FullName);

            PsCmdletAssert.ScriptDefinition = content;
            var parameters = @"-RequiredStringParameter 'someValue';";
            
            var results = PsCmdletAssert.Invoke(typeof(TestCmdlet2), parameters);
            Assert.IsNotNull(results);
            for (var c = 0; c < results.Count; c++)
            {
                var psObject = results[c];

                var message = string.Format("{0}: '{1}'", c, psObject ?? "<null>");
                System.Diagnostics.Trace.WriteLine(message);
            }
        }

        [TestMethod]
        public void Test2()
        {
            var message = string.Format("CallerFilePath '{0}'", GetCallerFilePathAttribute());
            Contract.Assert(false, message);
        }

        [TestMethod]
        public void Test4()
        {
            var message = string.Format("AssemblyDirectory '{0}'", AssemblyDirectory);
            Contract.Assert(false, message);
        }

        [TestMethod]
        public void Test5()
        {
            var sourceFileInfo = new FileInfo(GetCallerFilePathAttribute());
            Contract.Assert(sourceFileInfo.Exists, sourceFileInfo.FullName);
        }

        [TestMethod]
        public void Test6()
        {
            var sourceFileInfo = new FileInfo(GetCallerFilePathAttribute());
            Contract.Assert(null != sourceFileInfo.DirectoryName);
            var fileName = Path.Combine(sourceFileInfo.DirectoryName, SCRIPT_FILE);
            var fileInfo = new FileInfo(fileName);
            Contract.Assert(fileInfo.Exists, fileInfo.FullName);
        }

        public string GetCallerFilePathAttribute([System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
        {
            return sourceFilePath;
        }

        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }

}
