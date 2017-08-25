using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using RapidPRWeb.Hubs;
using RapidPRWeb.Models;

namespace RapidPRWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ApplicationDbContext Db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Video";

            return View("Video");
        }

        public ActionResult Video()
        {
            ViewBag.Title = "Video";
            return View();
        }

        [HttpPost]
        public string PostVideo(string videoName, long duration, string link)
        {
            var data = "";
            try
            {
                var video = new Video
                {
                    VideoUploadTime = DateTime.Now,
                    NowPublish = false,
                    VideoName = videoName,
                    VideoPublishTime = DateTime.Now,
                    VideoLiveLink = link,
                    VideoDuration = duration/1000
                };
                Db.Videos.Add(video);
                Db.SaveChanges();
                data = new JavaScriptSerializer().Serialize(new
                {
                    res = true,
                    msg = "succesfully added.."
                });
            }
            catch (Exception e)
            {
                data = new JavaScriptSerializer().Serialize(new
                {
                    res = false,
                    msg = e.ToString()
                });
            }

            return data;
        }

        public ActionResult Text()
        {
            ViewBag.Title = "Text";
            return View();
        }

        public ActionResult Advertise()
        {
            ViewBag.Title = "Advertise";
            return View();
        }

        [System.Web.Http.HttpGet]
        public string GetAllVideo()
        {
            try
            {
                var res = Db.Videos;
                var s = JsonConvert.SerializeObject(res, Formatting.Indented);
                return s;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public ActionResult DeleteVideo(int id)
        {
            ViewBag.Title = "Video";

            var v = Db.Videos.Find(id);
            if (v == null)
            {
                ViewBag.msg = "No Video Found";
                return View("Video");
            }

            var ext = v.VideoName.Split('.');
            var fullPath = Request.MapPath("~/Video/" + v.VideoId + "." + ext[1]);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            Db.Videos.Remove(v);
            Db.SaveChanges();


            ViewBag.msg = "Delete Successfully";
            return View("Video");
        }

        [HttpPost]
        public ActionResult VideoPublish(int id)
        {
            ViewBag.Title = "Video";
            var v = Db.Videos.Find(id);
            if (v == null)
            {
                ViewBag.msg = "No Video Found";
                return View("Video");
            }
            if (v.NowPublish)
            {
                ViewBag.msg = "Video Publish Cancel Successfully";
                v.NowPublish = false;
                Db.SaveChanges();
            }
            else
            {
                ViewBag.msg = "Video Publish Successfully";
                v.NowPublish = true;
                Db.SaveChanges();
            }

            TvHub.AddPlayList();
            return View("Video");
        }

        /****************************Advertise*********************************/

        [System.Web.Http.HttpGet]
        public string GetAllAdvertise()
        {
            var res = Db.Advertises;
            var s = JsonConvert.SerializeObject(res, Formatting.Indented);
            return s;
        }

        [HttpPost]
        public ActionResult PostAdvertise(HttpPostedFileBase file, string boxName)
        {
            ViewBag.Title = "Advertise";
            if (file != null)
            {
                try
                {
                    // Validate the uploaded file if you want like content length(optional)

                    // Get the complete file path

                    var res = file.FileName.Split('.');
                    var advertise = new Advertise
                    {
                        AdvertiseUploadTime = DateTime.Now,
                        IsPublish = false,
                        BoxName = boxName,
                        AdvertiseName = file.FileName,
                        AdvertisePublishTime = DateTime.Now
                    };

                    Db.Advertises.Add(advertise);
                    Db.SaveChanges();

                    var uploadFilesDir = System.Web.HttpContext.Current.Server.MapPath("~/Advertise");
                    if (!Directory.Exists(uploadFilesDir))
                    {
                        Directory.CreateDirectory(uploadFilesDir);
                    }
                    var fileSavePath = Path.Combine(uploadFilesDir, advertise.AdvertiseId + "." + res[1]);

                    // Save the uploaded file to "UploadedFiles" folder
                    file.SaveAs(fileSavePath);

                    ViewBag.msg = "Uploaded Successfully";
                    return View("Advertise");
                }
                catch (Exception e)
                {
                    ViewBag.msg = e.ToString();
                    return View("Advertise");
                }
            }
            ViewBag.msg = "Faild to upload image";
            return View("Advertise");
        }

        public ActionResult DeleteAdvertise(int id)
        {
            ViewBag.Title = "Advertise";

            var v = Db.Advertises.Find(id);
            if (v == null)
            {
                ViewBag.msg = "No Advertise Found";
                return View("Advertise");
            }

            var ext = v.AdvertiseName.Split('.');
            var fullPath = Request.MapPath("~/Advertise/" + v.AdvertiseId + "." + ext[1]);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }


            Db.Advertises.Remove(v);
            Db.SaveChanges();


            ViewBag.msg = "Delete Successfully";
            return View("Advertise");
        }

        [HttpPost]
        public String AdvertisePublish(int id, string boxName, int duration)
        {
            ViewBag.Title = "Advertise";
            var v = Db.Advertises.Find(id);
            v.AdvertisePublishTime = DateTime.Now;
            if (v == null)
            {
                return JsonConvert.SerializeObject(new
                {
                    res = false,
                    msg = "No Advertise Found"
                });
            }
            var ext = v.AdvertiseName.Split('.');
            var fullPath = Request.MapPath("~/Advertise/" + v.AdvertiseId + "." + ext[1]);


            v.BoxName = boxName;
            v.LiveDurationInSec = duration;
            var msg = "";
            if (v.IsPublish)
            {
                msg = "Advertise Publish Cancel Successfully";
                v.IsPublish = false;
                Db.SaveChanges();
            }
            else
            {
                msg = "Advertise Publish Successfully";
                v.IsPublish = true;
                Db.SaveChanges();
            }


            return JsonConvert.SerializeObject(new
            {
                res = true,
                msg,
                advertise = v
            });
        }

        /**********************************************************************/


        /*******************Text Controll*******************************************/

        [HttpPost]
        public ActionResult PostText(string text)
        {
            ViewBag.Title = "Text";
            var t = new Text
            {
                TextContent = text,
                IsPublishNow = false,
                TextUploadTime = DateTime.Now,
                TextPublishTime = DateTime.Now
            };
            Db.Texts.Add(t);
            Db.SaveChanges();

            ViewBag.msg = "Successfully post";
            return View("Text");
        }

        [System.Web.Http.HttpGet]
        public string GetAllText()
        {
            var res = Db.Texts;
            var s = JsonConvert.SerializeObject(res, Formatting.Indented);
            return s;
        }

        [HttpPost]
        public ActionResult TextPublish(int id)
        {
            ViewBag.Title = "Text";
            var v = Db.Texts.Find(id);
            v.TextPublishTime = DateTime.Now;
            if (v == null)
            {
                ViewBag.msg = "No Text Found";
                return View("Text");
            }


            /*var allPublishText = Db.Texts.Where(x => x.IsPublishNow && x.TextId != id);
            foreach (var a in allPublishText)
            {
                a.IsPublishNow = false;
            }*/

            if (v.IsPublishNow)
            {
                ViewBag.msg = "Text Publish Cancel Successfully";
                v.IsPublishNow = false;
                Db.SaveChanges();
            }
            else
            {
                ViewBag.msg = "Text Publish Successfully";
                v.IsPublishNow = true;
                Db.SaveChanges();
            }

            TvHub.AddText();
            return View("Text");
        }

        public ActionResult DeleteText(int id)
        {
            ViewBag.Title = "Text";

            var v = Db.Texts.Find(id);
            if (v == null)
            {
                ViewBag.msg = "No Text Found";
                return View("Text");
            }


            Db.Texts.Remove(v);
            Db.SaveChanges();


            ViewBag.msg = "Delete Successfully";
            return View("Text");
        }

        /****************************************************************************/


        /**************************Model Generator************************************/

        public string GetAdvertiseData(Advertise advertise)
        {
            var s = JsonConvert.SerializeObject(new
            {
                id = advertise.AdvertiseId,
                address = advertise.GetPath(),
                duration = advertise.LiveDurationInSec,
                boxName = advertise.BoxName
            });
            return s;
        }

        public ActionResult UploadYoutube()
        {
            ViewBag.msg = "Upload youtube";
            ViewBag.Title = "Upload youtube";
            return View();
        }
    }
}