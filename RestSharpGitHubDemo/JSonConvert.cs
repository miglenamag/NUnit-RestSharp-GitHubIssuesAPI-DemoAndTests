using System;
using RestSharp;

internal class JSonConvert
{
    
    internal class DeserializeObject<T>
    {
        private RestResponse response;

        public DeserializeObject(RestResponse response)
        {
            this.response = response;
        }
    }
}