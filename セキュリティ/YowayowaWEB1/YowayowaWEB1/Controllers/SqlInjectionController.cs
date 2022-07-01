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
    public class SqlInjectionController : Controller
    {
        private readonly SqlInjectionService service;

        public SqlInjectionController(SqlInjectionService service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search(string name)
        {
            var result = service.GetMembers(name);
            return View("index", result);
        }
    }
}
