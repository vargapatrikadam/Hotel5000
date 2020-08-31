using BlazorWebApp.Interfaces;
using BlazorWebApp.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebApp.Services.Domain
{
    public class LodgingService : ILodgingService
    {
        private readonly HttpService _httpService;
        private string _apiUrl;
        public LodgingService(HttpService httpService,
            BaseUrlConfiguration baseUrlConfiguration)
        {
            _httpService = httpService;
            _apiUrl = baseUrlConfiguration.ApiBase;
        }

        public async Task<List<Lodging>> List()
        {
            var lodgings = await _httpService.HttpGet<List<Lodging>>("lodgings");
            var items = lodgings;
            return items;
        }
    }
}
