using System;
using System.Collections.Generic;
using System.Text;

namespace Taller.Models
{
    public class Response
    {
        public bool IsSuccess
        {
            get;
            set;
        }
        public String Message
        {
            get;
            set;
        }
        public Object Result
        {
            get;
            set;
        }
    }
}
