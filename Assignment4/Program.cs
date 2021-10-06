﻿using System;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Assignment4.Entities;

namespace Assignment4
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = LoadConfiguration();
            var connectionString = configuration.GetConnectionString("KanbanContext");

            var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>()
           .UseSqlServer(connectionString);

            using var context = new KanbanContext(optionsBuilder.Options);
            
        //     KanbanContextFactory.Seed(context);

            var try = new TaskRepository(configuration);
        }
        static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>();

            return builder.Build();
        }
    }
}
