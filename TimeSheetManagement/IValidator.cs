using System;
using System.Collections.Generic;
using System.Text;

namespace TimeSheetManagement.Business
{
    /// <summary>
    /// A generic interface to be implmented 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IValidator<T>
    {
        bool Validate(T t);
    }
}
