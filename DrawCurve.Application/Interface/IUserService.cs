﻿using DrawCurve.Domen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Application.Interface
{
    public interface IUserService
    {
        public User GetUser(int id);
    }
}
