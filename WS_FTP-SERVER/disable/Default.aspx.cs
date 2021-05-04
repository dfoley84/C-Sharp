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

public partial class _Default : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        errorDiv.Visible = false;
        successDiv.Visible = false;
        DisableDiv.Visible = false;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string sqlupdate = null;
        string sqlCheckUser = null;
        string user = username.Text;
        Regex reg = new Regex(@"[^a-zA-Z]");

        //Simple Text Field Vaildator
        if (user == String.Empty)
        {
            username.Text = string.Empty;
            return;
        }//end if 
        else
        {
            SqlConnection cnn;
            SqlCommand command;
            string ConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            sqlCheckUser = "SELECT COUNT(*) FROM dbo.Host_Users WHERE User_LoginID=@user";
            sqlupdate = "UPDATE dbo.Host_Users SET Account_Disabled='1' WHERE User_LoginID=@user";
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
                        errorDiv.Visible = false;
                        successDiv.Visible = true;
                        DisableDiv.Visible = false;

                        if (checkbox.Checked == true)
                        {
                            errorDiv.Visible = false;
                            successDiv.Visible = false;
                            DisableDiv.Visible = true;
                            using (Process process = Process.Start(@"C:\Program Files (x86)\Ipswitch\WS_FTP Server\iftpaddu.exe", "-mod -u " + user + " -h <HostName> -active"))
                            {
                                proc.WaitForExit(); //Wait for Batch File to Exit.
                            }
                            using (Process process1 = Process.Start(@"C:\Program Files (x86)\Ipswitch\WS_FTP Server\iftpaddu.exe", "-mod -u " + user + "-c1 -h <HostName> -active"))
                            { 
                                process1.WaitForExit(); //Wait for Batch File to Exit.
                            }
                            using (Process process2 = Process.Start((@"C:\Program Files (x86)\Ipswitch\WS_FTP Server\iftpaddu.exe", "-mod -u " + user + "-c2 -h <HostName> -active"))
                            {
                                process2.WaitForExit(); //Wait for Batch File to Exit.
                            }
                            using (Process process3 = Process.Start(@"C:\Program Files (x86)\Ipswitch\WS_FTP Server\iftpaddu.exe", "-mod -u " + user + "-c3 -h <HostName> -active"))
                            {
                                process3.WaitForExit(); //Wait for Batch File to Exit.
                            }
                            using (Process process4 = Process.Start(@"C:\Program Files (x86)\Ipswitch\WS_FTP Server\iftpaddu.exe", "-mod -u " + user + "-c4 -h <HostName> -active");
                            {
                                process4.WaitForExit(); //Wait for Batch File to Exit.
                            }
                            using (Process process5 = Process.Start(@"C:\Program Files (x86)\Ipswitch\WS_FTP Server\iftpaddu.exe", "-mod -u " + user + "-c5 -h <HostName> -active");
                            {
                                process5.WaitForExit(); //Wait for Batch File to Exit.
                            }          
                            Str += "User Account was found for the Following User ";
                            Str += Environment.NewLine;
                            Str += Environment.NewLine;
                            Str += user;
                            Str += Environment.NewLine;
                            Str += " Account is now Disabled.";
                            Str += Environment.NewLine;
                            Str += Environment.NewLine;
                            Str += "Thanks";
                            MailMessage mail = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient(""); //SMTP SERVER IP ADDRESS
                            mail.From = new MailAddress("");
                            mail.To.Add("");
                            mail.CC.Add("");
                            mail.Subject = "Disabled Account";
                            mail.Body = Str;
                            SmtpServer.Send(mail);
                            username.Text = string.Empty;
                        }
                    }//End Count If
                    else
                    {
                        errorDiv.Visible = true;
                        successDiv.Visible = false;
                        DisableDiv.Visible = false;

                        Str += "No User Account found for the Following User ";
                        Str += Environment.NewLine;
                        Str += Environment.NewLine;
                        Str += user;
                        Str += Environment.NewLine;
                        Str += Environment.NewLine;
                        Str += "Thanks";
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("");
                        mail.From = new MailAddress("");
                        mail.To.Add("");
                        mail.Subject = "Disabled Account";
                        mail.Body = Str;
                        SmtpServer.Send(mail);
                        username.Text = string.Empty;
                    }//End Else
                }//End Using 
                command.Dispose();//Dispose of Command
                cnn.Close();//Close Database Connection
            }//End Try
            catch (Exception ex)
            { }//End Catach
        }//End Else 
    }//End Button on Click
}//End C# Class
