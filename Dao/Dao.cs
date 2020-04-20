using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using taskcore.Models;

namespace taskcore.Dao
{
    public abstract class Dao
    {
        public DatabaseContext _context = null;

  //      public abstract Task<List<Project>> Read(int id); 

        //public abstract 
        public DatabaseContext getContext()
        {
            if (_context == null)
            {
                _context = DatabaseContext.getContext();
            }
            return _context;
        }


    }
}