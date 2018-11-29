﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapstoneProject.Models;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace CapstoneProject.Controllers
{
    [Authorize]
    public class GroupController : BaseController
    {
        PMSEntities db = new PMSEntities();
        // GET: Group
        public ActionResult Index()
        {
            string user = User.Identity.GetUserName();
            Session["UserName"] = user;
            var viewlistworkspace = db.WorkSpaces.OrderByDescending(m => m.ID).ToList();
            return View(viewlistworkspace);
        }
        public ActionResult CreateGroup()
        {
            if (Request.IsAuthenticated)
            {
                string user = User.Identity.GetUserName();
                Session["UserName"] = user;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult CreateGroup(WorkSpace ws)
        {
            ws.Createdate = DateTime.Now;
            WS_User_Roles wsp = new WS_User_Roles();
            string userid = User.Identity.GetUserId();
            Session["UserId"] = userid;
            wsp.User_ID = userid;
            wsp.Role_Admin = true;
            wsp.Role_Member = true;
            db.WS_User_Roles.Add(wsp);
            db.WorkSpaces.Add(ws);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteGroup(WorkSpace ws)
        {
            WorkSpace worksp = db.WorkSpaces.Find(ws.ID);
            db.WorkSpaces.Remove(worksp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ShowGroup(int? id)
        {
            WorkSpace showws = db.WorkSpaces.Find(id);
            if (showws == null)
            {
                return HttpNotFound();
            }
            return View(showws);
        }

        public ActionResult AddMemberWS(int? id)
        {
            WorkSpace addmem = db.WorkSpaces.Find(id);
            return View(addmem);
        }

        [HttpPost]
        public ActionResult AddMemberWS(WorkSpace model, List<string> adduser)
        {
            WorkSpace wp = db.WorkSpaces.Find(model.ID);
            foreach (var user in adduser)
            {
                WS_User_Roles wsuser = new WS_User_Roles();
                wsuser.User_ID = db.AspNetUsers.SingleOrDefault(x => x.Email == user).Id;
                wsuser.WorkSpace_ID = wp.ID;
                wsuser.Role_Member = true;
                db.WS_User_Roles.Add(wsuser);
            }
            db.SaveChanges();
            return RedirectToAction("AddMemberWS", new { id = model.ID });
        }

        public ActionResult SettingGroup(int? id)
        {
            WorkSpace editws = db.WorkSpaces.Find(id);
            return View(editws);
        }

        [HttpPost]
        public ActionResult SettingGroup(WorkSpace model)
        {
            WorkSpace ws = db.WorkSpaces.Find(model.ID);
            ws.WorkSpaceName = model.WorkSpaceName;
            ws.Description = model.Description;
            db.SaveChanges();
            return RedirectToAction("ShowWorkSpace", new { id = model.ID });
        }

        public ActionResult EditRoleMemberWS(int? id)
        {
            WS_User_Roles ws = db.WS_User_Roles.Find(id);
            return View(ws);
        }

        [HttpPost]
        public ActionResult EditRoleMemberWS(WS_User_Roles model, List<string> chk1)
        {
            WS_User_Roles wsp = db.WS_User_Roles.Find(model.ID);
            wsp.Role_Admin = model.Role_Admin;
            wsp.Role_Manager = model.Role_Manager;
            wsp.Role_Member = model.Role_Member;

            db.Entry(wsp).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AddMemberWS", new { id = wsp.WorkSpace_ID });
        }
    }
}