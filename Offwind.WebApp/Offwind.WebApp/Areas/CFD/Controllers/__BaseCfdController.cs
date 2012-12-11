﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;
using Offwind.OpenFoam.Sintef;
using Offwind.WebApp.Models;
using Offwind.WebApp.Models.Account;
using log4net;

namespace Offwind.WebApp.Areas.CFD.Controllers
{
// ReSharper disable InconsistentNaming
    [Authorize(Roles = SystemRole.RegularUser)]
    public class __BaseCfdController : Controller
    {
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected string Title;
        protected string ShortTitle;
        protected string SectionTitle;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            Debug.Assert(Request.IsAuthenticated);

            var user = User.Identity.Name;
            using (var ctx = new OffwindEntities())
            {
                var dCase = ctx.DWorkCases.FirstOrDefault(c => c.Owner == user && c.Name == StandardCases.CfdCase);
                if (dCase == null)
                {
                    // Init basic properties
                    dCase = new DWorkCase();
                    dCase.Id = Guid.NewGuid();
                    dCase.Name = StandardCases.CfdCase;
                    dCase.Owner = user;
                    dCase.Created = DateTime.UtcNow;

                    // Init model
                    var model = SolverData.GetDefaultModel();
                    var serializer = new XmlSerializer(typeof(SolverData));
                    using (var writer = new StringWriter())
                    {
                        serializer.Serialize(writer, model);
                        dCase.Model = writer.ToString();
                        writer.Close();
                    }

                    ctx.DWorkCases.AddObject(dCase);
                    ctx.SaveChanges();
                }
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            ViewBag.SectionTitle = SectionTitle;
            ViewBag.ShortTitle = ShortTitle ?? Title;
            ViewBag.Title = String.Format("{0} | {1} | CFD | Offwind", Title ?? ShortTitle, SectionTitle);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            _log.Error(filterContext.Exception);
        }

        protected SolverData GetSolverData()
        {
            using (var ctx = new OffwindEntities())
            {
                var dCase = ctx.DWorkCases.First(c => c.Owner == User.Identity.Name && c.Name == StandardCases.CfdCase);
                var serializer = new XmlSerializer(typeof(SolverData));
                using (var reader = new StringReader(dCase.Model))
                {
                    return (SolverData)serializer.Deserialize(reader);
                }
            }
        }

        protected void SetSolverData(SolverData model)
        {
            var serializer = new XmlSerializer(typeof(SolverData));
            using (var ctx = new OffwindEntities())
            using (var writer = new StringWriter())
            {
                var dCase = ctx.DWorkCases.First(c => c.Owner == User.Identity.Name && c.Name == StandardCases.CfdCase);
                serializer.Serialize(writer, model);
                dCase.Model = writer.ToString();
                writer.Close();
                ctx.SaveChanges();
            }
        }
    }
    // ReSharper restore InconsistentNaming
}
