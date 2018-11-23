using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bizchat.Core.Verbs
{
    public interface IVerb<in InputType>
    {
        Task Run(InputType inputType);
    }
}
