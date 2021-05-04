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
        //SubAccounts 
        PasswordGenerator passwordGenerator = PasswordGenerator.GetPasswordGenerator();
        string MainAccount =   passwordGenerator.NewPassword();
        string sqlCheckUser = null;
        string user = username.Text;
        Regex reg = new Regex(@"[^a-zA-Z]");
        //var checkbox = !string.IsNullOrEmpty(Request.Form["checkbox"]);

        //Simple Text Field Vaildator
        if ((user == String.Empty) || reg.IsMatch(user))
        {
            username.Text = string.Empty;
            return;
        }//end if 
        else
        {
            try
            {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString);
            SqlCommand command;
            SqlDataReader dataReader;
              cnn.Open();
            command =  new SqlCommand ("SELECT * FROM dbo.Host_Users WHERE User_LoginID=@user", cnn);
            command.Parameters.Add("@User", SqlDbType.NVarChar,100).Value = user;
            dataReader = command.ExecuteReader();

                if(dataReader.HasRows)
                {
                    var proc = System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Ipswitch\WS_FTP Server\iftpaddu.exe", "-mod -u " + user + " -h <HOSTNAME> -p " + MainAccount);
                    proc.WaitForExit(); //Wait for Batch File to Exit.
                        errorDiv.Visible = false;
                        successDiv.Visible = true;
                        DisableDiv.Visible = false;
                        dataReader.Read();
                    string userLogin = dataReader["User_LoginID"].ToString();
                    string email = dataReader["User_Email_Addr"].ToString(); 
                    string userFullname = dataReader["User_FullName"].ToString();
                    dataReader.Close();

                            string Str = "Hi " +userFullname+".";
                            Str += Environment.NewLine;
                            Str += Environment.NewLine;
                            Str += "MainAccount passwords for user is the Following;";
                            Str += Environment.NewLine;    
                            Str += Environment.NewLine;
                            Str += "Password :" + " " + MainAccount;
                            Str += Environment.NewLine;
                            Str += Environment.NewLine;
                            Str += "Any Issues with the Password Please Reply to this Email";
                            Str += Environment.NewLine;
                            Str += Environment.NewLine;
                            Str += "Thanks";                  

                            MailMessage mail = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient("");
                            mail.From = new MailAddress("");
                            mail.To.Add(email);
                            mail.CC.Add("");
                            mail.Subject = "Main Account Password Reset";
                            mail.Body = Str;
                            SmtpServer.Send(mail);
                            username.Text = string.Empty;
                    }//End DATAREADER If
                    else
                    {}//End Else
                command.Dispose();//Dispose of Command
                cnn.Close();//Close Database Connection
            cnn.Dispose();
            }//End Try
            catch (Exception ex)
            { }//End Catach
        }//End Else 
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
        int numbercase = ;
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
