using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exception
{
    public abstract class BusinessException : SystemException
    {

        protected BusinessException(int code)
        {
            Code = code;
        }

        public int Code { get; set; }

        public abstract string GetTranslate();

        public override string Message => GetTranslate();
    }
}
