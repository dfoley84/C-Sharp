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
<div class="error">User Account Found!</div>
</asp:Panel>
<asp:Panel runat="server" ID="successDiv">
<div class="success">No Account Found, User Created!</div>
</asp:Panel>


<div id="form-main">
  <div id="form-div">     
        <form id="form1" name="form1" runat="server">
           <p class="name">
            <asp:TextBox id="name"  class="validate[required,custom[onlyLetter],length[0,100]] feedback-input" runat="server" type="text" placeholder="Enter in Full Name"/> <br />
              </p>
               <p class="email">
              <asp:TextBox id="email" runat="server" class="validate[required,custom[email]] feedback-input" type="text" placeholder="Email" />  <br />
              </p>
              <p class="username">
               <asp:TextBox id="username" runat="server"  class="validate[required,custom[onlyLetter],length[0,100]] feedback-input" type="text"  placeholder="Username" /><br />
              </p>
                    <p class="styled-select">
                         <asp:DropDownList ID="GroupDropDown" runat="server" Height="44px" Width="546px">
                         <asp:ListItem>IT</asp:ListItem>
                                <asp:ListItem>Development</asp:ListItem>
                     </asp:DropDownList>
             </p>

    <div class="submit">
      <asp:Button ID="Submit" runat="server" type="button" value = "SEND" OnClick="Button1_Click" OnClientClick="myFunction()" Text="Submit" Width="552px"/>
      
   </div>
    <div class="password" style="display:none;">
      <label for="password_row">Main Password :</label><asp:TextBox id="password_row" runat="server" /><br />
      <label for="pass1">Password Sub1 :</label><asp:TextBox id="pass1" runat="server" /><br />
       <label for="pass2">Password Sub2 :</label><asp:TextBox id="pass2" runat="server" /><br />
       <label for="pass3">Password Sub3 :</label><asp:TextBox id="pass3" runat="server" /><br />
       <label for="pass4">Password Sub4 :</label><asp:TextBox id="pass4" runat="server" /><br />
       <label for="pass5">Password Sub5 :</label><asp:TextBox id="pass5" runat="server" /><br />
        </div>
                </form>
                  </div>
    </div>
    </div>
</body>
</html>
