using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WFA_NHibernate.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class Collaborator
        : User
    {
        public virtual string VatNumber { get; set; }
    }
}
