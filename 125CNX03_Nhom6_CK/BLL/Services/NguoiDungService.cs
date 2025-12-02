using _125CNX03_Nhom6_CK.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public class NguoiDungService : INguoiDungService
    {
        private readonly INguoiDungRepository _userRepository;

        public NguoiDungService()
        {
            _userRepository = new NguoiDungRepository();
        }

        public List<XElement> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public XElement GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public XElement GetUserByEmail(string email)
        {
            return _userRepository.GetByEmail(email);
        }

        public XElement AuthenticateUser(string email, string password)
        {
            var passwordHash = GetPasswordHash(password);
            return _userRepository.GetByEmailAndPassword(email, passwordHash);
        }

        public void AddUser(XElement user)
        {
            var passwordHash = GetPasswordHash(user.Element("MatKhauHash").Value);
            user.Element("MatKhauHash").Value = passwordHash;
            _userRepository.Add(user);
        }

        public void UpdateUser(XElement user)
        {
            _userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            _userRepository.Delete(id);
        }

        public List<XElement> GetUsersByRole(string role)
        {
            return _userRepository.GetByRole(role);
        }

        public List<XElement> GetActiveUsers()
        {
            return _userRepository.GetActiveUsers();
        }

        public bool IsEmailUnique(string email, int? userIdToExclude = null)
        {
            var existingUser = _userRepository.GetByEmail(email);
            if (existingUser != null)
            {
                if (userIdToExclude.HasValue)
                {
                    var userId = int.Parse(existingUser.Element("Id").Value);
                    return userId == userIdToExclude.Value;
                }
                return false;
            }
            return true;
        }

        private string GetPasswordHash(string password)
        {
            // Simple MD5 hash for demonstration - consider using a more secure method like bcrypt
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
                var hashBytes = md5.ComputeHash(inputBytes);
                return System.BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}