using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityModel;
using Iesi.Collections.Generic;
using PersistentLayer.Domain;

namespace WFA_NHibernate.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class UserManager<T>
        : VersionedEntity<int>
        where T: User
    {
        public virtual DateTime? BeginDate { get; set; }
        public virtual DateTime? FinalDate { get; set; }
        public Iesi.Collections.Generic.ISet<T> Users { get; set; }
    }
}
