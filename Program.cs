using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace RemoteServerTests
{   
    class RemoteServer
    {
        static void Main(string[] args){
            RemoteServer serv = new RemoteServer(3600);
            serv.startServer();
            Console.ReadKey(true);
        }

        TcpListener server;
        int port;
        RemoteServer(int port){
            this.port = port;
            server = new TcpListener(IPAddress.Any,port);
        }

        void startServer(){
            server.Start();
            listenConnection();
            Console.WriteLine($"Listening on port {port} ...");
        }

        private void listenConnection(){
            server.BeginAcceptTcpClient(handleClient,server);
        }


        private string generateHeaders(HttpStatusCode responseCode,Dictionary<string,string> options){
            string response = $"HTTP/1.1 {(int)responseCode} {responseCode}";
            
            return response;
        }

        private string generateHeaders(HttpStatusCode responseCode){
            return generateHeaders(responseCode,new Dictionary<string,string>());
        }

        private string processRequest(string request){

            try{
                string[] requestParts = request.Trim().Split(Environment.NewLine+Environment.NewLine);
                string[] headers = requestParts.FirstOrDefault<string>("").Trim().Split("\n");
                string body = requestParts.Length > 1 ? requestParts[1] : "";
                Console.WriteLine($"Recieved a request with body {requestParts[1]}");
            }catch(Exception e){
                string response = generateHeaders(HttpStatusCode.InternalServerError);
                return response;
            }
            return generateHeaders(HttpStatusCode.OK);
        }

        private void handleClient(IAsyncResult result){
            listenConnection();

            TcpClient client = server.EndAcceptTcpClient(result);

            NetworkStream ns = client.GetStream();

            byte[] request = new byte[1024];
            
            ns.Read(request,0,request.Length);
            string req = Encoding.UTF8.GetString(request);
            Console.WriteLine($"Recieved a request {req}");
            string res = processRequest(req);
            byte[] response = new byte[1024];
            Console.WriteLine($"Response: {res}");
            response = Encoding.Default.GetBytes(res);
            ns.Write(response,0,response.Length);
            ns.Close();
        }


    }
}