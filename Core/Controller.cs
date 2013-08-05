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
using System.Text.RegularExpressions;
using httping.Protocols;

class Controller
{
    public bool Runnable(Monitor Item) {
        if (Item.Schedule == "*") { return true; } else { return false;}
    }

    public void Run(Monitor Item) {
        if (Runnable(Item)) {
            switch (Item.Type.ToLower())
            {
                case "http": case "http-get":
                    Program.Print("Monitor [{0}]...", false, Item.Name);
                    MonitorResult Res = HTTP.GET(System.Web.HttpUtility.UrlDecode(Item.Target));
                    if (!CheckCondition(Item.Condition, Res.Data) || !Res.Success) { ExecAlarm(Item.Alarm, Res);}
                        else { Program.Print("NOP"); }
                    break;

                case "http-post":
                    break;

                default:
                    throw new Exception("Invalid monitor type.");
            }
        }
    }

    private bool CheckCondition(string Cond, string Val) {
        // Numeric
        if (Regex.Match(Cond, @"^gte(\s{0,3}):*", RegexOptions.IgnoreCase).Success) {
            return Convert.ToInt32(Regex.Replace(Cond,  @"^gte(\s{0,3}):*", "", RegexOptions.IgnoreCase)) >= Convert.ToInt32(Val);
        };

        if (Regex.Match(Cond, @"^gt(\s{0,3}):*", RegexOptions.IgnoreCase).Success)
        {
            return Convert.ToInt32(Regex.Replace(Cond, @"^gt(\s{0,3}):*", "", RegexOptions.IgnoreCase)) > Convert.ToInt32(Val);
        };

        if (Regex.Match(Cond, @"^lte(\s{0,3}):*", RegexOptions.IgnoreCase).Success)
        {
            return Convert.ToInt32(Regex.Replace(Cond, @"^lte(\s{0,3}):*", "", RegexOptions.IgnoreCase)) <= Convert.ToInt32(Val);
        };

        if (Regex.Match(Cond, @"^lt(\s{0,3}):*", RegexOptions.IgnoreCase).Success) {
            return Convert.ToInt32(Regex.Replace(Cond, @"^lt(\s{0,3}):*", "", RegexOptions.IgnoreCase)) < Convert.ToInt32(Val);
        };

        // String
        if (Regex.Match(Cond, @"^eq(\s{0,3}):*", RegexOptions.IgnoreCase).Success) {
            return Regex.Replace(Cond,  @"^eq(\s{0,3}):*", "", RegexOptions.IgnoreCase) == Val;
        };

        if (Regex.Match(Cond, @"^\!eq(\s{0,3}):*", RegexOptions.IgnoreCase).Success)
        {
            return Regex.Replace(Cond, @"^\!eq(\s{0,3}):*", "", RegexOptions.IgnoreCase) != Val;
        };

        if (Regex.Match(Cond, @"^cont(\s{0,3}):*", RegexOptions.IgnoreCase).Success) {
            return Regex.Replace(Cond, @"^cont(\s{0,3}):*", "", RegexOptions.IgnoreCase).IndexOf(Val) > 0;
        };

        if (Regex.Match(Cond, @"^\!cont(\s{0,3}):*", RegexOptions.IgnoreCase).Success) {
            return !(Regex.Replace(Cond, @"^\!cont(\s{0,3}):*", "", RegexOptions.IgnoreCase).IndexOf(Val) > 0);
        };

        throw new Exception("INVALID CONDITION");
    }

    private void ExecAlarm(string AlarmList, MonitorResult Res)
    {
        foreach (string A in AlarmList.Split(',')) {
            Program.Print("RUN [{0}]", true, A);
            Action Act = xmlConfig.Actions[A.ToLower().Trim()];

            switch (Act.Type.ToLower()) { 
                case "email":
                    SMTP.send(Act.From, Act.To, compileMonitor(Res, Act.Subject), compileMonitor(Res, Act.Message));

                    break;

                case "http": case "http-get":
                    
                case "exec":

                case "http-post":
                    break;

                default:
                    throw new Exception("Invalid action type.");
            }
        }
    }

    private string compileMonitor(MonitorResult mon, string val)
    {
        return val.Replace("{%data%}", mon.Data).Replace("{%error%}", mon.Error).Replace("{%success%}", Convert.ToString(mon.Success));
    }

}

