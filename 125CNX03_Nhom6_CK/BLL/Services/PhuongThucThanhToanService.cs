using _125CNX03_Nhom6_CK.DAL.Repositories;
using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public class PhuongThucThanhToanService : IPhuongThucThanhToanService
    {
        private readonly IPhuongThucThanhToanRepository _paymentMethodRepository;

        public PhuongThucThanhToanService()
        {
            _paymentMethodRepository = new PhuongThucThanhToanRepository();
        }

        public List<XElement> GetAllPaymentMethods()
        {
            return _paymentMethodRepository.GetAll();
        }

        public XElement GetPaymentMethodById(int id)
        {
            return _paymentMethodRepository.GetById(id);
        }

        public void AddPaymentMethod(XElement paymentMethod)
        {
            _paymentMethodRepository.Add(paymentMethod);
        }

        public void UpdatePaymentMethod(XElement paymentMethod)
        {
            _paymentMethodRepository.Update(paymentMethod);
        }

        public void DeletePaymentMethod(int id)
        {
            _paymentMethodRepository.Delete(id);
        }

        public List<XElement> GetPaymentMethodsByUserId(int userId)
        {
            return _paymentMethodRepository.GetByUserId(userId);
        }

        public XElement GetPaymentMethodByCardNumber(string cardNumber)
        {
            return _paymentMethodRepository.GetByCardNumber(cardNumber);
        }
    }
}