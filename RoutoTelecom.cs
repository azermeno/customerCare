using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Text; 

/// <summary>
/// Summary description for RoutoSMSTelecom
/// </summary>
public class RoutoSMSTelecom
{
	public RoutoSMSTelecom()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public String user = "";
    public String pass = "";
    public String number = "";
    public String ownnum = "";
    public String message = "";
    public String messageId = "";
    public String type = "";
    public String model = "";
    public String op = "";

    public String SetUser(String newuser)
    {
        this.user = newuser;
        return user;
    }

    public String SetPass(String newpass)
    {
        this.pass = newpass;
        return pass;
    }

    public String SetNumber(String newnumber)
    {
        this.number = newnumber;
        return number;
    }

    public String SetOwnNumber(String newownnum)
    {
        this.ownnum = newownnum;
        return ownnum;
    }

    public String SetType(String newtype)
    {
        this.type = newtype;
        return type;
    }

    public String SetModel(String newmodel)
    {
        this.model = newmodel;
        return model;
    }

    public String SetMessage(String newmessage)
    {
        this.message = newmessage;
        return message;
    }

    public String SetMessageId(String newmessageid)
    {
        this.messageId = newmessageid;
        return messageId;
    }

    public String SetOp(String newop)
    {
        this.op = newop;
        return op;
    }

    private String urlencode(String str)
    {
        return HttpUtility.UrlEncode(str);
    }

    public String Send()
    {
        String Body = "";
        Body += "number=" + this.number;
        Body += "&user=" + urlencode(this.user);
        Body += "&pass=" + urlencode(this.pass);
        Body += "&message=" + urlencode(this.message); 

        if ((this.messageId).Length >= 1)
        {
            Body += "&mess_id=" + urlencode(this.messageId) + "&delivery=1";
        }

        if ((this.ownnum) != "")
        {
            Body += "&ownnum=" + urlencode(this.ownnum);
        }

        if ((this.model) != "")
        {
            Body += "&model=" + urlencode(this.model);
        }

        if ((this.op) != "")
        {
            Body += "&op=" + urlencode(this.op);
        }

        if ((this.type) != "")
        {
            Body += "&type=" + urlencode(this.type);
        }

        int ContentLength = Body.Length;

        String Host = "smsc5.routotelecom.com";

        String Header = "POST /cgi-bin/SMSsend HTTP/1.0\n" + "Host: " + Host + "\n" + "Content-Type: application/x-www-form-urlencoded\n" + "Content-Length: " + ContentLength + "\n\n" + Body + "\n";

        Socket mysocket = null;
        TcpClient tcpClient;
        StreamWriter streamWriter = null;
        StreamReader streamReader = null;
        NetworkStream networkStream = null;

        String line = "";
        try
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(Host, 80);
            
            mysocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mysocket.Connect(Host, 80);
            
            networkStream = tcpClient.GetStream();

            streamReader = new StreamReader(networkStream);
            streamWriter = new StreamWriter(networkStream);
            streamWriter.WriteLine(Header);
            streamWriter.Flush();

            line = streamReader.ReadToEnd();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return Header + " " + line;
    }


    public String GetUser()
    {
        return this.user;
    }

    public String GetPass()
    {
        return this.pass;
    }

    public String GetNumber()
    {
        return this.number;
    }

    public String GetMessage()
    {
        return this.message;
    }

    public String GetMessageId()
    {
        return this.messageId;
    }

    public String GetOwnNum()
    {
        return this.ownnum;
    }

    public String GetOp()
    {
        return this.op;
    }

    public new String GetType()
    {
        return this.type;
    }

    public String GetModel()
    {
        return this.model;
    }

    public static byte[] ReadFully(Stream stream)
    {
        byte[] buffer = new byte[32768];
        using (MemoryStream ms = new MemoryStream())
        {
            while (true)
            {
                int read = stream.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                    return ms.ToArray();
                ms.Write(buffer, 0, read);
            }
        }
    }

    public string getImage(string url)
    {
        try

        {
            WebRequest request = WebRequest.Create(url);

            byte[] file = new byte[1024 * 1024];
            Stream stream = request.GetResponse().GetResponseStream();

            file = ReadFully(stream);

            return Convert.ToBase64String(file, Base64FormattingOptions.InsertLineBreaks);
        }

        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
