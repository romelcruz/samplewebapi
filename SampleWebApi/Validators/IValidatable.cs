using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApi.Validators
{
    public interface IValidatable<T>
    {
        bool IsValid();
        string ErrorMessage { get; set; }
    }
}
