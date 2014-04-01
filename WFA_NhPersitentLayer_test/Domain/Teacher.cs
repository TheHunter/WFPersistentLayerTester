using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WFA_NHibernate.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class Teacher
        : User
    {
        public virtual int CollegeCode { get; set; }
    }
}
