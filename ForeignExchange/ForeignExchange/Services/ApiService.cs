﻿
namespace ForeignExchange.ApiServices
{
    using ForeignExchange.Helpers;
    using ForeignExchange.Models;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class ApiService
    {
        public async Task<Response> CheckConnection()
        {
            var result = new Response();
            result.isSucess = false;
            if (!CrossConnectivity.Current.IsConnected)
            {
                result.Message = Languages.InternetError;
            }

            var response = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if (!response)
            {
                result.Message = Languages.InternetSettingsError;
            }
            else
            {
                result.isSucess = true;
            }

            return result;
        }

        public async Task<Response> GetList<T>(string urlBase, string controller)
        {
            
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        isSucess = false,
                        Message = result
                    };
                }
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    isSucess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    isSucess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
