$resourceGroup = "INH-5946-WRKSHP-D-AZWE-RG-1"
$functionappName = "assignmentsandernederstigt577208"

$cwd = (Get-Location)
$publishDir = "$cwd/Functions/bin/Release/net6.0/publish"
$publishZip = "$cwd/publish.zip"

az deployment group create --mode Incremental --resource-group $resourceGroup --template-file ./template.bicep
dotnet publish -c Release
if (Test-path $publishZip) {Remove-item $publishZip}
Add-Type -assembly "system.io.compression.filesystem"
[io.compression.zipfile]::CreateFromDirectory($publishDir, $publishZip)
az functionapp deployment source config-zip -g $resourceGroup -n $functionappName --src $publishZip