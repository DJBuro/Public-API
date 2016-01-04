using MyAndromeda.Data.MenuDatabase.Models.Database.MenuTableAdapters;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Web.Common;
using MyAndromeda.Data.MenuDatabase.Context;
using Ninject;
using MyAndromeda.Framework.Contexts;

namespace MyAndromeda.Data.MenuDatabase
{
    public class AccessMenuDatabaseNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Kernel.Bind<MenuConnectionStringContext>().ToMethod((context) =>
            {
                var workContextWrapper = context.Kernel.Get<WorkContextWrapper>();
                if (workContextWrapper.Available) {
                    if (workContextWrapper.Current.CurrentSite.Available) { 
                        return new MenuConnectionStringContext(workContextWrapper.Current.CurrentSite.SiteId);
                    }
                }

                return null;
            }).InTransientScope();

            this.Kernel.Bind<n_MenuTableAdapter>().ToMethod(context => {
                var currentSite = context.Kernel.Get<ICurrentSite>();
                var connection = new MenuConnectionStringContext(currentSite.AndromediaSiteId);
                var adaptor = new n_MenuTableAdapter() {
                    Connection = new System.Data.OleDb.OleDbConnection(connection.ConnectionString)
                };
                return adaptor;
            }).InRequestScope();

            this.Kernel.Bind<w_MenuSectionsTableAdapter>().ToMethod(context => {
                var currentSite = context.Kernel.Get<ICurrentSite>();
                var connection = new MenuConnectionStringContext(currentSite.AndromediaSiteId);
                var adaptor = new w_MenuSectionsTableAdapter() {
                    Connection = new System.Data.OleDb.OleDbConnection(connection.ConnectionString)
                };
                return adaptor;
            }).InRequestScope();

            this.Kernel.Bind<n_SecondaryCatTableAdapter>().ToMethod(context => {
                var currentSite = context.Kernel.Get<ICurrentSite>();
                var connection = new MenuConnectionStringContext(currentSite.AndromediaSiteId);
                var adaptor = new n_SecondaryCatTableAdapter() {
                    Connection = new System.Data.OleDb.OleDbConnection(connection.ConnectionString)
                };
                return adaptor;
            }).InRequestScope();

            this.Kernel.Bind<n_PrimaryCatTableAdapter>().ToMethod(context => {
                var currentSite = context.Kernel.Get<ICurrentSite>();
                var connection = new MenuConnectionStringContext(currentSite.AndromediaSiteId);
                var adaptor = new n_PrimaryCatTableAdapter(){
                    Connection = new System.Data.OleDb.OleDbConnection(connection.ConnectionString)
                };
                return adaptor;
            }).InRequestScope();

            this.Kernel.Bind<n_GroupsTableAdapter>().ToMethod(context => {
                var currentSite = context.Kernel.Get<ICurrentSite>();
                var connection = new MenuConnectionStringContext(currentSite.AndromediaSiteId);
                var adaptor = new n_GroupsTableAdapter()
                {
                    Connection = new System.Data.OleDb.OleDbConnection(connection.ConnectionString)
                };
                return adaptor;
            }).InRequestScope();

        }
    }
}
