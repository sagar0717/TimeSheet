using System;
using System.Collections.Generic;
using System.Text;

namespace TimeSheetManagement.Business
{
    interface IValidator<T>
    {
        bool Validate(T t);
    }
}
