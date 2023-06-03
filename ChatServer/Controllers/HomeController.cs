﻿using ChatServer.Areas.Identity.Data;
using ChatServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Packaging;
using System.Diagnostics;
using System.Security.Claims;

namespace ChatServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _userContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext userContext)
        {
            _logger = logger;

            _userContext = userContext;
        }
        [Authorize]
        public IActionResult Index()
        {
            //var AllMessages = _context.Messages.ToList();
            //var messages = AllMessages.FindAll(item => item.ReceiverEmail == User.FindFirstValue(ClaimTypes.Email) || item.SenderEmail == User.FindFirstValue(ClaimTypes.Email));
            //Console.WriteLine(messages.Count);
            //var chatsEmails = new List<string>();
            //foreach (var message in messages)
            //{
            //    if (!chatsEmails.Contains(message.SenderEmail))
            //        chatsEmails.Add(message.SenderEmail);
            //}
            //ViewData["Messages"] = messages;
            //ViewData["Chats"] = chatsEmails;
            ViewData["User"] = _userContext.Users.Where( X => X.Email == User.FindFirstValue(ClaimTypes.Email)).First().Id;
            return View();
        }
        [HttpGet]
        public JsonResult GetChats()
        {
            var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var chats = _userContext.Chat
                .Include(c => c.ChatUsers)
                .Include(c=>c.Messages)
                .ToList();

            var UserChats = chats.FindAll(item => item.ChatUsers.Contains(_userContext.Users.ToList<ApplicationUser>().Find(item => item.Id == loggedUserId)));
            string json = JsonConvert.SerializeObject(UserChats, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(json);
        }


        [HttpGet]
        public JsonResult GetUser(string email)
        {
            var user = _userContext.Users.ToList().Find(x => x.Email == email);
            string json = JsonConvert.SerializeObject(user, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(json);
        }









        public class ChatData
        {
            public List<ApplicationUser> users { get; set; }
            public string Title { get; set; }
        }
        [HttpPost]
        public IActionResult AddChat([FromBody] ChatData data)
        {
            try
            {
                var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user1 = _userContext.Users.ToList<ApplicationUser>().Find(item => item.Id == loggedUserId);
                data.users.Add(user1);

                Chat newChat = new Chat();
                
                newChat.ChatUsers = new List<ApplicationUser>();


                foreach (var user in data.users)
                {
                    newChat.ChatUsers.Add(_userContext.Users.Where(item => item.Id == user.Id).FirstOrDefault());
                }

                newChat.Date = DateTime.Now;
                newChat.Id = Guid.NewGuid().ToString("N");
                string Names = "";
                foreach(var user in data.users)
                {
                    Names += user.FirstName + " "; 
                }
                newChat.Title = data.Title;
                _userContext.Chat.Add(newChat);
                _userContext.SaveChanges();

                return Json(new { status = "ok" });
            }
            catch (Exception e)
            {
                return Json(new { status = e.Message });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Send(Message message)
        //{

        //    var email = User.FindFirstValue(ClaimTypes.Email);
        //    message.SenderEmail = email;
        //    message.Time = DateTime.Now;
        //    message.Id = Guid.NewGuid().ToString("N");

        //        _context.Messages.Add(message);
        //        _context.SaveChanges();
        //        Console.WriteLine(message.Id.ToString());
        //        return Redirect("/");

        //   return View("Index");
        //}
        public record DataJ(string Content, string ChatId);
        [HttpPost]
       
        public IActionResult SendMessage([FromBody] DataJ data)
        {
            try
            {
                var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _userContext.Users.ToList<ApplicationUser>().Find(item => item.Id == loggedUserId);
                Message obj = new Message();
                obj.user = user;
                obj.Date = DateTime.Now;
                obj.Id = Guid.NewGuid().ToString("N");
                obj.Chat = _userContext.Chat.ToList<Chat>().Find(item => item.Id == data.ChatId);
                
                obj.Content = data.Content;
                
                
                _userContext.Message.Add(obj);
                _userContext.SaveChanges();
                
                return Json(new { status = "ok" });
            }
            catch (Exception e)
            {
                return Json(new { status = e.Message });
            }
        }


        public record ChatRec(string ChatID);
        [HttpPost]

        public IActionResult DeleteConversation([FromBody] ChatRec chatID)
        {
            try
            {
                
                
                _userContext.Message.RemoveRange(_userContext.Message.Where(item => item.Chat.Id == chatID.ChatID));
                //
                //
                _userContext.SaveChanges();

                return Json(new { status = "ok" });
            }
            catch (Exception e)
            {
                return Json(new { status = e.Message });
            }


            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}