using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using RemoteServerTests.Http;

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

        //Start server and listen for connection
        void startServer(){
            server.Start();
            listenConnection();
            Console.WriteLine($"Listening on port {port} ...");
        }

        private void listenConnection(){
            server.BeginAcceptTcpClient(handleClient,server);
        }



        

        private void handleClient(IAsyncResult result){
            listenConnection();

            TcpClient client = server.EndAcceptTcpClient(result);

            NetworkStream ns = client.GetStream();

            byte[] request = new byte[1024];
            
            ns.Read(request,0,request.Length);
        
            Request req = new Request(Encoding.UTF8.GetString(request));
            /*TODO CREATE Request HERE Move processing request logic to own class
                In the end there needs to be a nice obj with fields for http version, path, the body and for headers map
            */

            Response res = new Response();
            if(req.isValid){
                //TODO ask some generic server how they want to answer to this request get back a formatted Response obj  
                res.OK(); // just for now accept every request
            }else{
                res.InternalErr();
            }

            byte[] response = new byte[1024];
            response = Encoding.Default.GetBytes(res.toString());
            ns.Write(response,0,response.Length);
            ns.Close();
        }


    }
}