using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LDN.SelfWebServiceApp.WebService
{
    public class TcpListenerWebService
    {
        public void Start()
        {
            //取得本机的loopback网络地址，即127.0.0.1
            IPAddress ip = IPAddress.Loopback;
            //创建可以访问的网络端点，8888表示端口号
            IPEndPoint endpoint = new IPEndPoint(ip, 8888);
            //初始化tcp监听器
            /*
             * 传入监听的端点参数，通过构造函数初始化监听器，
             * 不再关注如何设置网络协议等细节，
             * 具体可以和上篇文章的方式进行对比。
             */
            TcpListener listener = new TcpListener(endpoint);
            //开启监听器
            listener.Start();
            //打印提示
            Console.WriteLine("监听开始......");
            while (true)
            {
                //AcceptTcpClient方法将阻塞进程，直到一个客户端的连接到达监听器，返回一个TcpClient类型的对象。可通过该对象与客户端进行通信
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("与客户端已经建立连接.....");
                //得到一个网络流，通过TcpClient可以得到一个用于输入和输出的网络流对象NetworkStream，对Socket的输入和输出进行了封装。
                NetworkStream ns = client.GetStream();
                //处理过程使用utf8 进行编码
                System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
                //设置用于接收的字节数组
                byte[] buffer = new byte[4096];
                int length = ns.Read(buffer, 0, buffer.Length);
                //得到请求内容
                string requestString = utf8.GetString(buffer, 0, length);
                //打印
                Console.WriteLine(requestString);
                //回应的状态行
                string statusLine = "HTTP/1.1 200 OK\r\n";
                byte[] statusLineBuffer = utf8.GetBytes(statusLine);
                //准备发送到客户端的网页
                string responseBody = "<html><head><title>这是一个web服务器的测试</title></head><body><h1>Hello World.</h1></body></html>";
                byte[] responseBodyBuffer = utf8.GetBytes(responseBody);
                //回应的头部
                string responseHeader = string.Format(
                    "Content-Type:text/html;charset=UTF-8\r\nContent-Length: {0}\r\n", responseBodyBuffer.Length);
                byte[] responseHeaderBuffer = utf8.GetBytes(responseHeader);
                //响应状态行
                ns.Write(statusLineBuffer, 0, statusLineBuffer.Length);
                //响应头部
                ns.Write(responseHeaderBuffer, 0, responseHeaderBuffer.Length);
                //输出头部与内容之间的空行
                ns.Write(new byte[] { 13, 10 }, 0, 2);
                //输出内容部分
                ns.Write(responseBodyBuffer, 0, responseBodyBuffer.Length);
                //关闭与客户端的连接
                client.Close();
                break;
            }
            //关闭服务器监听
            listener.Stop();
            Console.Read();
        }
    }
}