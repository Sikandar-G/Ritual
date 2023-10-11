using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


/// <summary>
/// Summary description for GoldLogging
/// </summary>
public class GoldLogging
{
    private string ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;
    private System.Web.HttpContext context = null;
    public GoldLogging()
    {
        //
        // TODO: Add constructor logic here
        //
        context = System.Web.HttpContext.Current;
    }
    public void SendErrorToText(Exception ex)
    {
        var line = Environment.NewLine + Environment.NewLine;
        ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
        Errormsg = ex.GetType().Name.ToString();
        extype = ex.GetType().ToString();
        exurl = context.Request.Url.ToString();
        ErrorLocation = ex.Message.ToString();

        try
        {
            string filepath = context.Server.MapPath("~/App_Data/Error/error.txt");  //Text File Path

          
            using (StreamWriter sw = File.AppendText(filepath))
            {
                string error = string.Format("Log Written Date: {0}{1}Error Line No : {2}{1}Error Message: {3}{1}Exception Type: {4}{1}Error Location : {5}{1} Error Page Url: {6}{1}User Host IP: {7}{1}", DateTime.Now, line, ErrorlineNo, Errormsg, extype, ErrorLocation, exurl, hostIp);
                sw.WriteLine(string.Format("-----------Exception Details on  {0}-----------------", DateTime.Now));
                sw.WriteLine("-------------------------------------------------------------------------------------");
                sw.WriteLine(line);
                sw.WriteLine(error);
                sw.WriteLine("--------------------------------*End*------------------------------------------");
                sw.WriteLine(line);
                sw.Flush();
                sw.Close();

            }

        }
        catch (Exception e)
        {
            //throw new Exception(string.Format("Unable to log this error: {0}", e.InnerException));
            
        }
    }
}