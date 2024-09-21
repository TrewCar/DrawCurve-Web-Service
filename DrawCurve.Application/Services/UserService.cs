using DrawCurve.Application.Interface;
using DrawCurve.Domen.Models;
using DrawCurve.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Application.Services
{
    public class UserService : IUserService
    {
        private readonly DrawCurveDbContext context;
        public UserService(DrawCurveDbContext context)
        {
            this.context = context;
        }

        public User GetUser(int id)
        {
            var result = context.Users.Where(x => x.Id == id);

            if (!result.Any())
                return null;

            return result.First();
        }
    }
}
