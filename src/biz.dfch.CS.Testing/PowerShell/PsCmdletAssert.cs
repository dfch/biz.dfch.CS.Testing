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

        public static bool HasAlias(Type implementingType, string alias)
        {
            Contract.Requires(null != implementingType);
            Contract.Requires(!string.IsNullOrWhiteSpace(alias));

            var customAttribute = (AliasAttribute) implementingType.GetCustomAttributes(typeof(AliasAttribute), true).FirstOrDefault();
            if (null == customAttribute || null == customAttribute.AliasNames)
            {
                return false;
            }

            foreach (var customAttributeAliasName in customAttribute.AliasNames)
            {
                if (alias.Equals(customAttributeAliasName))
                {
                    return true;
                }
            }

            return false;
        }
            
        #region CmdletEvaluationHelper - should go into testing library when finished
        public static Type GetOutputType<T>(T cmdlet)
            where T : Cmdlet
        {
            Contract.Requires(null != cmdlet);

            return GetOutputType(typeof(T), default(string));
        }
            
        public static Type GetOutputType<T>(T cmdlet, string parameterSetName)
            where T : Cmdlet
        {
            Contract.Requires(null != cmdlet);

            return GetOutputType(typeof(T), parameterSetName);
        }

        public static Type GetOutputType(Type type)
        {
            Contract.Requires(null != type);
            return GetOutputType(type, default(string));
        }

        public static Type GetOutputType(Type type, string parameterSetName)
        {
            Contract.Requires(null != type);

            var outputTypeAttributes = (OutputTypeAttribute[]) type.GetCustomAttributes(typeof(OutputTypeAttribute), true);

            if (0 >= outputTypeAttributes.Length)
            {
                return default(Type);
            }

            foreach (var outputTypeAttribute in outputTypeAttributes)
            {
                foreach (var psTypeName in outputTypeAttribute.Type)
                {
                    if (string.IsNullOrEmpty(parameterSetName))
                    {
                        return psTypeName.Type;
                    }
                    
                    if (!string.IsNullOrEmpty(parameterSetName) && outputTypeAttribute.ParameterSetName.Contains(parameterSetName))
                    {
                        return psTypeName.Type;
                    }
                }
            }

            return default(Type);
        }

        public IList<object> GetInvocationResults(IEnumerable enumerable)
        {
            Contract.Requires(null != enumerable);
            Contract.Ensures(null != Contract.Result<IList<object>>());

            return enumerable.Cast<object>().ToList();
        }

        public IList<T> GetInvocationResults<T>(IEnumerable<T> enumerable)
            where T : class
        {
            Contract.Requires(null != enumerable);
            Contract.Ensures(null != Contract.Result<IList<T>>());

            return enumerable.ToList();
        }

        #endregion
    }
}
