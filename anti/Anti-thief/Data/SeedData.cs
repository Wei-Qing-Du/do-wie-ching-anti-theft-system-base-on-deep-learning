using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Anti_thief.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anti_thief.Data
{
    public static class SeedData
    {
        public static IApplicationBuilder UseDataInitializer(
           this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var dbcontext = scope.ServiceProvider.GetService<RecordbContext>();

                System.Console.WriteLine("开始执行迁移数据库...");

                dbcontext.Database.Migrate();
                System.Console.WriteLine("数据库迁移完成...");

                if (!dbcontext.Recorder.Any())
                {
                    System.Console.WriteLine("开始创建种子数据中...");

                    dbcontext.Recorder.Add(new Record
                    {
                        ID = 0,
                        Name = "Unknown",
                        RecordDate = DateTime.Now
                }) ; 
                }

                return builder;
            }
        }
    }
  
}

