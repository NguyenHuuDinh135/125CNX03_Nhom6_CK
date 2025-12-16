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
            if (user.Element("MatKhauHash") == null)
                user.Add(new XElement("MatKhauHash", ""));

            string rawPassword = user.Element("MatKhauHash")?.Value ?? "";

            string passwordHash = GetPasswordHash(rawPassword);

            user.Element("MatKhauHash").Value = passwordHash;

            _userRepository.Add(user);
        }


        public void UpdateUser(XElement user)
        {
            if (user.Element("MatKhauHash") == null)
                user.Add(new XElement("MatKhauHash", ""));

            // ⚠️ Chỉ hash khi user có nhập mật khẩu mới
            string rawPassword = user.Element("MatKhauHash")?.Value ?? "";

            if (!string.IsNullOrWhiteSpace(rawPassword))
            {
                string passwordHash = GetPasswordHash(rawPassword);
                user.Element("MatKhauHash").Value = passwordHash;
            }

            _userRepository.Update(user); // ✅ ĐÚNG
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
        public int GenerateNewId()
        {
            var all = _userRepository.GetAll();
            if (all == null || all.Count == 0)
                return 1;

            return all.Max(x =>
            {
                int.TryParse(x.Element("Id")?.Value, out int id);
                return id;
            }) + 1;
        }

    }
}