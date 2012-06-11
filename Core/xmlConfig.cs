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
using System.Xml;

public static class xmlConfig
{
    private static bool _WaitKeyOnExit;
    private static bool _SingleIstance;
    private static bool _Loaded;
    private static List<Monitor> _Items;
    private static Dictionary<string, Action> _Actions;

    public static void Load(String FileName)
    {
        XmlDocument doc = new XmlDocument();
        _Items = new List<Monitor>();
        _Actions = new Dictionary<string, Action>();

        /* Load Settings */
        doc.Load(FileName);
        _WaitKeyOnExit = Convert.ToBoolean(doc.GetElementsByTagName("WaitKeyOnExit").Item(0).InnerText);
        _SingleIstance = Convert.ToBoolean(doc.GetElementsByTagName("SingleIstance").Item(0).InnerText);
  
        /* Load Items */
        foreach (XmlNode nd in doc.SelectNodes("Config/Items"))
            {
                foreach (XmlNode n in nd.ChildNodes)
                {
                    //Console.WriteLine("Riga: " + n.Attributes.Item(0).Name + " > " + n.InnerText);
                    Monitor Item = new Monitor();
                    Item.Target = n.InnerText;
                    for (int i = 0; i < n.Attributes.Count; i++){
                        switch (n.Attributes.Item(i).Name.ToLower()) { 
                            case "name":
                                Item.Name = n.Attributes.Item(i).Value;
                                break;

                            case "type":
                                Item.Type = n.Attributes.Item(i).Value;
                                break;

                            case "schedule":
                                Item.Schedule = n.Attributes.Item(i).Value;
                                break;

                            case "condition":
                                Item.Condition = n.Attributes.Item(i).Value;
                                break;

                            case "alarm":
                                Item.Alarm = n.Attributes.Item(i).Value;
                                break;

                            default:
                                throw new Exception("Unknown atribute " + n.Attributes.Item(i).Name);
                        }
                    }
                    _Items.Add(Item);
                }
            }

        /* Load Items */
        foreach (XmlNode nd in doc.SelectNodes("Config/Actions"))
        {
            foreach (XmlNode n in nd.ChildNodes)
            {
                Console.WriteLine("Riga: " + n.Attributes.Item(0).Name + " > " + n.InnerText);
                Action Item = new Action();
                for (int i = 0; i < n.Attributes.Count; i++)
                {
                    switch (n.Attributes.Item(i).Name.ToLower())
                    {
                        case "name":
                            Item.Name = n.Attributes.Item(i).Value;
                            break;

                        case "type":
                            Item.Type = n.Attributes.Item(i).Value;
                            break;

                        case "to":
                            Item.To = n.Attributes.Item(i).Value;
                            break;

                        case "from":
                            Item.From= n.Attributes.Item(i).Value;
                            break;

                        case "subject":
                            Item.Subject = n.Attributes.Item(i).Value;
                            break;

                        case "message":
                            Item.Message = n.Attributes.Item(i).Value;
                            break;

                        case "data":
                            Item.Data = n.Attributes.Item(i).Value;
                            break;

                        default:
                            throw new Exception("Unknown atribute " + n.Attributes.Item(i).Name);
                    }
                }
                _Actions.Add(Item.Name.ToLower().Trim(), Item);
            }
        }
        _Loaded = true;
    }

    public static bool WaitKeyOnExit{
        get { return _WaitKeyOnExit; }
    }

    public static bool SingleIstance
    {
        get { return _SingleIstance; }
    }

    public static bool Loaded
    {
        get { return _Loaded; }
    }

    public static List<Monitor> Items
    {
        get { return _Items; }
    }

    public static Dictionary<string, Action> Actions
    {
        get { return _Actions; }
    }
}