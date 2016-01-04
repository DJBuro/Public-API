using System;
using System.Web.Mvc;
using AndroAdmin.Dao;
using AndroAdmin.Dao.Domain;
using AndroAdmin.Models;
using AndroAdmin.Mvc;
using AndroAdmin.Mvc.Filters;

namespace AndroAdmin.Controllers
{
    public class AdminController : SiteController
    {
        public IAndroUserPermissionDao AndroUserPermissionDao { get; set; }
        public IProjectDao ProjectDao { get; set; }


        [RequiresAuthorisation]
        public ActionResult Index()
        {
            var data = new AndroAdminViewData.AdminViewData();
            var userId = Mvc.Utilities.Cookie.GetAuthoriationCookieUserId(Request);

            data.AndroUser = AndroUserDao.FindById(userId);

            data.AndroUsers = AndroUserDao.FindAll();

            return (View(AdminControllerViews.AllUsers, data));
        }

        [RequiresAuthorisation]
        public ActionResult AddUser()
        {
            var data = new AndroAdminViewData.AdminViewData();
            var userId = Mvc.Utilities.Cookie.GetAuthoriationCookieUserId(Request);

            data.AndroUser = AndroUserDao.FindById(userId);
            data.AndroEditUser = new AndroUser();

            return (View(AdminControllerViews.AddUser, data));
        }

        [RequiresAuthorisation]
        public ActionResult DeleteUser(int? id)
        {
            var data = new AndroAdminViewData.AdminViewData();
            var userId = Mvc.Utilities.Cookie.GetAuthoriationCookieUserId(Request);

            var androEditUser = AndroUserDao.FindById(id.Value);

            AndroUserDao.Delete(androEditUser);

            data.AndroUsers = AndroUserDao.FindAll();

            data.AndroUser = AndroUserDao.FindById(userId);

            return (View(AdminControllerViews.AllUsers, data));
        }

        [RequiresAuthorisation]
        public ActionResult EditUser(int? id)
        {
            var data = new AndroAdminViewData.AdminViewData();
            var userId = Mvc.Utilities.Cookie.GetAuthoriationCookieUserId(Request);

            data.AndroUser = AndroUserDao.FindById(userId);
            data.AndroEditUser = AndroUserDao.FindById(id.Value);

            return (View(AdminControllerViews.EditUser, data));
        }  
        
        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult Permissions(AndroUser androUser)
        {
            var data = new AndroAdminViewData.AdminViewData();
            var userId = Mvc.Utilities.Cookie.GetAuthoriationCookieUserId(Request);

            if (androUser.Id.HasValue)
            {
                data.AndroEditUser = AndroUserDao.FindById(androUser.Id.Value);
                
                if(UpdateModel(data.AndroEditUser))
                {
                    AndroUserDao.Save(data.AndroEditUser);
                }
            }
            else
            {
                androUser.Created = DateTime.Now;
                data.AndroEditUser = AndroUserDao.Create(androUser);
            }          
            
            data.AndroUser = AndroUserDao.FindById(userId);
            data.AndroUserPermissions = AndroUserPermissionDao.FindAll();

            data.Projects = ProjectDao.FindAll();

            return (View(AdminControllerViews.Permissions, data));
        }


        [RequiresAuthorisation]
        public ActionResult AddPermissions(int? id, int? trans)
        {
            var data = new AndroAdminViewData.AdminViewData();
            var userId = Mvc.Utilities.Cookie.GetAuthoriationCookieUserId(Request);

           data.AndroEditUser = AndroUserDao.FindById(id.Value);
            var project = ProjectDao.FindById(trans.Value);

            var permission = new AndroUserPermission(data.AndroEditUser, project);

            AndroUserPermissionDao.Save(permission);

            data.AndroUser = AndroUserDao.FindById(userId);
           data.AndroUserPermissions = AndroUserPermissionDao.FindAll();

            data.Projects = ProjectDao.FindAll();

            return (View(AdminControllerViews.Permissions, data));
        }

        [RequiresAuthorisation]
        public ActionResult RemovePermissions(int? id, int? trans)
        {
            var data = new AndroAdminViewData.AdminViewData();
            var userId = Mvc.Utilities.Cookie.GetAuthoriationCookieUserId(Request);

            data.AndroEditUser = AndroUserDao.FindById(id.Value);
            foreach (AndroUserPermission userPermission in data.AndroEditUser.UserPermissions)
            {
                if(userPermission.Project.Id == trans)
                {
                    AndroUserPermissionDao.Delete(userPermission);
                    break;
                }

            }
            data.AndroEditUser = AndroUserDao.FindById(id.Value);
            data.AndroUser = AndroUserDao.FindById(userId);
            data.AndroUserPermissions = AndroUserPermissionDao.FindAll();

            data.Projects = ProjectDao.FindAll();

            return (View(AdminControllerViews.Permissions, data));
        }

    }
}
