using AutoMapper;
using Evacuation.Bll.Services;
using Evacuation.Dal.Entities;
using Evacuation.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evacuation.Web.Controllers
{
    public class LogInController : Controller
    {
        UserService userSevice;
        ProjectService projectService;

        MapperConfiguration con = new MapperConfiguration(cf =>
        {
            cf.CreateMap<User, UserModel>();
            cf.CreateMap<UserModel, User>();
            cf.CreateMap<Project, ProjectModel>();
            cf.CreateMap<ProjectModel, Project>();
        });

        public LogInController(UserService us, ProjectService ps)
        {
            userSevice = us;
            projectService = ps;
        }

        // GET: LogIn
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(UserModel user)
        {
            if (user != null)
            {
                var userlogin = userSevice.Get(userSevice.GetUserId(user.Email));
                Session["LogedUserId"] = userlogin.UserID.ToString();
                Session["LogedUserName"] = userlogin.UserName;
                Session["LogedUserFullName"] = userlogin.FirstName + " " + userlogin.LastName;
                return RedirectToAction("AfterLogIn");
            }
            return View();
        }

        public ActionResult AfterLogIn()
        {
            if (Convert.ToInt32(Session["LogedUserId"]) == 1)
                return RedirectToAction("AdminPage", "Home");
            else
            {
                var map = con.CreateMapper();
                int id = Convert.ToInt32(Session["LogedUserId"]);
                UserModel user = map.Map<UserModel>(userSevice.Get(id));
                List<Project> pr = projectService.GetProjects(id);
                List<ProjectModel> projects = map.Map<List<ProjectModel>>(pr);
                ViewBag.Project = projects;                
                return View(user);
            }         
        }      

        [HttpGet]
        public ActionResult UserPage()
        {
            var map = con.CreateMapper();
            int id = Convert.ToInt32(Session["LogedUserId"]);
            UserModel user = map.Map<UserModel>(userSevice.Get(id));
            List<Project> pr = projectService.GetProjects(id);
            List<ProjectModel> projects = map.Map<List<ProjectModel>>(pr);
            ViewBag.Project = projects;
            return View(user);
        }

        [HttpPost]        
        public ActionResult UserPage(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var map = con.CreateMapper();
                var userEdit = userSevice.Get(user.UserID);
                userEdit = map.Map<User>(user);
                userSevice.Edit(userEdit);
                return RedirectToAction("UserPage", "LogIn");
            }
            return View();
        }    

        [HttpGet]
        public ActionResult CreateP()
        {          
            return View(new ProjectModel());
        }

        [HttpPost]
        public ActionResult CreateP(ProjectModel projectM, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                var map = con.CreateMapper();
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                projectM.Image = imageData;
                projectM.DataCreation = DateTime.Now;
                projectM.UserID = Convert.ToInt32(Session["LogedUserId"]);
                Project project = map.Map<Project>(projectM);
                projectService.Create(project);
                return RedirectToAction("Projects", "LogIn");
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditP(int id)
        {
            var map = con.CreateMapper();
            var projectEdit = projectService.Get(id);
            var projectM = map.Map<ProjectModel>(projectEdit);
            List<SelectListItem> users = new List<SelectListItem>();
            foreach (var item in userSevice.GetAll().ToList())
            {
                users.Add(new SelectListItem { Text = item.UserName, Value = item.UserID.ToString() });
            }
            SelectList PUsers = new SelectList(new List<SelectListItem>(users), "Value", "Text");
            ViewBag.Users = PUsers;
            return View(projectM);
        }

        [HttpPost]
        public ActionResult EditP(ProjectModel projectM, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                var map = con.CreateMapper();
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                projectM.Image = imageData;
                projectM.DataCreation = DateTime.Now;
                Project project = map.Map<Project>(projectM);
                projectService.Edit(project);
                return RedirectToAction("UserPage", "LogIn");
            }
            return View();
        }

        [HttpGet]
        public ActionResult DeleteP(int id)
        {
            var map = con.CreateMapper();
            var projectDelete = projectService.Get(id);
            var projectM = map.Map<ProjectModel>(projectDelete);
            return View(projectM);
        }

        [HttpPost, ActionName("DeleteP")]
        public ActionResult DeletePr(int id)
        {
            if (ModelState.IsValid)
            {
                var projectDelete = projectService.Get(id);
                projectService.Delete(projectDelete);
                return RedirectToAction("UserPage", "LogIn");
            }
            return View();
        }

        public ActionResult Projects()
        {
            var map = con.CreateMapper();
            int id = Convert.ToInt32(Session["LogedUserId"]);
            UserModel user = map.Map<UserModel>(userSevice.Get(id));
            List<Project> pr = projectService.GetProjects(id);
            List<ProjectModel> projects = map.Map<List<ProjectModel>>(pr);
            ViewBag.Project = projects;
            return View();
        }

        [HttpGet]
        public ActionResult DetailsP(int id)
        {
            var map = con.CreateMapper();
            var projectDetails = projectService.Get(id);
            var projectD = map.Map<ProjectModel>(projectDetails);
            Session["Choice"] = 0;          
            return View(projectD);
        }

        [HttpPost]
        public ActionResult DetailsP(ProjectModel project, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                var map = con.CreateMapper();
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                var prEdit = projectService.Get(project.ProjectID);
                prEdit = map.Map<Project>(project);
                prEdit.Image = imageData;
                projectService.Edit(prEdit);
                return RedirectToAction("DetailsP", "LogIn");
            }
            return View();
        }

        
        public ActionResult DetailsPr(int id)
        {
            if (ModelState.IsValid)
            {
                var projectDelete = projectService.Get(id);
                projectService.Delete(projectDelete);
                return RedirectToAction("Projects", "LogIn");
            }
            return View();
        }




        }
}