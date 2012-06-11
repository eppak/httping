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

class Controller
{
    public bool Runnable(Monitor Item) {
        if (Item.Schedule == "*") { return true; } else { return false;}
    }

    public void Run(Monitor Item) {
        if (Runnable(Item)) {
            switch (Item.Type.ToLower())
            {
                case "http":
                case "http-get":
                    MonitorResult Res = HTTP.GETPage(Item.Target);
                    if (!CheckCondition(Item.Condition, Res.Data) || !Res.Success) {
                        ExecAlarm(Item.Alarm, Res);
                    }
                    else { Program.Print("Item OK"); }
                    break;

                case "http-post":
                    break;

                default:
                    throw new Exception("Invalid monitor type.");
            }
        }
    }

    private bool CheckCondition(string Cond, string Val) {
        return true;
    }

    private void ExecAlarm(string AlarmList, MonitorResult Res)
    {
        foreach (string A in AlarmList.Split(',')) {
            Program.Print("Alarm {0}", false, A);
            Action Act = xmlConfig.Actions[A.ToLower().Trim()];

            switch (Act.Type.ToLower()) { 
                case "email":
                    break;

                case "post":
                case "http-post":
                    break;

                default:
                    throw new Exception("Invalid action type.");
            }
        }
    }
}

