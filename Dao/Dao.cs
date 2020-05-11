namespace taskcore.Dao
{
    public class Dao
    {
        public DatabaseContext _context = null;

         
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