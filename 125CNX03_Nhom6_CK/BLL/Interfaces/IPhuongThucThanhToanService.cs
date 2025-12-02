using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public interface IPhuongThucThanhToanService
    {
        List<XElement> GetAllPaymentMethods();
        XElement GetPaymentMethodById(int id);
        void AddPaymentMethod(XElement paymentMethod);
        void UpdatePaymentMethod(XElement paymentMethod);
        void DeletePaymentMethod(int id);
        List<XElement> GetPaymentMethodsByUserId(int userId);
        XElement GetPaymentMethodByCardNumber(string cardNumber);
    }
}