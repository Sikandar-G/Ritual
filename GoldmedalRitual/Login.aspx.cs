using Engine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class Login : System.Web.UI.Page
{
    //Do not Change Or Modify The EncryptionKey
    string EncryptionKey = ConfigurationManager.AppSettings["EncryptionKey"];


    GoldDataAccess da = new GoldDataAccess();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    private bool SendOTPWithResponse(string MobileNO)
    {
        bool isOTPSend = false;
        string apiUrl = "http://sms6.rmlconnect.net:8080/bulksms/bulksms";
        string username = "goldmedal";
        string password = "sd56jjaa";
        string type = "0";
        string dlr = "1";
        string destination = MobileNO; // Mobile NO
        string source = "GLDMDL";
        string entityId = "1601100000000001629";
        string templateId = "1007714922322636513"; // Template ID
        string OTP = GenerateOTP().ToString();
        string message = $"Your OTP is:{ OTP } OTP Validate for 5 Minutes Only. Thanks Team Goldmedal";

        string apiUrlWithParams = $"{apiUrl}?username={username}&password={password}&type={type}&dlr={dlr}&destination={destination}&source={source}&entityid={entityId}&tempid={templateId}&message={message}";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = client.GetAsync(apiUrlWithParams).Result;

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            isOTPSend = response.IsSuccessStatusCode;
            string responseData = response.Content.ReadAsStringAsync().Result;

            SqlParameter[] objectPara = new SqlParameter[3];
            objectPara[0] = new SqlParameter("@OTP", OTP);
            objectPara[1] = new SqlParameter("@MobileNo", destination);
            objectPara[2] = new SqlParameter("@OUT", SqlDbType.Int);
            objectPara[2].Direction = ParameterDirection.Output;
            da.ExecuteNonQueryWithParameters("USP_OTPUpdate", objectPara);

            var result = objectPara[2].Value.ToString();
        }
        return isOTPSend;
    }
    private int GenerateOTP()
    {
        //Generarte 5 Digit Random Number
        Random random = new Random();
        int randomNumber = random.Next(10000, 100000);
        return randomNumber;
    }
    private void Cookies(UserInfo Info)
    {
        HttpCookie userInfo = new HttpCookie("userInfo");

        userInfo["UserName"] = Info.UserName;
        userInfo["UserID"] = EncryptionEngine.Encrypt(Info.SLNO.ToString(), EncryptionKey);
        userInfo["UserType"] = Info.UserType.ToString();
        userInfo["Status"] = Info.Status.ToString();
        userInfo["UniKey"] = Info.UniqueKey.ToString();
        string DecryptKey = EncryptionEngine.Decrypt(userInfo["UserID"].ToString(), EncryptionKey);
        userInfo.Expires = DateTime.Now.AddDays(30);
        Response.Cookies.Add(userInfo);
    }
    protected void btnSendOTP_Click(object sender, EventArgs e)
    {
        if (!ValidateUser(txtMobileNo.Text))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Toast", "toast('w','User Not Exists',4)", true);
            return;
        }
        SendOTPWithResponse(txtMobileNo.Text);
        ScriptManager.RegisterStartupScript(this, GetType(), "OTPCounDown", "CountDown()", true);
    }
    protected void Callback_Callback(object source, DevExpress.Web.CallbackEventArgs e)
    {
        Thread.Sleep(500);
    }
    private bool ValidateUser(string MobileNo)
    {
        SqlParameter[] objectPara = new SqlParameter[2];
        objectPara[0] = new SqlParameter("@MobileNo", MobileNo);
        objectPara[1] = new SqlParameter("@OUT", SqlDbType.Int);
        objectPara[1].Direction = ParameterDirection.Output;
        var result = da.ReturnDataTableWithParameters("USP_IsUserExist", objectPara);

        int.TryParse(objectPara[1].Value.ToString(), out int IsExists);

        if (IsExists != 1)
        {
            return false;
        }

        int.TryParse(result.Rows[0]["SLNO"].ToString(), out int slno);
        string UserName = result.Rows[0]["UserName"].ToString();
        int.TryParse(result.Rows[0]["UserType"].ToString(), out int UserType);
        int.TryParse(result.Rows[0]["Status"].ToString(), out int Status);
        string UniQue = result.Rows[0]["UniqueKey"].ToString();

        var Obj = new UserInfo()
        {
            SLNO = slno,
            UserName = UserName,
            UserType = UserType,
            Status = Status,
            UniqueKey = UniQue
        };

        Cookies(Obj);
        return result.Rows.Count > 0;
    }
    private void ClearValidationErrors()
    {
        foreach (var control in new TextBox[] { txtMobileNo, txtOPT })
        {
            control.BorderColor = System.Drawing.Color.Empty;
        }

    }
    protected bool CheckMandatoryFields()
    {
        var mandatoryFields = new Dictionary<string, Control>()
    {
        { "Mobile No", txtMobileNo },
        { "OTP", txtOPT },
    };

        foreach (var entry in mandatoryFields)
        {
            var fieldName = entry.Key;
            var control = entry.Value;

            if (!IsFieldValid(control, fieldName))
                return false;
            ClearValidationErrors();
        }

        return true;
    }
    private bool IsFieldValid(Control control, string fieldName)
    {
        if (control is TextBox textBox && textBox.Text == string.Empty)
        {
            ShowValidationMessage(fieldName, "Please enter");
            textBox.BorderColor = Color.Red;
            textBox.Focus();
            return false;
        }
        return true;
    }
    private void ShowValidationMessage(string fieldName, string messagePrefix)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "ToastMessage", $"toast('w','{messagePrefix} <b>{fieldName}</b>.',3);", true);
    }


    protected void btnLogin_Click(object sender, EventArgs e)
    {

        if (!CheckMandatoryFields())
        {
            return;
        }

        SqlParameter[] objectPara = new SqlParameter[3];
        objectPara[0] = new SqlParameter("@MobileNo", txtMobileNo.Text);
        objectPara[1] = new SqlParameter("@OTP", txtOPT.Text);
        objectPara[2] = new SqlParameter("@OUT", SqlDbType.Int);
        objectPara[2].Direction = ParameterDirection.Output;
        var result = da.ReturnDataTableWithParameters("USP_LoginValidation", objectPara);

        int.TryParse(objectPara[2].Value.ToString(), out int isValidUSer);

        switch (isValidUSer)
        {
            case 1:
                resetOTP(txtMobileNo.Text);
                Response.Redirect("~/Pages/Home.aspx");
                break;
            case 202:
                ScriptManager.RegisterStartupScript(this, GetType(), "Toast", "toast('e','Invalid OTP',5)", true);
                break;
            case 101:
                ScriptManager.RegisterStartupScript(this, GetType(), "Toast", "toast('e','User InActive',5)", true);
                break;
            default:
                ScriptManager.RegisterStartupScript(this, GetType(), "Toast", "toast('e','User Not Exists',5)", true);
                break;
        }


    }


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


public class UserInfo
{
    public int SLNO { get; set; }
    public string UserName { get; set; }
    public int UserType { get; set; }
    public int Status { get; set; }
    public string UniqueKey { get; set; }
}
