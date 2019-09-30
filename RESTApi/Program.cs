// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        16.01.2019 15:26
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only


using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CMCCloud
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                //.UseUrls("http://0.0.0.0:8091/")
        ;
    }
}