﻿using System.Collections.Generic;
using Manicure.BusinessLogic.Services.Abstract;
using Manicure.Common.Domain;
using Manicure.DataAccess.Abstract;

namespace Manicure.BusinessLogic.Services.Concrete
{
    public class MasterService : IMasterService
    {
        private readonly IRepository<Master> _masterRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MasterService(
            IRepository<Master> masterRepository,
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork)
        {
            _masterRepository = masterRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Master master, string userLogin)
        {
            var user = _userRepository.GetFirst(u => u.Login == userLogin);

            master.User = user;

            _masterRepository.Create(master);
            _unitOfWork.SaveChanges();
        }

        public IEnumerable<Master> GetAll()
        {
            return _masterRepository.GetAll();
        }
    }
}