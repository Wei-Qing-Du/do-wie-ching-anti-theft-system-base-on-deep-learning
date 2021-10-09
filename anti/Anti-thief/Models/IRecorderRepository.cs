using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anti_thief.Models
{
    public interface IRecorderRepository
    {
        Record Add(Record student);
        Record Update(Record updateStudent);
        Record Delete(int id);
    }
}
