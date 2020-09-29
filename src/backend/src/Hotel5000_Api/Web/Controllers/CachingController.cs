using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Web.Controllers
{
    public abstract class CachingController : ControllerBase
    {
        protected IMemoryCache Cache { get; }
        protected CachingController(IMemoryCache cache)
        {
            Cache = cache;
        }
        protected string GetKey(params object[] args)
        {
            unchecked
            {
                int hash = 27;
                for (int i = 0; i < args.Length; i++)
                {
                    hash = (13 * hash) + (args[i] == null ? 1 : args[i].GetHashCode());
                }
                return hash.ToString();
            }
        }
    }
}
    