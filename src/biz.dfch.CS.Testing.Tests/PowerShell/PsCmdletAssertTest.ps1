
$name = "tralala";
$value = [Guid]::NewGuid().Guid;

$null = Get-Command Test-Cmdlet1 -ErrorAction:Stop;
$null = Get-Command Test-Cmdlet2 -ErrorAction:Stop;

$result = Test-Cmdlet1 -RequiredStringParameter $name -OptionalStringParameter $value;

Set-Variable -Name $name -Value $result -Scope Global -ErrorAction:SilentlyContinue;
Write-Output $result;
