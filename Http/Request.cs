using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteServerTests.Http
{
    public class Request
    {
        private string raw;
        private HttpMethod? method;
        private double? httpVersion;
        private string? body;
        private Dictionary<String,String> headers = new Dictionary<string, string>();

        private bool _isValid = true;
        public bool isValid {get{
            return _isValid;
        }}

        public Request(string rawStr){
            this.raw = rawStr;
            processRawRequest();
        }

        private void processRawRequest(){

            try{
                string[] requestParts = raw.Trim().Split(Environment.NewLine+Environment.NewLine);
                string[] headerString = requestParts.FirstOrDefault<string>("").Trim().Split(Environment.NewLine);

                body = requestParts.Length > 1 ? requestParts[1] : "";
                Console.WriteLine($"Recieved a request with body {body}");

                

            }catch(Exception e){
                _isValid = false;
            }

        }

    }
}