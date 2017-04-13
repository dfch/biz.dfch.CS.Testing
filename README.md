# biz.dfch.CS.Testing
[![Build Status](https://build.dfch.biz/app/rest/builds/buildType:(id:CSharpDotNet_BizDfchCsTestingGit_Build)/statusIcon)](https://build.dfch.biz/project.html?projectId=CSharpDotNet_BizDfchCsTestingGit&tab=projectOverview)
[![License](https://img.shields.io/badge/license-Apache%20License%202.0-blue.svg)](https://github.com/dfch/biz.dfch.CS.Testing/blob/master/LICENSE)
[![Version](https://img.shields.io/nuget/v/biz.dfch.CS.Testing.svg)](https://www.nuget.org/packages/biz.dfch.CS.Testing/)

Assembly: biz.dfch.CS.Testing.dll

d-fens GmbH, General-Guisan-Strasse 6, CH-6300 Zug, Switzerland

## Download

* Get it on [NuGet](https://www.nuget.org/packages/biz.dfch.CS.Testing/)

* See [Releases](https://github.com/dfch/biz.dfch.CS.Testing/releases) on [GitHub](https://github.com/dfch/biz.dfch.CS.Testing)

## Description

This project containts a collection of utility classes to facilitate testing. Formerly this functionality has been included in [biz.dfch.CS.System.Utilities](https://github.com/dfch/biz.dfch.CS.System.Utilities), but due to the dependency on some VisualStudio assemblies this has been extraced and separated.

* ExpectContractFailure attribute to assert Code Contract exceptions

**Telerik JustMock has to be licensed separately. Only the code samples (source code files) are licensed under the Apache 2.0 license. The Telerik JustMock software has to be licensed separately. See the NOTICE file for more information about this.**

## [Release Notes](https://github.com/dfch/biz.dfch.CS.Testing/releases)

See also [Releases](https://github.com/dfch/biz.dfch.CS.Testing/releases) and [Tags](https://github.com/dfch/biz.dfch.CS.Testing/tags)

### 1.5.0 - 20170413
FEATURES

PsCmdletAssert
* Invoke overloads added to allow mocking by passing objects cmdlet parameters

### 1.4.0 - 20161130
FEATURES

PsCmdAssert
- added support for multiple PsCmdlets
- added support for script block initialisation code to be executed before Cmdlet invocation

ExpectException
- added ExpectExceptionAttribute to validate exception messages

### 1.3.0 - 20161115
FEATURES

PowerShell

* added `PsCmdletAssert` class for testing C# based Powershell `PSCmdlet`s.
* added `ExpectParameterBindingValidationExceptionAttribute` to catch `ParameterBindingValidationException`s
* added `ExpectParameterBindingExceptionAttribute` to catch `ParameterBindingException`s
* added `ExpectAssertFailedExceptionAttribute` to catch `AssertFailedException`s
* added `PsCmdletAssert.HasAlias` to assert defined aliases on Cmdlets
* added `PsCmdletAssert.HasOutputType` to assert defined output types on Cmdlets

C#
* changed `ExpectContractFailureAttribute` to support regex based `Message` validation

### 1.2.1 - 20161114

### 1.1.0 - 20161010

### 1.0.0 - 20161007

* Initial release

[![TeamCity Logo](https://github.com/dfch/biz.dfch.CS.Testing/blob/develop/TeamCity.png)](https://www.jetbrains.com/teamcity/)

Built and released with TeamCity
