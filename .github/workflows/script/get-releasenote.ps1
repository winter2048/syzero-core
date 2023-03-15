param(
    [string]$version
)

$releaseNotes = Get-Content "$PSScriptRoot\..\..\..\ReleaseNotes.md" -Encoding utf8

$isAdd = $false
$node = ""
foreach ($row in $releaseNotes) {
    if ($row.StartsWith("###")) {
        $rowVersion = $row.split("v")[1]
        if ($rowVersion -eq $version) {
            $isAdd = $true
        }
    }

    if ($isAdd -and $row.StartsWith("---")) {
        $isAdd = $false
    }

    if ($isAdd) {
        $node += "`r`n$row"
    }
}
Write-Host $node
echo "releasenote=$node" >> $env:GITHUB_OUTPUT