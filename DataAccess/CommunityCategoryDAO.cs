using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CommunityCategoryDAO
    {
        private static CommunityCategoryDAO instance = null;
        private static readonly object instanceLock = new object();

        public static CommunityCategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CommunityCategoryDAO();
                    }
                    return instance;
                }
            }
        }




    }
}