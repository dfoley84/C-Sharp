using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Deployment;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;


public partial class _Default : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        errorDiv.Visible = false;
        successDiv.Visible = false;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
       
        //Gobal Varables 
        string user = username.Text; 
        string userFullName = name.Text;
        string userEmail = email.Text;   
        string mainPass = password_row.Text;
        string dropdown = null;
        string dropdownSub = null; 
   

        //SQL Varables
        
        PasswordGenerator passwordGenerator = PasswordGenerator.GetPasswordGenerator();
        string sqlCheckUser = null;
        string sqlCheckUserSub1 = null;
        string sqlCheckUserSub2 = null;
        string sqlCheckUserSub3 = null;
        string sqlCheckUserSub4 = null;
        string sqlCheckUserSub5 = null;
        string SQLUpdatePasswordExpire = null;
        string SQLUpdateEmail = null;
        string SQLUpdateFullName = null;
        string updateSubAccount1 = null;
        string updateSubAccount2 = null;
        string updateSubAccount3 = null;
        string updateSubAccount4 = null;
        string updateSubAccount5 = null;
        string mainpass =   passwordGenerator.NewPassword();

        //Below Code will Add the Path to have the Folders Created.
       string FolderName = @"\\<FILE PATH>\e$\\users\" + user;
       string pathString = System.IO.Path.Combine(FolderName, user + "TestAccount");



        //Simple Text Field Vaildator
        if (user == String.Empty)
        {
            return;
        }//end if 
        else
        {
            //Creating If Statments for Groups and SubAccount Groups.
            if (GroupDropDown.SelectedItem.Value == " ")
            {
                dropdown = "";
                dropdownSub = @"""  sub Accounts""";
            }
            else if (GroupDropDown.SelectedItem.Value == " ")
            {
                dropdown = "";
                dropdownSub = @"""  Sub accounts""";
            }
            else if (GroupDropDown.SelectedItem.Value == " ")
            {
                dropdown = @""" """;
                dropdownSub = @"""ub accounts""";
            }
            else if (GroupDropDown.SelectedItem.Value == "")
            {
                dropdown = "";
                dropdownSub = @""" Sub accounts""";
            }
            else if (GroupDropDown.SelectedItem.Value == "")
            {
                dropdown = ;
                dropdownSub = @""" subaccounts""";
            }
            else if (GroupDropDown.SelectedItem.Value == "")
            {
                dropdown = "";
                dropdownSub = @""" Sub accounts""";
            }
            else if (GroupDropDown.SelectedItem.Value == "")
            {
                dropdown = "";   
            }
            else if (GroupDropDown.SelectedItem.Value == "")
            {
                dropdown = "";
            }


             /*
             * Connection TO SQL Database to Update the Map Drives / Add Email Address and finally Ad 42 day Account Lockout
             * */

            SqlConnection cnn;
            SqlCommand command;
            string ConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            sqlCheckUser = "SELECT COUNT(*) FROM dbo.Host_Users WHERE User_LoginID=@user";
            cnn = new SqlConnection(ConnectionString);
            try
            {
                cnn.Open();
                using (command = new SqlCommand(sqlCheckUser, cnn))
                {
                    command.Parameters.Add("user", SqlDbType.Char).Value = user;
                    int userCount = (int)command.ExecuteScalar();
                    if (userCount > 0)
                    {
                        errorDiv.Visible = true;
                        successDiv.Visible = false;
                    }//end if
                    else
                    {
                        errorDiv.Visible = false;
                        successDiv.Visible = true;

                        /************************************************************
                         * If User Is not found within the WS_FTP Database Create Sub Accounts
                         * 
                         * */

                        //Creating User Directory  for Sub Accounts
                      System.IO.Directory.CreateDirectory(pathString);


                      var proc = System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Ipswitch\WS_FTP Server\iftpaddu.exe", "-add -u " + user + " -h <HOSTNAME> -p " + mainpass + " +lock +root +g " + dropdown);
                      proc.WaitForExit(); //Wait for Batch File to Exit.

                        SQLUpdatePasswordExpire = "UPDATE dbo.Host_Users SET Pass_Expire_Days = 42, Pass_Expire_Option = 1  WHERE User_LoginID=@user";
                        SQLUpdateEmail = "UPDATE dbo.Host_Users SET User_Email_addr='" + userEmail + "' WHERE User_LoginID=@user";
                        SQLUpdateFullName = "UPDATE dbo.Host_Users SET User_FullName='" + userFullName + "' WHERE User_LoginID=@user";
                        cnn = new SqlConnection(ConnectionString);
                         try
              {
                cnn.Open();
                using (command = new SqlCommand(sqlCheckUser, cnn))
                {
                    command.Parameters.Add("user", SqlDbType.Char).Value = user;
                    int userCount1 = (int)command.ExecuteScalar();
                    if (userCount1 > 0)
                    {

                        errorDiv.Visible = false;
                        successDiv.Visible = true;

                        //Update User Email Address
                        SqlCommand Udpateemail = new SqlCommand(SQLUpdateEmail, cnn);
                        Udpateemail.Parameters.Add("user", SqlDbType.Char).Value = user;
                        Udpateemail.ExecuteNonQuery();

                        //Update The Password Expire Days
                        SqlCommand cmd = new SqlCommand(SQLUpdatePasswordExpire, cnn);
                        cmd.Parameters.Add("user", SqlDbType.Char).Value = user;
                        cmd.ExecuteNonQuery();
                    
                        //Update User Full Name
                        SqlCommand updateName = new SqlCommand(SQLUpdateFullName, cnn);
                        updateName.Parameters.Add("user", SqlDbType.Char).Value = user;
                        updateName.ExecuteNonQuery();

                        /**********
                         * Sending Email with Password.
                         * 
                         * */
                        string Str = "Hi  ";
                        Str += Environment.NewLine;
                        Str += Environment.NewLine;
                        Str += "SFTP Account has been Created for : " + " " + userFullName;
                        Str += Environment.NewLine;
                        Str += Environment.NewLine;
                        Str += "Username : " + " " + user;
                        Str += Environment.NewLine;
                        Str += "Main Account Password : " + " " + mainpass;
                        Str += Environment.NewLine;
                        Str += Environment.NewLine;
                        Str += "Thanks";

                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("");
                        mail.From = new MailAddress("");
                        mail.To.Add("");
                        mail.CC.Add(" ");
                        mail.Subject = "User Account Created for " + userFullName;
                        mail.Body = Str;
                        SmtpServer.Send(mail);
                        username.Text = string.Empty;
                    }//end if
                    else
                    {}//End Else
                }//End Using
            }//End Try
            catch (Exception ex)
            { }
                    }
              }//end using
                command.Dispose();
                cnn.Close();
            }//end Try
            catch(Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Not Connected')", true);
            }
           // }//end using
        }//end els
    }//End Button on Click


    /*
       * Password Generator
       * */
    public sealed class PasswordGenerator
    {
        private static readonly PasswordGenerator instance = new PasswordGenerator();
        private Random random = new Random();
        private PasswordGenerator() { }
        public static PasswordGenerator GetPasswordGenerator()
        {
            return instance;
        }
        int lowerscase = ;
        int uppercase = ;
        int numbercase =;
        int spe = ;
        private const string lowers = "abcdefghijklmnopqrstuvwxyz";
        private const string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string number = "0123456789";
        private const string specail = "{$}?_=/+-!*";
        public string NewPassword()
        {
            string generated = "!";
            for (int i = 1; i <= lowerscase; i++)
                generated = generated.Insert(
                    this.random.Next(generated.Length),
                    lowers[this.random.Next(lowers.Length - 1)].ToString()
                );
            for (int i = 1; i <= spe; i++)
                generated = generated.Insert(
                    this.random.Next(generated.Length),
                    specail[this.random.Next(specail.Length - 1)].ToString()
                );
            for (int i = 1; i <= uppercase; i++)
                generated = generated.Insert(
                    this.random.Next(generated.Length),
                    uppers[this.random.Next(uppers.Length - 1)].ToString()
                );

            for (int i = 1; i <= numbercase; i++)
                generated = generated.Insert(
                    this.random.Next(generated.Length),
                    number[this.random.Next(number.Length - 1)].ToString()
                );

            return generated.Replace("!", string.Empty);
        }
    }
}//End C# Class


                   