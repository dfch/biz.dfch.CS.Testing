
$name = "tralala";
$value = [Guid]::NewGuid().Guid;

$null = Get-Command Test-Cmdlet4 -ErrorAction:Stop;
$null = Get-Command Test-Cmdlet3 -ErrorAction:Stop;

$result = Test-Cmdlet4 -RequiredStringParameter $name -OptionalStringParameter $value;

Set-Variable -Name $name -Value $result -Scope Global -ErrorAction:SilentlyContinue;
Write-Output $result;
