using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public interface ILienHeService
    {
        List<XElement> GetAllMessages();
        XElement GetMessageById(int id);
        void AddMessage(XElement message);
        void UpdateMessage(XElement message);
        void DeleteMessage(int id);
        List<XElement> GetUnreadMessages();
        List<XElement> GetReadMessages();
        void MarkMessageAsRead(int id);
    }
}