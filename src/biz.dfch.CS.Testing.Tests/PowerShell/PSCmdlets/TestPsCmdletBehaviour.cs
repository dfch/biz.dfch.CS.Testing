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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Management.Automation;

namespace biz.dfch.CS.Testing.Tests.PowerShell.PSCmdlets
{
    [Cmdlet(VerbsDiagnostic.Test, "PsCmdletBehaviour", DefaultParameterSetName = ParametersSets.DEFAULT)]
    [Alias("Test-PsCmdletBehaviourWithAnAlias1", "Test-PsCmdletBehaviourWithAnAlias2")]
    // output type for __AllParameterSets
    [OutputType(typeof(double))]
    [OutputType(typeof(string), ParameterSetName = new string[] { ParametersSets.DEFAULT } )]
    [OutputType(typeof(long), ParameterSetName = new string[] { ParametersSets.VALUE } )]
    // bogus output type defined to create an overlap in parameter sets
    [OutputType(typeof(float), ParameterSetName = new string[] { ParametersSets.DEFAULT, ParametersSets.VALUE } )]
    public class TestPsCmdletBehaviour : PSCmdlet
    {
        public static class ParametersSets
        {
            public const string DEFAULT = "default";
            public const string VALUE = "value";
        }
    
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParametersSets.DEFAULT)]
        [Alias("name")]
        public string RequiredStringParameter { get; set; }

        [Parameter(Mandatory = false, Position = 1)]
        [Alias("description")]
        public string OptionalStringParameter { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParametersSets.VALUE)]
        [Alias("value")]
        public int RequiredIntParameter { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            WriteDebug("myWriteDebug");

            WriteVerbose("myWriteVerbose");

            var informationRecord = new InformationRecord("myInformationRecord", "TestPsCmdletThatDoesNothing");
            WriteInformation(informationRecord);

            WriteWarning("myWriteWarning");

            if (RequiredStringParameter.Equals("invalid-user"))
            {
                var errorRecord = new ErrorRecord(new ArgumentException("Invalid username", RequiredStringParameter), "myErrorId", ErrorCategory.InvalidArgument, RequiredStringParameter);
                WriteError(errorRecord);
            }

            Trace.TraceInformation("BeginProcessing. RequiredStringParameter '{0}'. OptionalStringParameter '{1}'", RequiredStringParameter, OptionalStringParameter);
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if (!ShouldProcess("myTarget"))
            {
                return;
            }

            //if (!ShouldContinue("queryToContinue", "captionToContinue"))
            //{
            //    return;
            //}

            switch (ParameterSetName)
            {
                case ParametersSets.DEFAULT:
                    ProcessDefault();
                    break;
                case ParametersSets.VALUE:
                    ProcessValue();
                    break;
                default:
                    // ReSharper disable once NotResolvedInText
                    throw new ArgumentOutOfRangeException("ParameterSetName", ParameterSetName, "Invalid ParameterSetName");
            }

            Trace.TraceInformation("ProcessRecord. RequiredStringParameter '{0}'. OptionalStringParameter '{1}'", RequiredStringParameter, OptionalStringParameter);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();

            Trace.TraceInformation("EndProcessing. RequiredStringParameter '{0}'. OptionalStringParameter '{1}'", RequiredStringParameter, OptionalStringParameter);
        }

        protected override void StopProcessing()
        {
            base.StopProcessing();

            Trace.TraceInformation("StopProcessing. RequiredStringParameter '{0}'. OptionalStringParameter '{1}'", RequiredStringParameter, OptionalStringParameter);
        }

        internal void ProcessDefault()
        {
            var output = !string.IsNullOrWhiteSpace(OptionalStringParameter) ? 
                string.Format("{0}{1}", RequiredStringParameter, OptionalStringParameter) : 
                RequiredStringParameter;
            WriteObject(output);
        }

        internal void ProcessValue()
        {
            switch (RequiredIntParameter)
            {
                // we return the wrong parameter type (should be long, is string)
                case 8:
                    var stringOutput = RequiredIntParameter.ToString(CultureInfo.InvariantCulture);
                    WriteObject(stringOutput);
                    break;

                // we create an ErrorRecord to test the errorHandler Func
                case 15:
                    // ReSharper disable once NotResolvedInText
                    var errorRecord = new ErrorRecord(new ArgumentException("Invalid int value", "RequiredIntParameter"), "myFullQualifiedErrorId", ErrorCategory.InvalidArgument, RequiredIntParameter);
                    WriteError(errorRecord);
                    break;
                
                // we throw an ArgumentException to test the exceptionHandler Func
                case 42:
                    // ReSharper disable once NotResolvedInText
                    throw new ArgumentException("Invalid int value", "RequiredIntParameter");
                
                // we return the correct parameter type (long)
                default:
                    var output = (long) RequiredIntParameter;
                    WriteObject(output);
                    break;
            }
        }
    }
}
