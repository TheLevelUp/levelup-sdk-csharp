param($target)

$companyName = "SCVNGR, Inc. d/b/a LevelUp"
$headerBars = "//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++"

$header = @"
{2}
// <copyright file="{0}" company="{1}">
//   Copyright(c) 2014 {1}. All rights reserved.
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
{2}`r`n
"@

function Write-Header($file) {
    
    $filename = Split-Path -Leaf $file
    Write-Host "Adding header to " $fileName
    
    if($filename.ToLower().EndsWith(".cs")) {
        $content = Get-Content $file
        $fileHeader = $header -f $filename,$companyName,$headerBars

        # if the file already has a header, trim it off.
        if($content.Contains($headerBars)) {    
            $newLine = "`r`n"
            $stringContent = $content -join $newLine  
            $lastIndex = $stringContent.LastIndexOf($headerBars)    
            $stringContent = $stringContent.Substring($lastIndex + $headerBars.Length + 2*$newLine.Length)
            Set-Content $file $fileHeader
            Add-Content $file $stringContent
        }
        else {
            Set-Content $file $fileHeader
            Add-Content $file $content
        }        
    }    
}

Get-ChildItem $target -Recurse | ? { $_.Extension -like ".cs" } | % {
    $temp = $_.PSPath
    $temp1 = $temp.Split(":", 3)
    $temp3 = $temp1[2]
    Write-Header $_.PSPath.Split(":", 3)[2]
}