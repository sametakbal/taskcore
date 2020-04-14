using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tasky.Models;

namespace Tasky.Dao
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