<%@ Page Language="C#" AutoEventWireup="true" Title="GoldmedalRitual Login" CodeFile="Login.aspx.cs" Inherits="Login" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="Scripts/bootstrap.bundle.js"></script>
    <script src="Scripts/Custom_Validation_Scripts.js"></script>
    <link rel="shortcut icon" href="Content/Images/GP%20LOGO.jpg" type="image/x-icon" />
    <title>Login</title>
    <link href="Content/Login.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css" />
    <link href="Content/JqueryToastCss.css" rel="stylesheet" />
    <script src="Scripts/JqueryToast.js"></script>
    <script src="Scripts/CustomToast.js"></script>

    <script>
        $(document).ready(function () {
            $('#showHide').click(function () {
                if ($(this).is(':checked')) {
                    $('#txtPassword').attr('type', 'text');
                    $('#lblshowPassword').text("Hide Password");
                } else {
                    $('#txtPassword').attr('type', 'password');
                    $('#lblshowPassword').text("Show Password");
                }
            });
        });
    </script>

    <style>
        .form-control {
            position: relative;
            padding: 10px;
            border: none;
            border-bottom: 1px solid #ccc;
            transition: all 0.3s ease-in-out;
        }

            .form-control:focus {
                outline: none;
                border-bottom: 2px solid #007BFF;
            }

            .form-control::after {
                content: '';
                position: absolute;
                bottom: 0;
                left: 0;
                width: 0%;
                height: 2px;
                background-color: #007BFF;
                transition: width 0.3s ease-in-out;
            }

            .form-control:focus::after {
                width: 100%;
            }

        .disabled {
            pointer-events: none;
            opacity: 0.6; /* Optional: Makes the button look faded */
        }

        .dxlpLoadingPanel_MaterialCompact .dxlp-loadingImage, .dxflFormLayout_MaterialCompact.dialog-formlayout.dialog-preparing:after, .dxlpLoadingPanelWithContent_MaterialCompact .dxlp-loadingImage {
            -webkit-border-radius: 50%;
            -moz-border-radius: 50%;
            -o-border-radius: 50%;
            -khtml-border-radius: 50%;
            border-radius: 50%;
            animation: dxLoadSpinSys 1s linear infinite;
            border: 4px solid #6b0218;
            border-top: 4px solid #EEE;
            width: 24px;
            height: 24px;
        }
    </style>
    <script>

        function CountDown() {
            var OTPTextBox = document.getElementById("txtOPT");
            var btnLogin = document.getElementById("btnLogin");
            var MobileNo = document.getElementById("txtMobileNo")

            // Set the end time to 5 minutes from now
            var endTime = Date.now() + 5 * 60 * 1000;

            // Update the countdown every second
            var intervalId = setInterval(function () {
                // Calculate the remaining time
                var remainingTime = Math.round((endTime - Date.now()) / 1000);

                // If the remaining time is zero or less, stop the countdown
                if (remainingTime <= 0) {
                    clearInterval(intervalId);
                    btnSendOTP.SetText('Send OTP')
                    btnSendOTP.GetMainElement().classList.remove('disabled');
                    btnSendOTP.clientEnabled = true;
                    OTPTextBox.disabled = true;
                    btnLogin.classList.add('disabled');
                    //Reset OTP From DataBase After CounDown Over
                    ResetOTP(MobileNo.value);
                } else {
                    // Update the button text with the remaining time
                    var minutes = Math.floor(remainingTime / 60);
                    var seconds = remainingTime % 60;
                    btnSendOTP.SetText(minutes + ":" + (seconds < 10 ? "0" : "") + seconds)
                    btnSendOTP.GetMainElement().classList.add('disabled');
                    btnSendOTP.clientEnabled = false;
                    OTPTextBox.disabled = false;
                    btnLogin.classList.remove('disabled');
                }
            }, 1000);
        }

        function ResetOTP(MobileNo) {
            $.ajax({
                type: 'POST',
                url: 'Services/CommonServices.asmx/resetOTP',
                data: JSON.stringify({ MobileNo: MobileNo }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    if (response.d === '1') {
                    }
                },
                error: function (error) {
                    console.error('Error:', error);
                },
            });
        }
    </script>
</head>
<body oncontextmenu="return false">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>

        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <dx:ASPxLoadingPanel runat="server" Theme="MaterialCompact" Modal="true" ID="lp" ClientInstanceName="lp"></dx:ASPxLoadingPanel>
                <dx:ASPxCallback runat="server" ID="Callback" ClientInstanceName="Callback" OnCallback="Callback_Callback">
                    <ClientSideEvents CallbackComplete="function(s,e){lp.Hide();}" />
                </dx:ASPxCallback>
            </ContentTemplate>
        </asp:UpdatePanel>
        <section class="vh-100 BG">
            <div class="container py-5 h-100">
                <div class="row d-flex justify-content-center align-items-center h-100">
                    <div class="col col-xl-10">
                        <div class="card fade-in-top" style="border-radius: 1rem;">
                            <div class="row g-0">
                                <div class="col-sm-5 col-md-6 col-lg-5 d-none d-md-block">
                                    <img src="Content/Images/GP%20LOGO.jpg" alt="GP LOGO" class="img-fluid" />
                                </div>

                                <div class="col-md-6 col-lg-7 d-flex align-items-center">
                                    <div class="card-body p-4 p-lg-5 text-black">

                                        <div>
                                            <div class="d-flex align-items-center mb-3 pb-1">
                                                <%--<i class="fas fa-cubes fa-2x me-3" style="color: #ff6219;"></i>--%>
                                                <span class="h1 fw-bold mb-0">Login</span>
                                            </div>

                                            <div class="input-group">
                                                <div class="form-floating mb-3">
                                                    <asp:TextBox runat="server" onchange="validateMobileNumber(this)" MaxLength="10" CssClass="form-control" ID="txtMobileNo" ClientIDMode="Static" placeholder="name@example.com"></asp:TextBox>
                                                    <label for="txtMobileNo">Mobile No</label>
                                                </div>
                                                <div class="pt-1 mb-0 ms-2 mt-1">
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                             <%--Theme="MGP"--%>
                                                            <dx:ASPxButton ValidationGroup="OTP" ForeColor="White" CssClass="btn btn-outline-secondary" ClientInstanceName="btnSendOTP" ID="btnSendOTP" runat="server" Text="Send OTP" OnClick="btnSendOTP_Click">
                                                                <ClientSideEvents Click="function(s,e){lp.Show();Callback.PerformCallback();}" />
                                                            </dx:ASPxButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMobileNo" Font-Size="Small" ForeColor="Red"
                                                SetFocusOnError="true" ErrorMessage="Please Enter Mobile No" ValidationGroup="OTP"></asp:RequiredFieldValidator>
                                        </div>



                                        <div class="form-floating mt-3">
                                            <asp:TextBox Enabled="false" onchange="validateOTPDigits(this,5)" runat="server" MaxLength="5" CssClass="form-control" ID="txtOPT" ClientIDMode="Static" placeholder="OTP"></asp:TextBox>
                                            <label for="txtOPT">Enter OTP</label>
                                            <div class="mt-3">
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOPT" Font-Size="Small" ForeColor="Red"
                                                    SetFocusOnError="true" ErrorMessage="Please Enter OTP" ValidationGroup="LOGIN"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>


                                        <div class="mt-3 d-flex justify-content-center">
                                            <div class="pt-1 mb-4">
                                                <asp:LinkButton ID="btnLogin" ClientIDMode="Static" runat="server" OnClientClick="lp.Show();Callback.PerformCallback();" CssClass="btn btn-outline-secondary btn-lg btn-block disabled" OnClick="btnLogin_Click" ValidationGroup="LOGIN">
                                                       <i class="fa-solid fa-chevron-right fa-xl"></i>
                                                </asp:LinkButton>
                                            </div>
                                            <%--   <div style="display: flex; flex-direction: row; justify-content: space-between">
                                                <label style="font-size: 12px; margin-left: 5px; margin-top: 6px;" for="showHide" id="lblshowPassword">Show Password</label>
                                                <div class="checkbox-wrapper-18" style="margin-left: 5px; margin-top: 5px;">
                                                    <div class="round">
                                                        <input type="checkbox" id="showHide" />
                                                        <label for="showHide"></label>
                                                    </div>
                                                </div>
                                            </div>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </form>
</body>
</html>


