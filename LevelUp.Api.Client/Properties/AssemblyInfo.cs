#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="AssemblyInfo.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2016 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
// </copyright>
// <license publisher="Apache Software Foundation" date="January 2004" version="2.0">
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
//   in compliance with the License. You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software distributed under the License
//   is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//   or implied. See the License for the specific language governing permissions and limitations under
//   the License.
// </license>
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#endregion

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("LevelUp.Api.Client")]
[assembly: AssemblyDescription("A library for interacting with the LevelUp API")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("SCVNGR Inc.")]
[assembly: AssemblyProduct("LevelUp.Api.Client")]
[assembly: AssemblyCopyright("Copyright © LevelUp 2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("13aca667-f878-4698-a242-27ee657ceda2")]

// Important
// Assembly*Version attributes were removed in lieu of GitVersion and GitVersionTask to handle versioning.
// When MSBuild is run, GitVersionTask will substitute GitVersion values for the assembly version fields. 
//
// AssemblyVersion will be set to the AssemblySemVer variable.
// AssemblyFileVersion will be set to the MajorMinorPatch variable with .0 appended to it.
// AssemblyInformationalVersion will be set to the InformationalVersion variable.
//
// See https://gitversion.readthedocs.io/en/latest/usage/msbuild-task/ for more information.

// make test assembly a friend
[assembly: InternalsVisibleTo("LevelUp.Api.Client.Test")]
