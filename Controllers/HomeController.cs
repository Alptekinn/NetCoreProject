using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetimOl.Models;
using System.Diagnostics;

namespace PetimOl.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IDapperHelper _dapperHelper;

        public HomeController(ILogger<HomeController> logger, IDapperHelper dapperHelper)
        {
            _logger = logger;
            _dapperHelper = dapperHelper;
        }
        public IActionResult AddPost()
        {
            return View();
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAnimalData([FromQuery] Dictionary<string, string>? formData)
        {
            string sql = @"  Select top(20) AnimalID,AnimalName,Age,Color,Weight,Sickness,Title,AnimalType.name as animalTypeName,VaccineStatus,Gender,ImagePath,Announcements.Id,Location from animals 
	   join Announcements on(animals.AnnouncementID=Announcements.Id)
	   Join AnimalRace on (Animals.RaceID=AnimalRace.RaceID)
	   Join AnimalType on (AnimalRace.TypeID = AnimalType.TypeID)";
            if (formData != null && formData.Count > 0)
            {
                sql += " where AnimalType.TypeID = @AnimalTypeID and Gender = @Gender and Color = @Color ";
                var model = _dapperHelper.GetList<AnimalModel>(sql, new
                {
                    AnimalTypeID = formData["AnimalTypeName"],
                    Gender = formData["Gender"],
                    Color = formData["Color"],
                });

                return Json(model);
            }
            else
            {
                var model = _dapperHelper.GetList<AnimalModel>(sql);

                return Json(model);
            }
        }

        public IActionResult Detail(string postId)
        {
            string sql = @"Select * from animals 
	   join Announcements on(animals.AnnouncementID=Announcements.Id)
	   Join AnimalRace on (Animals.RaceID=AnimalRace.RaceID)
	   Join AnimalType on (AnimalRace.TypeID = AnimalType.TypeID)
	   join Users on (Announcements.UserID = Users.UserID)
	   where AnimalID = @postId";
            var model = _dapperHelper.GetList<AnimalModel>(sql, new { postId });

            return View(model);
        }

        [HttpPost]
        public IActionResult AddPost(AnimalModel model)
        {
            model.CreatedDate = DateTime.Now;
            if (HttpContext.User!=null)
            {
                string ID = HttpContext.User.FindFirst("UserID")!.Value.ToString();

                model.UserID = Convert.ToInt32(ID);
            }
            string sqlAnnouncements = @"INSERT INTO Announcements (CreatedDate, Title, Description, UserID, ImagePath, Location) OUTPUT INSERTED.Id VALUES (@CreatedDate, @Title, @Description, @UserID, @ImagePath, @Location);";
            string sqlAnimals = @"insert into Animals(AnimalName,Age,Color,Sickness,Weight,VaccineStatus,RaceID,AnnouncementID,Gender ) values (@AnimalName, @Age, @Color, @Sickness,@Weight, @VaccineStatus, @RaceID, @AnnouncementID, @Gender)";
            int announcementID = _dapperHelper.QueryFirst<int>(sqlAnnouncements, new { model.CreatedDate, model.Title, model.Description, model.UserID, model.ImagePath, model.Location });
            model.AnnouncementID = announcementID;           
            int data = _dapperHelper.Execute(sqlAnimals, new { model.AnimalName,model.Weight, model.Age, model.Color, model.Sickness, model.VaccineStatus, model.RaceID, model.AnnouncementID, model.Gender });

            return RedirectToAction("Index","Home");
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Chats()
        {

            return View();
        }
        [HttpGet]
        public IActionResult GetData(int id)
        {
            string GetChat = @"Select MessageID,a.UserID,Message,MessageDate,UserSecondID,a.Username+' '+a.LastName as Fullname
	   ,b.Username+' '+b.LastName as SecondFullname from Messages
	   inner join Users as a on (Messages.UserID=a.UserID)
	   inner join Users as b on (Messages.UserSecondID = b.UserID) where
	   Messages.UserID in (select UserID from MEssages where Messages.UserID=@UserID)";
            string UserId = HttpContext.User.FindFirst("UserID")!.Value;
            var model = _dapperHelper.GetList<ChatModel>(GetChat, new { UserId = Convert.ToInt32(UserId) });
            return Json(model);
        }
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}