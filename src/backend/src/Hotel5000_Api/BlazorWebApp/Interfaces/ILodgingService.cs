using BlazorWebApp.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebApp.Interfaces
{
    public interface ILodgingService
    {
        Task<List<Lodging>> List();
    }
}
