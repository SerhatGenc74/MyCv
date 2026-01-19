using MyCv.Models;

namespace MyCv.ViewModels.DataBase
{
    public class DB
    {
        private static AppDBContext DataBase { get; set; }

        public static AppDBContext GetContext()
        {
            if (DataBase == null)
            {
                DataBase = new AppDBContext(default);
            }
            return DataBase;
        }
    }

}