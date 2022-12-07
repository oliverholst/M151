using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Garage.Models
{
    public interface IAccount
    {
        /// <summary>
        /// Verbindet zur DB
        /// </summary>
        public SqlConnection Connect();

        /// <summary>
        /// Schliesst DB
        /// </summary>
        public void Close();

        /// <summary>
        /// Erzeugt ein User
        /// </summary>
        /// <param name="account"></param>
        public void Create(Account account);
    }
}

