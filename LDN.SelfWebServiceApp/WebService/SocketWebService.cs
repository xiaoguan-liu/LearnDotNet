using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LDN.SelfWebServiceApp.WebService
{
    public class SocketWebService
    {
        private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  //侦听socket
        private string webRoot = @"D:\TestProject\MyTest\LearnDotNet\LDN.SelfWebServiceApp\pages\";
        private string defaultPage = "index.html,home.html";


        public void Start()
        {
            webRoot = @"D:\TestProject\MyTest\LearnDotNet\LDN.SelfWebServiceApp\pages\";
            _socket.Bind(new System.Net.IPEndPoint(IPAddress.Any, 8080));
            _socket.Listen(100);
            _socket.BeginAccept(new AsyncCallback(OnAccept), _socket);  //开始接收来自浏览器的http请求（其实是socket连接请求）
            writeLog("Socket Web Server 已启动监听！" + Environment.NewLine + "  监听端口：" + ((IPEndPoint)_socket.LocalEndPoint).Port);
            Console.ReadKey();
            _socket.Dispose();
        }

        public void Start2()
        {
            IPHostEntry lipa = Dns.Resolve("host.contoso.com");
            IPEndPoint lep = new IPEndPoint(lipa.AddressList[0], 11000);

            Socket s = new Socket(lep.Address.AddressFamily,
                                           SocketType.Stream,
                                              ProtocolType.Tcp);
            try
            {
                s.Bind(lep);
                s.Listen(1000);

                while (true)
                {
                    //allDone.Reset();

                    Console.WriteLine("Waiting for a connection...");
                    s.BeginAccept(new AsyncCallback(OnAccept), s);

                    // allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// 接受处理http的请求
        /// </summary>
        /// <param name="ar"></param>
        private void OnAccept(IAsyncResult ar)
        {
            try
            {
                //_socket.BeginAccept(new AsyncCallback(OnAccept), _socket);  //开始接收来自浏览器的http请求（其实是socket连接请求）

                Socket socket = ar.AsyncState as Socket;
                Socket web_client = socket.EndAccept(ar);  //接收到来自浏览器的代理socket

                //NO.1  并行处理http请求
                socket.BeginAccept(new AsyncCallback(OnAccept), socket); //开始下一次http请求接收   （此行代码放在NO.2处时，就是串行处理http请求，前一次处理过程会阻塞下一次请求处理）

                byte[] recv_Buffer = new byte[1024 * 640];
                int recv_Count = web_client.Receive(recv_Buffer);  //接收浏览器的请求数据
                if (recv_Count <= 0)
                {
                    writeLog("recv_Count为0");
                    web_client.Close();
                }

                if (recv_Count > 0)
                {
                    string recv_request = Encoding.UTF8.GetString(recv_Buffer, 0, recv_Count);
                    writeLog("Data Request : " + recv_request);  //将请求显示到界面

                    //Resolve(recv_request, web_client);  //解析、路由、处理

                    byte[] cont = pageHandle(RouteHandle(recv_request));
                    sendPageContent(cont, web_client);
                }



                //web_client.Close();
                //socket.Close();

                //NO.2  串行处理http请求
            }
            catch (Exception ex)
            {
                writeLog("处理http请求时出现异常！" + Environment.NewLine + "\t" + ex.Message);
            }
        }



        private void sendPageContent(byte[] pageContent, Socket response)
        {

            string statusline = "HTTP/1.1 200 OK\r\n";   //状态行
            byte[] statusline_to_bytes = Encoding.UTF8.GetBytes(statusline);

            string content =
            "<html>" +
                "<head>" +
                    "<title>socket webServer  -- Login</title>" +
                "</head>" +
                "<body>" +
                   "<div style=\"text-align:center\">" +
                       "欢迎您！" + "" + ",今天是 " + DateTime.Now.ToLongDateString() +
                   "</div>" +
                "</body>" +
            "</html>";  //内容


            byte[] content_to_bytes = pageContent;

            string header = string.Format("Content-Type:text/html;charset=UTF-8\r\nContent-Length:{0}\r\n", content_to_bytes.Length);
            byte[] header_to_bytes = Encoding.UTF8.GetBytes(header);  //应答头


            response.Send(statusline_to_bytes);  //发送状态行
            response.Send(header_to_bytes);  //发送应答头
            response.Send(new byte[] { (byte)'\r', (byte)'\n' });  //发送空行
            response.Send(content_to_bytes);  //发送正文（html）

            response.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string RouteHandle(string request)
        {
            string retRoute = "";
            string[] strs = request.Split(new string[] { "\r\n" }, StringSplitOptions.None);  //以“换行”作为切分标志
            if (strs.Length > 1)  //解析出请求路径、post传递的参数(get方式传递参数直接从url中解析)
            {
                string[] items = strs[0].Split(' ');  //items[1]表示请求url中的路径部分（不含主机部分）
                string pageName = items[1];
                string post_data = strs[strs.Length - 1]; //最后一项
                //Dictionary<string, string> dict = ParameterHandle(strs);

                retRoute = pageName + (post_data.Length > 0 ? "?" + post_data : "");
            }

            return retRoute;

        }

        /// <summary>
        /// 按照HTTP协议格式,解析浏览器发送的请求字符串
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        private Dictionary<string, string> ParameterHandle(string[] strs)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();

            if (strs.Length > 0)  //解析出请求路径、post传递的参数(get方式传递参数直接从url中解析)
            {
                if (strs.Contains(""))  //包含空行  说明存在post数据
                {
                    string post_data = strs[strs.Length - 1]; //最后一项
                    if (post_data != "")
                    {
                        string[] post_datas = post_data.Split('&');
                        foreach (string s in post_datas)
                        {
                            param.Add(s.Split('=')[0], s.Split('=')[1]);
                        }
                    }
                }
            }
            return param;
        }

        private byte[] pageHandle(string pagePath)
        {
            byte[] pageContent = null;
            webRoot = webRoot.TrimEnd('/', '\\');
            pagePath = pagePath.TrimEnd('/', '\\');
            if (pagePath.Length == 0)
            {
                foreach (string page in defaultPage.Split(','))
                {
                    if (System.IO.File.Exists(webRoot + page))
                    {
                        pagePath = page;
                        break;
                    }
                }
            }
            if (System.IO.File.Exists(webRoot + pagePath))
                pageContent = System.IO.File.ReadAllBytes(webRoot + pagePath);
            if (pageContent == null)
            {

                string content = notExistPage();
                pageContent = Encoding.UTF8.GetBytes(content);

            }
            return pageContent;
        }


        private void writeLog(string msg)
        {
            Console.WriteLine("  " + msg);
        }


        private string notExistPage()
        {
            string cont = @"<!DOCTYPE HTML>
                    <html>

                        <head>
                            <link rel='stylesheet' type='text/css' href='NewErrorPageTemplate.css' >

                            <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>
                            <title>This page can&rsquo;t be displayed</title>

                            <script src='errorPageStrings.js' language='javascript' type='text/javascript'>
                            </script>
                            <script src='httpErrorPagesScripts.js' language='javascript' type='text/javascript'>
                            </script>
                        </head>

                        <body onLoad='javascript:getInfo();'>
                            <div id='contentContainer' class='mainContent'>
                                <div id='mainTitle' class='title'>This page can&rsquo;t be displayed</div>
                                <div class='taskSection' id='taskSection'>
                                    <ul id='cantDisplayTasks' class='tasks'>
                                        <li id='task1-1'>Make sure the web address <span id='webpage' class='webpageURL'></span>is correct.</li>
                                        <li id='task1-2'>Look for the page with your search engine.</li>
                                        <li id='task1-3'>Refresh the page in a few minutes.</li>
                                    </ul>
                                    <ul id='notConnectedTasks' class='tasks' style='display:none'>
                                        <li id='task2-1'>Check that all network cables are plugged in.</li>
                                        <li id='task2-2'>Verify that airplane mode is turned off.</li>
                                        <li id='task2-3'>Make sure your wireless switch is turned on.</li>
                                        <li id='task2-4'>See if you can connect to mobile broadband.</li>
                                        <li id='task2-5'>Restart your router.</li>
                                    </ul>
                                </div>
                                <div><button id='diagnose' class='diagnoseButton' onclick='javascript:diagnoseConnectionAndRefresh(); return false;'>Fix connection problems</button></div>
                            </div>
                        </body>
                    </html>";

            return cont;
        }

    }
}