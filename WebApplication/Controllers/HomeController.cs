// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        13.09.2019 10:15
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only

using System;
using System.Diagnostics;
using CMCWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMCWeb.Controllers
{
    /// <summary>
    /// <para>Home Controller</para>
    /// Class HomeController. (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Imprint()
        {
            return View();
        }
    }
}