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
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
    public class CsrfController : Controller
    {
        private readonly CsrfService service;

        public CsrfController(CsrfService service)
        {
            this.service = service;
        }

        [IgnoreAntiforgeryToken] // 偽造防止トークン無しでもリクエストを受け付ける
        public IActionResult Index()
        {
            var result = service.GetBoard();
            return View("index", result.Result);
        }

        //public IActionResult Search()
        //{
        //    var result = service.GetBoard();
        //    return View("index", result);
        //}

        [IgnoreAntiforgeryToken] // 偽造防止トークン無しでもリクエストを受け付ける
        public IActionResult Insert(BoardModel model)
        {
            var insResult = service.InsertBoard(model);
            var getResult = service.GetBoard();
            return View("index", getResult.Result);
        }
    }
}
