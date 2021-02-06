using ProgrammersBlog.Shared.Utilities.Resuslts.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Shared.Utilities.Resuslts.Abstract
{
    public interface IResult
    {
        public ResultStatus ResultStatus { get;  }
        public string Message { get;  }
        public Exception Exception { get; }


    }
}
