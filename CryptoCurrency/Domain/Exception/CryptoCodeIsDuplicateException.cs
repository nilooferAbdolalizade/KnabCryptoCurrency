using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exception
{
    public class CryptoCodeIsDuplicateException : BusinessException
    {
        public CryptoCodeIsDuplicateException() :
            base(1001)
        {
        }

        public override string GetTranslate()
        {
            return ExceptionResource.Exception_1001;
        }
    }
}
