
$name = "tralala";
$value = [Guid]::NewGuid().Guid;

#Set-Variable -Name $($name) -Value $value -Scope Global; # -ErrorAction:SilentlyContinue;
Set-Variable -Name 'tralala' -Value 'from the scripts' -Scope Global; # -ErrorAction:SilentlyContinue;

Write-Output $value;

return $name;
