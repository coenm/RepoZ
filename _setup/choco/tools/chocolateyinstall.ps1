$ErrorActionPreference = 'Stop';
$toolsDir = Split-Path -parent $MyInvocation.MyCommand.Definition

$packageArgs = @{
  packageName   = $env:ChocolateyPackageName
  unzipLocation = $toolsDir
  fileType      = 'EXE'
  file			= Join-Path $toolsDir 'RepoZ 5.5 Setup.exe'

  softwareName  = 'RepoZ*'

  checksum      = ''
  checksumType  = 'sha256'
  checksum64    = ''
  checksumType64= 'sha256'

  silentArgs    = "/S"
  validExitCodes= @(0, 3010, 1641)
}

Install-ChocolateyInstallPackage @packageArgs