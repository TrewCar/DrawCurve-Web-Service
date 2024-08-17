using DrawCurve.Domen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Application.Interface
{
    public interface ILoginService
    {
        public User? Login(string username, string password);
        public string RegIn(ref User user, UserLogin userLogin);
    }
}
