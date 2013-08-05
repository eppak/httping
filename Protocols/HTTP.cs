/* The MIT License (MIT)
Copyright (c) 2013 Alessandro Cappellozza (alessandro.cappellozza@gmail.com)

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
using System.Net;
using System.IO;
using System.Web;

public static class HTTP
{

    public static MonitorResult GET(string strURL)
    {
        MonitorResult Res = new MonitorResult() { Data = "", Success = false, Error = "" };
        WebResponse objResponse = default(WebResponse);
        WebRequest objRequest = HttpWebRequest.Create(strURL);
        objRequest.Method = "GET";
        objRequest.ContentType = "application/x-www-form-urlencoded";

        try {
            objResponse = objRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                Res.Success = true;
                Res.Data = sr.ReadToEnd();
                sr.Close();
            }
        }
        catch (Exception e){
            Res.Success = false;
            Res.Data = "";
            Res.Error = e.Message;
        }

        return Res;
    }

    public static MonitorResult POST(string strURL, Dictionary<string, string> Data)
    {
        MonitorResult Res = new MonitorResult() { Data = "", Success = false, Error = "" };
        string postData = "";

        WebResponse objResponse = default(WebResponse);            
        WebRequest objRequest = HttpWebRequest.Create(strURL);

        objRequest.Method = "POST";
        objRequest.ContentType = "application/x-www-form-urlencoded";
          
        foreach (KeyValuePair<string, string> V in Data){
                postData += V.Key + "=" + HttpUtility.UrlEncode(V.Value) + ";";
        }
            
        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        Stream dataStream = objRequest.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        try{
            objResponse = objRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                Res.Success = true;
                Res.Data = sr.ReadToEnd();
                sr.Close();
            }
        }
        catch (Exception e){
            Res.Success = false;
            Res.Data = "";
            Res.Error = e.Message;
        }

        return Res;
    }
}
