using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace CUT_RAIL_MACHINE.Models
{
    public interface IUserRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        void Add(UserModel userModel);
        void Edit(UserModel userModel);
        void Remove(string Id);
        UserModel GetById(int Id);
        UserModel GetByUsername(string username);
        IEnumerable<UserModel> GetByAll();
        bool StatusConnectSQL();
        //...
    }
}
