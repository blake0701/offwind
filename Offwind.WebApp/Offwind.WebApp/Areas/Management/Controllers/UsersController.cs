﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Offwind.WebApp.Areas.Management.Models;
using Offwind.WebApp.Models.Account;

namespace Offwind.WebApp.Areas.Management.Controllers
{
    public class UsersController : _BaseController
    {
        private List<VUserProfile> _model;

        public ActionResult Index()
        {
            var model = new VUsersHome();
            model.Users = _ctx.DVUserProfiles.OrderBy(x => x.FirstName).Select(VUserProfile.MapFromDb).ToList();
            return View(model);
        }

        public ActionResult Details(string userName)
        {
            var model = _ctx.DVUserProfiles.FirstOrDefault(x => x.UserName == userName);
            return View(model);
        }

        public FileResult DownloadExcel()
        {
            return File(new byte[0], "");
        }
        //public ActionResult Edit(int id)
        //{
        //    var db = _ctx.DUserProfiles;
        //    var usr = Enumerable.FirstOrDefault(
        //        db.Where(x => x.UserId == id).Select(x => new VUserProfile()
        //                                                      {
        //                                                          Id = x.UserId,
        //                                                          UserName = x.UserName,
        //                                                      }));
        //    if (usr != null)
        //    {
        //        var roles = (SimpleRoleProvider) Roles.Provider;
        //        usr.SelectRoles(roles.GetRolesForUser(usr.UserName));
        //        usr.OldPassword = Membership.GeneratePassword(6, 0);
        //        usr.Password = usr.ConfirmPassword = usr.OldPassword;
        //    }
        //    return View(usr);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(VUserProfile model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var roles = (SimpleRoleProvider) Roles.Provider;
        //        roles.RemoveUsersFromRoles(new[] {model.UserName}, model.SelectedRoles.Split(';'));
        //        model.SelectRoles();
        //        roles.AddUsersToRoles(new[] {model.UserName}, model.SelectedRoles.Split(';'));

        //        var membershipUser = Enumerable.FirstOrDefault(_ctx.webpages_Membership.Where(x => x.UserId == model.Id));
        //        if (membershipUser != null)
        //        {
        //            if (model.Password != model.OldPassword)
        //            {
        //                var oldPas = membershipUser.Password;
        //                WebSecurity.ChangePassword(model.UserName, oldPas, model.Password);
        //            }
        //        }               
        //        return RedirectToAction("Index");    
        //    }
        //    return View(model);
        //}

        //public ActionResult Add()
        //{
        //    var model = new VUserProfile();
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Add(VUserProfile model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
        //        var roles = (SimpleRoleProvider)Roles.Provider;
        //        model.SelectRoles();
        //        roles.AddUsersToRoles(new[] {model.UserName}, model.SelectedRoles.Split(';'));
        //        return RedirectToAction("Index");
        //    }
        //    return View(model);
        //}

        //[HttpPost]
        //public JsonResult Delete(int id)
        //{
        //    var usr = Enumerable.FirstOrDefault(_ctx.DUserProfiles.Where(x => x.UserId == id));
        //    if (usr != null)
        //    {
        //        Membership.DeleteUser(usr.UserName);
        //    }
        //    return Json("OK");
        //}        
    }
}
