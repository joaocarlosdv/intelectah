using ApiExterna.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApiExterna.Services
{
    public class CorreiosService : ICorreiosService
    {
        private async Task<string> getWebServiceAsync(string url)
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)await myHttpWebRequest.GetResponseAsync();
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            Encoding encode = Encoding.GetEncoding("utf-8");
            StreamReader readStream = new StreamReader(receiveStream, encode);
            Char[] read = new Char[256];

            int count = readStream.Read(read, 0, 256);

            string result = "";
            while (count > 0)
            {
                String str = new String(read, 0, count);
                count = readStream.Read(read, 0, 256);
                result += str;
            }
            myHttpWebResponse.Close();
            readStream.Close();

            return result;
        }
        public async Task<Cep> ConsultarEndereco(string cep)
        {
            string result = await getWebServiceAsync("https://viacep.com.br/ws/" + cep + "/json/");
            JsonSerializerSettings serSettings = new JsonSerializerSettings();
            serSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            try
            {
                Cep retorno = JsonConvert.DeserializeObject<Cep>(result);
                return retorno;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Cep();
            }
        }
    }
}
