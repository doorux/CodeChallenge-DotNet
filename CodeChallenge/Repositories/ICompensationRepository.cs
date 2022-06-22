﻿using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation    Add(Compensation compensation);
        Compensation    GetByEmployeeId(string id);

        Task            SaveAsync();

    }
}