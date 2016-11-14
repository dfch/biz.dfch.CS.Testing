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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Testing.PowerShell
{
    public class PsCmdletAssert
    {
        private const string POWERSHELL_CMDLET_NAME_FORMATSTRING = "{0}-{1}";
        // it really does not matter which help file name we use so we take this as a default when contructing a CmdletConfigurationEntry
        private const string HELP_FILE_NAME = "Microsoft.Windows.Installer.PowerShell.dll-Help.xml";

        public static IList<PSObject> Invoke(Type implementingType, string parameters)
        {
            Contract.Requires(null != implementingType);
            Contract.Requires(!string.IsNullOrWhiteSpace(parameters));
            Contract.Ensures(null != Contract.Result<IList<PSObject>>());

            return Invoke(implementingType, parameters, HELP_FILE_NAME, null);
        }

        public static IList<PSObject> Invoke(Type implementingType, string parameters, Func<Exception, Exception> exceptionHandler)
        {
            Contract.Requires(null != implementingType);
            Contract.Requires(!string.IsNullOrWhiteSpace(parameters));
            Contract.Ensures(null != Contract.Result<IList<PSObject>>());

            return Invoke(implementingType, parameters, HELP_FILE_NAME, exceptionHandler);
        }

        public static IList<PSObject> Invoke(Type implementingType, string parameters, string helpFileName, Func<Exception, Exception> exceptionHandler)
        {
            Contract.Requires(null != implementingType);
            Contract.Requires(!string.IsNullOrWhiteSpace(parameters));
            Contract.Requires(!string.IsNullOrWhiteSpace(helpFileName));
            Contract.Ensures(null != Contract.Result<IList<PSObject>>());

            // construct the Cmdlet name the type implements
            var cmdletAttribute = (CmdletAttribute) implementingType.GetCustomAttributes(typeof(CmdletAttribute), true).Single();
            Contract.Assert(null != cmdletAttribute);
            var cmdletName = string.Format(POWERSHELL_CMDLET_NAME_FORMATSTRING, cmdletAttribute.VerbName, cmdletAttribute.NounName);

            // add the cmdlet to the runspace
            var runspaceConfiguration = RunspaceConfiguration.Create();
            var cmdletConfigurationEntry = new CmdletConfigurationEntry
            (
                cmdletName
                ,
                implementingType
                ,
                helpFileName
            );
            runspaceConfiguration.Cmdlets.Append(cmdletConfigurationEntry);

            using (var runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration))
            {
                runspace.Open();
                var commandText = string.Format("{0} {1}", cmdletName, parameters);
                using (var pipeline = runspace.CreatePipeline(commandText))
                {
                    try
                    {
                        var invocationResults = pipeline.Invoke();
                        return invocationResults.ToList();
                    }
                    catch (Exception ex)
                    {
                        // process exceptionHandler if an exception was raised
                        if (null != exceptionHandler)
                        {
                            throw exceptionHandler(ex);
                        }

                        // throw original exception if no handler present
                        throw;
                    }
                }
            }
        }

        public static void IsAliasDefined(Type implementingType, string expectedAlias)
        {
            Contract.Requires(null != implementingType);
            Contract.Requires(!string.IsNullOrWhiteSpace(expectedAlias));

            IsAliasDefined(implementingType, expectedAlias, null);
        }

        public static void IsAliasDefined(Type implementingType, string expectedAlias, string message)
        {
            Contract.Requires(null != implementingType);
            Contract.Requires(!string.IsNullOrWhiteSpace(expectedAlias));

            var customAttribute = (AliasAttribute) implementingType.GetCustomAttributes(typeof(AliasAttribute), true).FirstOrDefault();
            var isAttributeDefined = null != customAttribute && null != customAttribute.AliasNames;
            if (!isAttributeDefined)
            {
                var attributeNotDefinedMessage = new StringBuilder();
                attributeNotDefinedMessage.AppendFormat("PsCmdletAssert.IsAliasDefined FAILED. No AliasAttribute defined.");
                if (null != message)
                {
                    attributeNotDefinedMessage.AppendFormat(" '{0}'", message);
                }
                throw new AssertFailedException(attributeNotDefinedMessage.ToString());
            }

            //foreach (var customAttributeAliasName in customAttribute.AliasNames)
            //{
            //    if (alias.Equals(customAttributeAliasName))
            //    {
            //        // ... 
            //    }
            //}

            var isAliasDefined = customAttribute.AliasNames.Any(expectedAlias.Equals);
            if (isAliasDefined)
            {
                return;
            }

            var aliasNotDefinedMessage = new StringBuilder();
            aliasNotDefinedMessage.AppendFormat("PsCmdletAssert.IsAliasDefined FAILED. ExpectedAlias '{0}' not defined.", expectedAlias);
            if (null != message)
            {
                aliasNotDefinedMessage.AppendFormat(" '{0}'", message);
            }
            throw new AssertFailedException(aliasNotDefinedMessage.ToString());
        }

        public static void IsOutputType(Type implementingType, Type expectedOutputType)
        {
            Contract.Requires(null != implementingType);
            Contract.Requires(null != expectedOutputType);

            IsOutputType(implementingType, expectedOutputType.FullName, ParameterAttribute.AllParameterSets, null);
        }

        public static void IsOutputType(Type implementingType, string expectedOutputTypeName)
        {
            Contract.Requires(null != implementingType);
            Contract.Requires(!string.IsNullOrWhiteSpace(expectedOutputTypeName));

            IsOutputType(implementingType, expectedOutputTypeName, ParameterAttribute.AllParameterSets, null);
        }

        public static void IsOutputType(Type implementingType, Type expectedOutputType, string parameterSetName)
        {
            Contract.Requires(null != implementingType);
            Contract.Requires(null != expectedOutputType);

            IsOutputType(implementingType, expectedOutputType.FullName, parameterSetName, null);
        }

        public static void IsOutputType(Type implementingType, string expectedOutputTypeName, string parameterSetName)
        {
            Contract.Requires(null != implementingType);
            Contract.Requires(!string.IsNullOrWhiteSpace(expectedOutputTypeName));

            IsOutputType(implementingType, expectedOutputTypeName, parameterSetName, null);
        }

        public static void IsOutputType(Type implementingType, string expectedOutputTypeName, string parameterSetName, string message)
        {
            Contract.Requires(null != implementingType);
            Contract.Requires(!string.IsNullOrWhiteSpace(expectedOutputTypeName));
            Contract.Requires(!string.IsNullOrWhiteSpace(parameterSetName));

            var outputTypeAttributes = (OutputTypeAttribute[]) implementingType.GetCustomAttributes(typeof(OutputTypeAttribute), true);

            var isValidOutputType = false;
            
            var outputTypeAttributesForGivenParameterSetName = outputTypeAttributes.Where(e => e.ParameterSetName.Contains(parameterSetName));
            foreach (var outputTypeAttribute in outputTypeAttributesForGivenParameterSetName)
            {
                isValidOutputType |= outputTypeAttribute.Type.Any(e => e.Name == expectedOutputTypeName);
            }
            
            if (isValidOutputType)
            {
                return;
            }

            var outputTypeAttributesForAllParameterSets = outputTypeAttributes.Where(e => e.ParameterSetName.Contains(ParameterAttribute.AllParameterSets));
            foreach (var outputTypeAttribute in outputTypeAttributesForAllParameterSets)
            {
                isValidOutputType |= outputTypeAttribute.Type.Any(e => e.Name == expectedOutputTypeName);
            }

            if (isValidOutputType)
            {
                return;
            }

            var invalidOutputTypeMessage = new StringBuilder();
            invalidOutputTypeMessage.AppendFormat("PsCmdletAssert.IsOutputType FAILED. ExpectedType '{0}' not defined for ParameterSetName '{1}'.", expectedOutputTypeName, parameterSetName);
            if (null != message)
            {
                invalidOutputTypeMessage.AppendFormat(" '{0}'", message);
            }
            throw new AssertFailedException(invalidOutputTypeMessage.ToString());
        }

    }
}
