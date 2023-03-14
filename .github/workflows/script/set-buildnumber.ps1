param(
    [int]$major,
    [int]$minor,
    [int]$patch,
    [string]$tag,
    [string]$ref
)

$version = Get-Content "$PSScriptRoot\..\.version"
if (!$major) {
    $major = $version.Split(".")[0]
}
if (!$minor) {
    $minor = $version.Split(".")[1]
}
if (!$patch) {
    $patch = $version.Split(".")[2]
}

$ProductMajorVersion = $major
$ProductMinorVersion = $minor
$ProductpatchVersion = $patch
$MyCustomBuildVersion = "$ProductMajorVersion.$ProductMinorVersion.$ProductpatchVersion"

if ($ref) {
    $tag = $ref.Split("/")[-1].Replace("_","-")
}
if ($tag -and $tag -ne "master") {
    $BaselineYear = 2023
    $CurrentDate = (Get-Date).ToUniversalTime()
    $StartOfDay = Get-Date -Date $CurrentDate -Hour 0 -Minute 00 -Second 00
    $BuildMajorVersion = ($CurrentDate.Year - $BaselineYear) * 12 + $CurrentDate.Month
    $BuildMajorVersion = $BuildMajorVersion * 31 + $CurrentDate.Day
    $BuildMinorVersion = [math]::floor(((New-TimeSpan -Start $StartOfDay -End $CurrentDate).TotalSeconds) / 2)
    $MyCustomBuildVersion = "$MyCustomBuildVersion-$tag.$BuildMajorVersion.$BuildMinorVersion"
}
$env:BUILD_BUILDNUMBER = $MyCustomBuildVersion
[Environment]::SetEnvironmentVariable("BUILD_BUILDNUMBER", $MyCustomBuildVersion, "Machine")
echo "BUILD_BUILDNUMBER=$MyCustomBuildVersion" >> $env:GITHUB_OUTPUT
echo "BUILD_BUILDNUMBER=$MyCustomBuildVersion" >> $env:GITHUB_ENV
Write-Host "Setting the value of current build version :  $MyCustomBuildVersion"

dir env: