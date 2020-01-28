using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IOption<T>
    {
        T option { get; }
    }
}
