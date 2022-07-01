using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.Web.Http;
using Web.Models;
using Dapper;
using Service;

namespace Controllers
{
    public class XssController : Controller
    {
        private readonly XssService service;

        public XssController(XssService service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            var result = service.GetBoard();
            return View("index", result.Result);
        }


        [ValidateAntiForgeryToken]
        public IActionResult Insert(BoardModel model)
        {
            var insResult = service.InsertBoard(model);
            var getResult = service.GetBoard();
            return View("index", getResult.Result);
        }
    }
}
