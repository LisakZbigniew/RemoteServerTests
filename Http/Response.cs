using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace RemoteServerTests.Http
{
    public class Response
    {
        private const double httpVersion = 1.1;
        private HttpStatusCode? status;
        private Dictionary<String,String> headers = new Dictionary<string, string>();
                private string? body;
        public Response(){
            //Add general info ablut host like ip
        }


        private string generateHeaders(){
            HttpStatusCode? code = status;
            if(code is null){
                code = HttpStatusCode.InternalServerError;
            }
            string headersStr = $"HTTP/{httpVersion} {(int)code} {code}";
            foreach(KeyValuePair<string,string> header in headers){
                headersStr += $"\n{header.Key} : {header.Value}";
            }
            return headersStr;
        }

        private string generateHeaders(HttpStatusCode responseCode){
            return generateHeaders();
        }

        public void OK(){
            status = HttpStatusCode.OK;
        }

        public void InternalErr(){
            status = HttpStatusCode.InternalServerError;
        }

        public string toString(){
            string rawResponseStr = generateHeaders();
            
            if(!(body is null)){
                rawResponseStr += "\n\n" + body;
            }
            
            return rawResponseStr;
        }
    }
}