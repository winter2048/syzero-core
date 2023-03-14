param(
[string]$source,
[string]$token
)

Write-Host "Push to $source"

$nupkgs = Get-ChildItem "$PSScriptRoot..\..\..\..\nuget\*.nupkg"
foreach($file in $nupkgs)
{
    Write-Host "$($file.FullName):Begin Push"
    dotnet nuget push "$($file.FullName)" --api-key "$token" --source "$source" --skip-duplicate
    Write-Host "$($file.FullName):End Push"
}