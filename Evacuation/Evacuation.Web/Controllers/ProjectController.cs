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
    public class ProjectController : Controller
    {
        ProjectService projectService;
        UserService userSevice;

        MapperConfiguration conf = new MapperConfiguration(cnf =>
        {           
            cnf.CreateMap<Project, ProjectModel>();
            cnf.CreateMap<ProjectModel, Project>();
        });

        public ProjectController(ProjectService ps, UserService us)
        {
            projectService = ps;
            userSevice = us;
        }
       
        public ActionResult Projects()
        {
            var map = conf.CreateMapper();
            IEnumerable<Project> pr = projectService.GetAll();
            IEnumerable<ProjectModel> pm = map.Map<IEnumerable<ProjectModel>>(pr);
            return View(pm.ToList());
        }

        [HttpGet]
        public ActionResult CreateProject()
        {            
            List<SelectListItem> users = new List<SelectListItem>();
            foreach(var item in userSevice.GetAll().ToList())
            {
                users.Add(new SelectListItem { Text = item.UserName, Value = item.UserID.ToString() });
            }
            SelectList PUsers = new SelectList(new List<SelectListItem> ( users ), "Value", "Text");
            ViewBag.Users = PUsers;           
            return View(new ProjectModel());
        }

        [HttpPost]
        public ActionResult CreateProject(ProjectModel projectM, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                var map = conf.CreateMapper();                
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                projectM.Image = imageData;
                projectM.DataCreation = DateTime.Now;
                Project project = map.Map<Project>(projectM);                
                projectService.Create(project);
                return RedirectToAction("Projects", "Project");
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditProject(int id)
        {
            var map = conf.CreateMapper();
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
        public ActionResult EditProject(ProjectModel projectM, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                var map = conf.CreateMapper();
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                projectM.Image = imageData;
                projectM.DataCreation = DateTime.Now;
                Project project = map.Map<Project>(projectM);
                projectService.Edit(project);
                return RedirectToAction("Projects", "Project");
            }
            return View();
        }



        [HttpGet]
        public ActionResult DeleteProject(int id)
        {
            var map = conf.CreateMapper();
            var projectDelete = projectService.Get(id);
            var projectM = map.Map<ProjectModel>(projectDelete);
            return View(projectM);
        }

        [HttpPost, ActionName("DeleteProject")]
        public ActionResult DeleteP(int id)
        {
            if (ModelState.IsValid)
            {
                var projectDelete = projectService.Get(id);
                projectService.Delete(projectDelete);
                return RedirectToAction("Projects", "Project");
            }
            return View();
        }
    }
}