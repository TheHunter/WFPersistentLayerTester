using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WFA_NHibernate.Wrappers
{
    public class SalesmanPrj
    {
        public virtual long? ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Email { get; set; }
        //public ISet<Salesman> Agents { get; set; }
    }
}
