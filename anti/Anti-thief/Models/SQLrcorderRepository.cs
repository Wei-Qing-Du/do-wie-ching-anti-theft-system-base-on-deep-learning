using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anti_thief.Models
{
    public class SQLrcorderRepository:IRecorderRepository
    {
        private readonly ILogger logger;
        private readonly RecordbContext context;

        public SQLrcorderRepository(RecordbContext context, ILogger<SQLrcorderRepository> logger)
        {
            this.logger = logger;
            this.context = context;
        }


        public Record Add(Record record)
        {
            context.Recorder.Add(record);
            context.SaveChanges();
            return record;
        }

        public Record Delete(int id)
        {
            Record student = context.Recorder.Find(id);
            if (student != null)
            {
                context.Recorder.Remove(student);
                context.SaveChanges();
            }
            return student;

        }

        public Record Update(Record updaterecord)
        {

            var student = context.Recorder.Attach(updaterecord);

            student.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            context.SaveChanges();

            return updaterecord;

        }
    }
}
