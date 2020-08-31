using BlazorWebApp.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebApp.Interfaces
{
    public interface ILodgingService
    {
        Task<List<Lodging>> List();
    }
}
