﻿using Scaffolder.Core;
using System;
using System.IO;

namespace Scaffolder.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var connectionString = File.ReadAllText("d:/tmp/connection.conf");
            var db = new Scaffolder.Core.Data.SqlServerDatabase(connectionString);

            var builder = new SqlServerModelBuild(connectionString);
            

            var tables = builder.Build();

            foreach (var t in tables)
            {
                Console.WriteLine(t.Name);
            }

            Console.ReadLine();
        }
    }
}
