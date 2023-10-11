using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

/// <summary>
/// Summary description for CommonServices
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class CommonServices : System.Web.Services.WebService
{
    GoldDataAccess da = new GoldDataAccess();
    public CommonServices()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    [WebMethod]
    public int resetOTP(string MobileNo)
    {
        SqlParameter[] objectPara = new SqlParameter[2];
        objectPara[0] = new SqlParameter("@MobileNo", MobileNo);
        objectPara[1] = new SqlParameter("@OUT", SqlDbType.Int);
        objectPara[1].Direction = ParameterDirection.Output;
        da.ExecuteNonQueryWithParameters("USP_ResetOTP", objectPara);
        int.TryParse(objectPara[1].Value.ToString(), out int result);
        return result;
    }

}
