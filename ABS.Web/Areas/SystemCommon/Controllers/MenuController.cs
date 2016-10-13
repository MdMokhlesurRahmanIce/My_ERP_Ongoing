using ABS.Models.ViewModel.SystemCommon;
using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.SystemCommon.Controllers
{
    
    public class MenuController : Controller
    {
        // GET: /SystemCommon/Menu/
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }

        [SessionTimeout]
        [HttpPost,ValidateInput(false)]
        public System.Web.Mvc.JsonResult StatusPost(vmMemberUserStatusPost objPost)
        {
            string exMessage_ = string.Empty;
            int result = 0;
            if (Request.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        System.Web.HttpPostedFileBase file = null;
                        string newfileName = string.Empty;
                        string fileExtention = string.Empty;
                        string fileName = string.Empty;

                        if (objPost != null)
                        {
                            if (Request.Files.Count > 0)
                            {
                                file = Request.Files[0];
                                fileExtention = Path.GetExtension(file.FileName);
                                fileName = Guid.NewGuid().ToString();
                                newfileName = fileName + fileExtention;
                            }
                            System.Threading.Thread.Sleep(800);
                            object[] parameters = { objPost.PostBy, objPost.PostContent, newfileName, objPost.PostType, objPost.IsPrivate };
                            
                            if (result == 1)
                            {
                                //Notify to all
                              //  NewsPostHub.BroadcastData();
                                if (Request.Files.Count > 0)
                                {
                                    string savelocation = Server.MapPath("../Areas/Social/Content/Files/Timeline/");
                                    if (Directory.Exists(savelocation) == false) { Directory.CreateDirectory(savelocation); }
                                    string savePath = savelocation + newfileName;
                                    file.SaveAs(savePath);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        exMessage_ = ex.ToString();
                        result = -1;
                    }
                }
            }

            return Json(new { Status = result, exMessage = exMessage_ });
        }
	}
}