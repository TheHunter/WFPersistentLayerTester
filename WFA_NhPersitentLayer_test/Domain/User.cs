using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityModel;

namespace WFA_NHibernate.Domain
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class User
        : VersionedEntity<int>
    {
        public virtual string Nick { get; set; }
        public virtual string Name { get; set; }
    }
}
