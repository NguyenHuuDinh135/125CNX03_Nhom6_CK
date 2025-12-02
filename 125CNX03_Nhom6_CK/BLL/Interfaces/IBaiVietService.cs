using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public interface IBaiVietService
    {
        List<XElement> GetAllArticles();
        XElement GetArticleById(int id);
        void AddArticle(XElement article);
        void UpdateArticle(XElement article);
        void DeleteArticle(int id);
        List<XElement> GetActiveArticles();
        List<XElement> GetLatestArticles(int count);
        List<XElement> GetArticlesByAuthor(int authorId);
        List<XElement> SearchArticles(string searchTerm);
    }
}