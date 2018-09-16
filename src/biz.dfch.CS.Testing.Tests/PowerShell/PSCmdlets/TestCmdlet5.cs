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

using System.Management.Automation;

namespace biz.dfch.CS.Testing.Tests.PowerShell.PSCmdlets
{
    [Cmdlet(
         VerbsDiagnostic.Test, "Cmdlet5"
         ,
         ConfirmImpact = ConfirmImpact.Low
         ,
         DefaultParameterSetName = ParameterSets.DEFAULT
         ,
         SupportsShouldProcess = true
         ,
         HelpUri = "http://dfch.biz/biz/dfch/PS/Testing/Tests/Test-Cmdlet5/"
    )]
    [OutputType(typeof(string))]
    public class TestCmdlet5 : PSCmdlet
    {
        public static class ParameterSets
        {
            public const string DEFAULT = "default";
        }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSets.DEFAULT)]
        public ArbitraryClass RequiredComplexParameter { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if (!ShouldProcess(RequiredComplexParameter.ToString()))
            {
                return;
            }

            var output = $"{RequiredComplexParameter.ArbitraryStringParameter}-{RequiredComplexParameter.ArbitraryIntParameter}";
            WriteObject(output);
        }
    }
}
