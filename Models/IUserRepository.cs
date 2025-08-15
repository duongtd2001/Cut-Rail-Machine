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
        void Add();
        void Edit();
        void Remove(string Id);
        string GetProductNameByPO(string Po);
        void GetByUsername(string username);
        //IEnumerable<> GetByAll();
        bool StatusConnectERP();
        bool StatusConnectOST();
        //...
    }
}
