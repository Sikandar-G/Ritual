<%@ Page Language="C#" Title="Goldmedal Ritual-Event Master" MasterPageFile="~/Master.master" AutoEventWireup="true" CodeFile="EventMaster.aspx.cs" Inherits="Pages_EventMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <asp:TextBox runat="server" MaxLength="5" CssClass="form-control" ID="txtEventName" ClientIDMode="Static" placeholder="EventName"></asp:TextBox>
                <label for="txtEventName">Event Name</label>
                <div class="mt-3">
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEventName" Font-Size="Small" ForeColor="Red"
                        SetFocusOnError="true" ErrorMessage="Please Event Name" ValidationGroup="LOGIN"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-outline-secondary" Text="Submit" />

            </div>
        </div>
    </div>
</asp:Content>


