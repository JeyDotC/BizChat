using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bizchat.Core.Verbs
{
    public interface IVerb<in InputType, TResult>
    {
        Task<TResult> Run(InputType inputType);
    }
}
