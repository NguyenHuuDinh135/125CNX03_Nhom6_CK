using System;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public class UserEventArgs : EventArgs
    {
        public XElement User { get; }

        public UserEventArgs(XElement user)
        {
            User = user;
        }
    }
}