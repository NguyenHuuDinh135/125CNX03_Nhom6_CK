using System;

namespace _125CNX03_Nhom6_CK.GUI.Forms
{
    public class UserEventArgs : EventArgs
    {
        // Add properties as needed, for example:
        public object User { get; }

        public UserEventArgs(object user)
        {
            User = user;
        }
    }
}
