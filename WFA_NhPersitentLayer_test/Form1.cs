using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Context;
using System.Configuration;
using System.IO;
using System.Xml;
using NHibernate.Transform;
using PersistentLayer;
using PersistentLayer.Domain;
using PersistentLayer.NHibernate;
using PersistentLayer.NHibernate.Impl;
using WFA_NHibernate.Wrappers;

namespace WFA_NHibernate
{
    public partial class Form1 : Form
    {
        static ISessionFactory sessionFactory = null;
        NhConfigurationBuilder builder = null;
        INhPagedDAO ownPagedDAO = null;
        ISessionProvider sessionProvider = null;
        string rootPathProject = null;

        public Form1()
        {
            InitializeComponent();
            SetRootPathProject();

            XmlTextReader configReader = new XmlTextReader(new MemoryStream(WFA_NHibernate.Properties.Resources.Configuration));
            DirectoryInfo dir = new DirectoryInfo(this.rootPathProject + "MappingsXml");
            builder = new NhConfigurationBuilder(configReader, dir);

            builder.SetProperty("connection.connection_string", GetConnectionString());

            builder.BuildSessionFactory();
            sessionFactory = builder.SessionFactory;

            sessionProvider = new SessionManager(sessionFactory);

            ownPagedDAO = new EnterprisePagedDAO(sessionProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetRootPathProject()
        {
            var list = new List<string>(Directory.GetCurrentDirectory().Split('\\'));
            list.RemoveAt(list.Count - 1);
            list.RemoveAt(list.Count - 1);
            list.Add(string.Empty);
            this.rootPathProject = string.Join("\\", list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            string output = this.rootPathProject + "db\\";

            var str = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
            return string.Format(str, output);
        }

        #region PAGING
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int pageSize = Convert.ToInt32(txtPageSize.Text);
                int pageIndex = Convert.ToInt32(txtIndexPage.Text) - 1;

                BindSession();

                QueryOver<Salesman> query = QueryOver.Of<Salesman>().Where(n => n.ID > 10);
                IPagedResult<Salesman> paged = ownPagedDAO.GetPagedResult<Salesman>(pageSize * pageIndex, pageSize, query);

                txtCounter.Text = paged.Counter.ToString();
                dgv_Paging.DataSource = paged.GetResult();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            finally
            {
                UnBindSession();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int pageSize = Convert.ToInt32(txtPageSize.Text);
                int pageIndex = Convert.ToInt32(txtIndexPage.Text) - 1;

                BindSession();

                DetachedCriteria query = DetachedCriteria.For<Salesman>();
                query.Add(Restrictions.Gt("ID", (long)5));

                IPagedResult<Salesman> paged = ownPagedDAO.GetPagedResult<Salesman>(pageSize * pageIndex, pageSize, query);

                txtCounter.Text = paged.Counter.ToString();
                dgv_Paging.DataSource = paged.GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            finally
            {
                UnBindSession();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int pageSize = Convert.ToInt32(txtPageSize.Text);
                int pageIndex = Convert.ToInt32(txtIndexPage.Text) - 1;

                BindSession();

                IQueryable<Salesman> query = ownPagedDAO.ToIQueryable<Salesman>().Where(n => n.ID > 5);

                IPagedResult<Salesman> paged = ownPagedDAO.GetPagedResult<Salesman>(pageSize * pageIndex, pageSize, query);

                txtCounter.Text = paged.Counter.ToString();
                dgv_Paging.DataSource = paged.GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            finally
            {
                UnBindSession();
            }
        }

        #endregion

        #region
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                BindSession();
                IQuery query = ownPagedDAO.GetNamedQuery(txtNamedQuery.Text);

                dgvTransformerResult.DataSource = query.SetParameter(txtNP1.Text, txtPar1.Text).List<Salesman>();
                MessageBox.Show("Nessuna transformazione richiesta.");

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            finally
            {
                UnBindSession();
            }
        }
        
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                BindSession();
                string strQuery = richTxtBox.Text;
                IQuery query = ownPagedDAO.MakeHQLQuery(strQuery);

                AddParameters(query);

                //dgvTransformerResult.DataSource = query.List<Salesman>();
                dgvTransformerResult.DataSource = query.List();
                MessageBox.Show("Nessuna transformazione richiesta.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            finally
            {
                UnBindSession();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                BindSession();
                string strQuery = richTxtBox.Text;
                ISQLQuery query = ownPagedDAO.MakeSQLQuery(strQuery);

                AddParameters(query);

                query.AddEntity(typeof(Salesman));
                dgvTransformerResult.DataSource = query.List<Salesman>();
                MessageBox.Show("Nessuna transformazione richiesta.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            finally
            {
                UnBindSession();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        private void AddParameters(IQuery query)
        {
            if (!string.IsNullOrEmpty(txtNP1.Text) && !string.IsNullOrEmpty(txtPar1.Text))
            {
                query.SetParameter(txtNP1.Text , txtPar1.Text);
            }

            if (!string.IsNullOrEmpty(txtNP2.Text) && !string.IsNullOrEmpty(txtPar2.Text))
            {
                query.SetParameter(txtNP2.Text, txtPar2.Text);
            }

            if (!string.IsNullOrEmpty(txtNP3.Text) && !string.IsNullOrEmpty(txtPar3.Text))
            {
                query.SetParameter(txtNP3.Text, txtPar3.Text);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindSession()
        {
            CurrentSessionContext.Bind(sessionFactory.OpenSession());
        }

        /// <summary>
        /// 
        /// </summary>
        private void UnBindSession()
        {
            ISession session = CurrentSessionContext.Unbind(sessionFactory);
            if (session != null)
            {
                session.Close();
            }
        }

        #region
        private void btnLoader_Click(object sender, EventArgs e)
        {
            try
            {
                BindSession();
                Salesman cons = ownPagedDAO.FindBy<Salesman, long?>(Convert.ToInt64(txtLoader.Text), LockMode.Read);

                MessageBox.Show(cons.ToString(), "All right");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            finally
            {
                UnBindSession();
            }
        }


        #endregion

        private void button9_Click(object sender, EventArgs e)
        {
            ISession session = sessionFactory.OpenSession();
            try
            {
                //BindSession();
                //sessionProvider.BeginTransaction(IsolationLevel.ReadCommitted);
                
                //(1)
                QueryOver<Salesman> query = QueryOver.Of<Salesman>()
                    .Where(n => n.ID > 1)
                    //.JoinQueryOver()
                    //.Select(n => n.ID, n => n.Name, n => n.Agents, n => n.Email, n => n.Surname) //genera un errore..
                    
                    //.Select(n => n.ID, n => n.Name, n => n.Surname, n => n.Email)
                    .Select(
                            Projections.ProjectionList()
                            .Add(Projections.Property("ID"), "ID")
                            .Add(Projections.Property("Name"), "Name")
                            .Add(Projections.Property("Surname"), "Surname")
                            .Add(Projections.Property("Email"), "Email")
                            //.Add(Projections.Property("Agents"), "Agents")
                            )
                    ;
                
                var queryExec = query.GetExecutableQueryOver(session).TransformUsing(Transformers.AliasToBean<SalesmanPrj>());
                //var queryExec2 = query.GetExecutableQueryOver(session);

                //var ddd = queryExec2.Future<object[]>();
                //var ccc = queryExec2.List<object[]>();
                //var tot = queryExec2.ToRowCountQuery().FutureValue<int>().Value;
                var aaa = queryExec.Future<SalesmanPrj>(); 
                var bbb = queryExec.ToRowCountQuery().FutureValue<int>();
                
                //int total = bbb.Value;
                //ConsultantPrj temp = aaa.First();

                //MessageBox.Show(tot.ToString()); //no future
                MessageBox.Show(bbb.Value.ToString());

                //sessionProvider.CommitTransaction();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            finally
            {
                //UnBindSession();
                session.Close();
            }
        }
        
    }
}
