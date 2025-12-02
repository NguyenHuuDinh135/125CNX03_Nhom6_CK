using _125CNX03_Nhom6_CK.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public class BaiVietService : IBaiVietService
    {
        private readonly IBaiVietRepository _articleRepository;

        public BaiVietService()
        {
            _articleRepository = new BaiVietRepository();
        }

        public List<XElement> GetAllArticles()
        {
            return _articleRepository.GetAll();
        }

        public XElement GetArticleById(int id)
        {
            return _articleRepository.GetById(id);
        }

        public void AddArticle(XElement article)
        {
            article.Element("NgayDang").Value = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            _articleRepository.Add(article);
        }

        public void UpdateArticle(XElement article)
        {
            _articleRepository.Update(article);
        }

        public void DeleteArticle(int id)
        {
            _articleRepository.Delete(id);
        }

        public List<XElement> GetActiveArticles()
        {
            return _articleRepository.GetActiveArticles();
        }

        public List<XElement> GetLatestArticles(int count)
        {
            return _articleRepository.GetLatestArticles(count);
        }

        public List<XElement> GetArticlesByAuthor(int authorId)
        {
            return _articleRepository.GetByAuthor(authorId);
        }

        public List<XElement> SearchArticles(string searchTerm)
        {
            return _articleRepository.SearchArticles(searchTerm);
        }
    }
}