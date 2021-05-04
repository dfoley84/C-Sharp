<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server"> 
    <link rel="stylesheet" type="text/css" href="StyleSheet.css" />
    <title></title>
</head>
<body>

<div id="Title">
<asp:Panel runat="server" ID="errorDiv">
<div class="error">No Account Found</div>
</asp:Panel>
<asp:Panel runat="server" ID="successDiv">
<div class="success">User Account Found </div>
</asp:Panel>
<asp:Panel runat="server" ID="DisableDiv">
<div class="disabled">User Account Found, Account Disabled.</div>
</asp:Panel>


<div id="form-main">
  <div id="form-div">     
        <form id="form1" name="form1" runat="server">
              <p class="username">
               <asp:TextBox id="username" runat="server"  class="validate[required,custom[onlyLetter],length[0,100]] feedback-input" type="text"  placeholder="Username" /><br />
                <asp:CheckBox ID="checkbox" runat="server" /> <asp:label runat="server">To Disable User Account plese Check this checkbox.</asp:label><br />
              </p>
    <div class="submit">
      <asp:Button ID="Submit" runat="server" type="button" value = "SEND" OnClick="Button1_Click" Text="Submit" Width="552px"/>
   </div>
                </form>
                  </div>
    </div>
    </div>
</body>
</html>
