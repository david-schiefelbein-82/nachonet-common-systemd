param ($version)

function Add-Version {
    param ( [string] $text, [int] $value = 1 )

    $m = [Regex]::Match($text, "^([0-9]+)\.([0-9]+)\.([0-9]+)\.([0-9]+)$")
    $v1 = [int]$($m.Groups[1].Value)
    $v2 = [int]$($m.Groups[2].Value)
    $v3 = [int]$($m.Groups[3].Value)
    $v4 = [int]$($m.Groups[4].Value)

    $v4 += $value

  return "$v1.$v2.$v3.$v4"
}

$fileName = "version.txt"

$content = [System.IO.File]::ReadAllText($fileName)

if ([System.String]::IsNullOrWhiteSpace($version)) {
    Write-Host "Version not supplied" -ForegroundColor Red
    return
}



$oldVersion = $content
    

if ($version -eq "increment") {
    $version = Add-Version -text $oldVersion -value 1
}
  
write-host "updated version from $oldVersion to $version"
$text = "@set NACHO_SIP_VER=$version"
[System.IO.File]::WriteAllText($fileName, $version)

