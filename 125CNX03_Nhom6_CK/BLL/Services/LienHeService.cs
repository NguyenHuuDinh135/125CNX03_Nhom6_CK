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

        public void AddMessage(XElement message)
        {
            message.Element("DaXem").Value = "false";
            message.Element("NgayGui").Value = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
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
            throw new NotImplementedException();
        }
    }
}