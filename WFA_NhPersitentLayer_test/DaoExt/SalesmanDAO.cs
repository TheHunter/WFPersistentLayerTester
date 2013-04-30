using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Transform;
using NHibernate.Criterion;
using NHibernate;
using PersistentLayer.Domain;
using PersistentLayer.NHibernate;
using PersistentLayer.NHibernate.Impl;
using WFA_NHibernate.Wrappers;

namespace WFA_NHibernate.DaoExt
{

    public class SalesmanDAO
        : BusinessPagedDAO<Salesman, long>
    {

        public SalesmanDAO(ISessionProvider sessionProvider)
            : base(sessionProvider)
        {
            
        }

        public IEnumerable<Salesman> GetInstancesWhithIDBiggerThan(long ID)
        {
            return this.CurrentSession.GetNamedQuery("InstancesByID").SetParameter("ID", ID).List<Salesman>();
        }

        public IEnumerable<Salesman> GetInstancesBiggerThan(long code)
        {
            return this.CurrentSession.GetNamedQuery("ConsultantsQueryByCode").SetParameter("code", code).List<Salesman>();
        }

        public IEnumerable<Salesman> GetConsByDataRif_func(DateTime datarif)
        {
            return this.CurrentSession.GetNamedQuery("GetConsByDataFunc")
                .SetDateTime("datarif", datarif)
                .List<Salesman>();
        }

        public IEnumerable<Salesman> SetConsultantByName(string name)
        {
            return this.CurrentSession.GetNamedQuery("SetConsultantByName").SetParameter("name", name).List<Salesman>();
        }

        public IEnumerable<Salesman> SPSetConsultantByName(string name)
        {
            return this.CurrentSession.GetNamedQuery("SPSetConsultantByName").SetParameter("name", name).List<Salesman>();
        }

        public IEnumerable<ReportSalesman> GetRepConsultant()
        {
            return this.CurrentSession.GetNamedQuery("RepConsultant").SetResultTransformer(Transformers.AliasToBean<ReportSalesman>()).List<ReportSalesman>();
        }

        public IEnumerable<ReportSalesman> GetReportWithLinq()
        {
            var res = this.ToIQueryable().Select(n => new ReportSalesman { ID = n.ID.Value, Name = n.Name, Surname = n.Surname, NumSubAgents = n.Agents.Count });
            return res.ToList();
        }

        public IEnumerable<Salesman> GetConsultantsByExample(Salesman instance)
        {
            return this.CurrentSession.CreateCriteria<Salesman>()
                .Add(Example.Create(instance))
                .List<Salesman>();
        }

        public IQueryOver<Salesman> GetQueryOverByExample(Salesman consultant)
        {
            IQueryOver<Salesman> query = QueryOver.Of<Salesman>().And(Example.Create(consultant));
            return query;
        }


    }
}
