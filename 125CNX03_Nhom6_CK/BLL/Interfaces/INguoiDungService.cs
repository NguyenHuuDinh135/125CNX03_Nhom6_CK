using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public interface INguoiDungService
    {
        List<XElement> GetAllUsers();
        XElement GetUserById(int id);
        XElement GetUserByEmail(string email);
        XElement AuthenticateUser(string email, string password);
        void AddUser(XElement user);
        void UpdateUser(XElement user);
        void DeleteUser(int id);
        List<XElement> GetUsersByRole(string role);
        List<XElement> GetActiveUsers();
        bool IsEmailUnique(string email, int? userIdToExclude = null);
    }
}