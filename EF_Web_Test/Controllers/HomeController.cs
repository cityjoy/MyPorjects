using AutoMapper;
using EF_Web_Test.Models;
using EF_Web_Test.Models.DTO;
using EF_Web_Test.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EF_Web_Test.Log4net;
using Webdiyer.WebControls.Mvc;

namespace EF_Web_Test.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        private Repository<Subject> subjectRepository;
        public HomeController()
        {
            subjectRepository = unitOfWork.Repository<Subject>();

        }
        public ActionResult Index(int? id, string title,string author)
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            int pageIndex = id?? 1;
            int pageSize = 2;
            //List<Subject> subjectList = subjectRepository.GetPageEntities(pageIndex, pageSize,"SubjectId",true, out totalCount);
            var qury = subjectRepository.Entities.AsQueryable();
            if (!string.IsNullOrWhiteSpace(title))
               qury =qury.Where(a => a.Title.Contains(title));
            if (!string.IsNullOrWhiteSpace(author))
               qury =qury.Where(a => a.Author.Contains(author));
            var model =qury.OrderByDescending(a => a.CreateTime).ToPagedList(pageIndex, pageSize);
           //var temp= subjectRepository.Entities.SqlQuery<SubjectDTO>("select * from Subject");
            //List<SubjectDTO> subjectdtoList = Mapper.DynamicMap<List<SubjectDTO>>(subjectList);

            //PagedList<SubjectDTO> model = subjectdtoList.ToPagedList(pageIndex, pageSize);
            
            //model.TotalItemCount = totalCount;
            // model.CurrentPageIndex = pageIndex;
            if (Request.IsAjaxRequest())
                return PartialView("_SubjectList", model);
            //using (EFDBContext EFDBContext = new EFDBContext())
            //{
            //    // item = EFDBContext.Set<Subject>().AsNoTracking().SingleOrDefault(s=>s.SubjectId==1);
            //    EFDBContext.Configuration.ProxyCreationEnabled = false;
            //    dynamic subject = EFDBContext.Subjects.AsNoTracking().Where(o => o.SubjectId == 1).Select(s => new { s.SubjectId, s.Title, s.CommentList }).First();
            //    SubjectDTO subjectdto = Mapper.DynamicMap<SubjectDTO>(subject);
            //    //var list = EFDBContext.Set<SubjectComment>().Where(s=>s.SubjectId==1).Project().To<SubjectCommentDTO>().ToList();
            //    ViewBag.SubjectDTO = subjectdto;
            //    ViewBag.SubjectCommentDTOList = subjectdto.CommentList;
            //}
            //LogHelper.WriteError("123");
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Create()
        {
            

            return View();
        }


        public ActionResult AjaxSearchHtmlGet( string title, string author,int id=1)
        {
            return ajaxSearchGetResult(title, author, id);
        }
        private ActionResult ajaxSearchGetResult(string title, string author, int id = 1)
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            int pageIndex = id;
            int pageSize = 2;
            var qury = subjectRepository.Entities.AsQueryable();
            if (!string.IsNullOrWhiteSpace(title))
                qury = qury.Where(a => a.Title.Contains(title));
            if (!string.IsNullOrWhiteSpace(author))
                qury = qury.Where(a => a.Author.Contains(author));
            var model = qury.OrderByDescending(a => a.CreateTime).ToPagedList(pageIndex, pageSize);
            
            if (Request.IsAjaxRequest())
                return PartialView("_AjaxSearchGet", model);
            return View(model);
        }



        [HttpPost]
        public ActionResult Create(Subject entity)
        {
            if (ModelState.IsValid)
            {
                entity.CreateTime = DateTime.Now;
                subjectRepository.Add(entity);
                unitOfWork.Commit();
            }
                return View(entity);

        }


    }
}
