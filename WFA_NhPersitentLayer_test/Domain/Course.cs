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
    public class Course<T>
        : VersionedEntity<int>
        where T : User
    {
        public virtual string Nominative { get; set; }
        public virtual T CurrentUser { get; set; }
    }


    //public class Course
    //    : VersionedEntity<int>
    //{
    //    public virtual string Nominative { get; set; }
    //    public virtual User CurrentUser { get; set; }
    //}

    public class DefinedCourse
        : Course<User>
    {
        
    }
}
