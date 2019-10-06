using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using SyZero.Common;
using SyZero.Domain;
using SyZero.Domain.Repository;
using SyZero.Domain.Entities;


namespace SyZero.Application
{
   public class UserService : IUserService
    {
        private readonly IRepository<User> _userRep;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRep, IMapper mapper, IUnitOfWork unitOfWork)
        {
                _userRep = userRep;
                _mapper = mapper;
                _unitOfWork = unitOfWork;
        }

        public bool Add(UserDto dto)
        {
           
            return _unitOfWork.SaveChange() > 0;
        }

        public bool Delect(string IDs)
        {
            string[] arry = IDs.Split(',');
            _userRep.Delete(u => ((IList)arry).Contains(u.Id.ToString()));
            return _unitOfWork.SaveChange() > 0;
        }

        public UserDto GetDto(long Id)
        {
            var user = _userRep.GetModel(Id);
            return _mapper.Map<UserDto>(user);
        }

        public List<UserDto> GetList(QueryDto queryDto)
        {
            queryDto.offset = string.IsNullOrEmpty(queryDto.offset) ? "0" : queryDto.offset;
            queryDto.limit = string.IsNullOrEmpty(queryDto.limit) ? "20" : queryDto.limit;
            var userlist = _userRep.GetPaged(int.Parse(queryDto.offset), int.Parse(queryDto.limit), p=>p.Id,u=>u.Name.IndexOf(queryDto.key) > 0,false);
            return _mapper.Map<List<UserDto>>(userlist);
        }

        public UserDto GetUser(string name)
        {
            Logger.Info("开始GetUser");
            var user = _userRep.GetModel(p => p.Name == name);
            return _mapper.Map<UserDto>(user);
        }

        public bool IsRepeatByName(string UserName)
        {
            bool IsExist = _userRep.Count(u => u.Name == UserName) > 0;
            return IsExist;
        }

        public bool Login(string UserName, string Password)
        {
            bool IsExist = _userRep.Count(u => u.Name == UserName && u.Password == MD5Encrypt.Get32MD5One(Password)) > 0;
            return IsExist;
        }

        public int Register(RegisterDto registerDto)
        {
            
            return _unitOfWork.SaveChange();
        }

        public int Updata(UserDto dto)
        {
         
            return _unitOfWork.SaveChange();
        }

        public int UpdataInfo(UserDto  userDto)
        {
          
            return _unitOfWork.SaveChange();
        }
    }
}
