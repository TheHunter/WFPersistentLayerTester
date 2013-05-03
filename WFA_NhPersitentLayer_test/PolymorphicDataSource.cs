using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Type;

namespace WFA_NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    public class PolymorphicDataSource
    {
        private string[] aliases;
        private IType[] aliasTypes;

        /// <summary>
        /// 
        /// </summary>
        public PolymorphicDataSource()
        {

        }

        private IList GetDataSource(IQuery query)
        {
            if (query == null)
                throw new ArgumentNullException("query", "The query object to execute cannot be null.");

            IList lista = query.List();
            if (lista == null || lista.Count == 0)
                return null;

            this.Reset();
            
            return lista;
        }

        public object MakeDataSource(IQuery query)
        {
            IList lista = this.GetDataSource(query);

            if (lista == null)
                return null;

            this.aliases = query.ReturnAliases;
            this.aliasTypes = query.ReturnTypes;

            Type objType = lista[0].GetType();
            if (objType.IsArray && this.aliases.Length == this.aliasTypes.Length)
            {
                return GetDataSourceTable(lista);
            }
            return lista;
        }

        public object MakeDataSource(ISQLQuery query)
        {
            IList lista = this.GetDataSource(query);

            if (lista == null)
                return null;

            object firstEl = lista[0];
            Type objType = firstEl.GetType();

            if (objType.IsArray)
            {
                object[] columns = firstEl as object[];
                if (columns == null)
                    return null;

                this.aliases = new string[columns.Length];
                this.aliasTypes = new IType[columns.Length];

                for (int index = 0; index < columns.Length; index++)
                {
                    this.aliases[index] = (index + 1).ToString();
                    this.aliasTypes[index] = NHibernateUtil.Object;
                }
                return this.GetDataSourceTable(lista);
            }
            return lista;
        }

        private DataTable GetDataSourceTable(IList lista)
        {
            DataTable dataSource = new DataTable();
            for (int index = 0; index < this.aliases.Length; index++)
            {
                dataSource.Columns.Add(this.aliases[index], this.aliasTypes[index].ReturnedClass);
            }

            foreach (object[] current in lista)
            {
                dataSource.Rows.Add(current);
            }

            return dataSource;
        }

        private void Reset()
        {
            this.aliases = null;
            this.aliasTypes = null;
        }
    }
}
