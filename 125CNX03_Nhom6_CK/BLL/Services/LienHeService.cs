using _125CNX03_Nhom6_CK.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public class LienHeService : ILienHeService
    {
        private readonly ILienHeRepository _contactRepository;

        public LienHeService()
        {
            _contactRepository = new LienHeRepository();
        }

        public List<XElement> GetAllMessages()
        {
            return _contactRepository.GetAll();
        }

        public XElement GetMessageById(int id)
        {
            return _contactRepository.GetById(id);
        }

        public void AddMessage(XElement message, XElement currentUser)
        {
            // 1️⃣ Id
            if (message.Element("Id") == null)
                message.AddFirst(new XElement("Id", GenerateNewId()));

            // 2️⃣ Số điện thoại lấy từ người gửi
            if (message.Element("SoDienThoai") == null)
            {
                string sdt = currentUser?.Element("SoDienThoai")?.Value ?? "";
                message.Add(new XElement("SoDienThoai", sdt));
            }

            // 3️⃣ Đã xem
            if (message.Element("DaXem") == null)
                message.Add(new XElement("DaXem", "false"));

            // 4️⃣ Ngày gửi
            if (message.Element("NgayGui") == null)
                message.Add(new XElement(
                    "NgayGui",
                    DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
                ));

            _contactRepository.Add(message);
        }



        public void UpdateMessage(XElement message)
        {
            _contactRepository.Update(message);
        }

        public void DeleteMessage(int id)
        {
            _contactRepository.Delete(id);
        }

        public List<XElement> GetUnreadMessages()
        {
            return _contactRepository.GetUnreadMessages();
        }

        public List<XElement> GetReadMessages()
        {
            return _contactRepository.GetReadMessages();
        }

        public int GenerateNewId()
        {
            var all = _contactRepository.GetAll();

            if (all == null || !all.Any())
            {
                return 1;
            }

            int maxId = all
                .Select(x => x.Element("Id"))
                .Where(element => element != null)
                .Max(element => (int)element);

            return maxId + 1;
        }

        public void MarkMessageAsRead(int id)
        {
            var msg = _contactRepository.GetById(id);
            if (msg == null) return;

            msg.Element("DaXem").Value = "true";
            _contactRepository.Update(msg);
        }

    }
}