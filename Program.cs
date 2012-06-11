/* The MIT License (MIT)
Copyright (c) 2012 Alessandro Cappellozza (alessandro.cappellozza@gmail.com)

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
documentation files (the "Software"), to deal in the Software without restriction, including without limitation
the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of
the Software. THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO
EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,  WHETHER IN
AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
OR OTHER DEALINGS IN THE SOFTWARE.*/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        /* Load configuration file */
        switch (args.Length)
        {
            case 0:
                if (File.Exists("config.xml")) { xmlConfig.Load("config.xml"); Print("LOADING CONFIG"); } else { Print("NO CONFIG FILE"); }
                    break;

            case 1:
                    if (File.Exists(args[0])) { xmlConfig.Load(args[0]); } else { Print("CONFIG FILE DOES NOT EXISTS"); }
                    break;

            default:
                    Print("INVALID ARGUMENTS");
                    break;
        }

        if (xmlConfig.Loaded)
        {
            /* Check mutex presence */
            if (xmlConfig.SingleIstance && Mutex.Exists()) { Print("MUTEX PRESENT"); return; } else { Print("MUTEX ON"); Mutex.Create(); }

            /* Execute */
            Run();

            /* Wait on Exit */
            if (xmlConfig.WaitKeyOnExit) { Print("[PRESS ANY KEY]"); Console.ReadKey(); };

            /* Delete mutex if present */
            if (Mutex.Exists()) { Print("MUTEX OFF"); Mutex.Delete(); }
        }
        else {
            Print("INVALID CONFIG FILE");
        }
    }

    private static void Run(){
        Controller Cntrl = new Controller();
        Print(xmlConfig.Items.Count + " Items and " + xmlConfig.Actions.Count + " Actions");
        foreach (Monitor M in xmlConfig.Items)
        {
            try
            {
                Print("Checking {0} monitor...", false, M.Name);
                Cntrl.Run(M);
            }
            catch (Exception e) {
                Print("Error {0}", false, e.Message);
            }
        }
    }

    public static void Print(string Data, bool WriteLog = false)
    {
        Console.WriteLine(Data);
    }

    public static void Print(string Data, bool WriteLog = false, params object[] arg)
    {
        Console.WriteLine(Data, arg);
    }
}
