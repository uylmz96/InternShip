using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace InternShip.MvcUI.App_Classes
{
    using iTextSharp.text.pdf.parser;
    using Models;
    using System.Text;

    public class Frequance
    {
        //Yüklenen pdf dosyasındaki kelimelerin frekanslarını hesaplamak için yazıldı.
        InternShipContext context = new InternShipContext();
        public void FileFrequance(string FilePath, HttpPostedFileBase file, int internshipID)
        {
            Dictionary<string, int> words = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
            countWordsInFile(FilePath, words);

            Document document = new Document();
            document.CrtDate = DateTime.Now;
            document.DelDate = null;
            document.DocName = file.FileName;
            document.InternShipID = internshipID;
            document.Keyword = KeywordsinFile(words).ToLower();
            document.Path = FilePath;
            context.Documents.Add(document);
            try
            {
                context.SaveChanges();
            }
            catch (Exception)
            {

            }
        }

        public string KeywordsinFile(Dictionary<string, int> words)
        {
            string keywords = "";
            if (words == null) return keywords;

            foreach (var item in words)
            {
                if (item.Value > 5)
                {
                    keywords += item.Key + ",";
                }
            }
            return keywords;
        }

        private void countWordsInFile(string FilePath, Dictionary<string, int> Words)
        {
            var content = ReadPdf(FilePath);
            var wordPattern = new Regex(@"\w+");
            foreach (Match match in wordPattern.Matches(content))
            {
                int currentCount = 0;
                Words.TryGetValue(match.Value, out currentCount);

                currentCount++;
                Words[match.Value] = currentCount;
            }
        }

        private string ReadPdf(string FilePath)
        {
            iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(FilePath);
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                sb.Append(PdfTextExtractor.GetTextFromPage(reader, i));
            }
            return sb.ToString();
        }

        public List<InternShip> CheckKeywords(List<InternShip> internShips, string keywords)
        {
            /* Bir staj listesi arasından;
             * Stajların dosyalarının anahtar kelimeleri içermeyenleri bulur;
             * ve o stajlar listeden silinir.
             */

            List<string> keywordList = keywords.ToLower().Split(',', ' ').ToList<string>();
            List<InternShip> result = new List<InternShip>();
            foreach (InternShip item in internShips)
            {
                List<Document> documents = context.Documents.Where(x => x.InternShipID == item.InternShipID).ToList();
                bool Keyword_in_Internship = false;
                foreach (Document document in documents)
                {
                    //Staja ait tüm dokümanlar
                    foreach (string keyword in keywordList)
                    {
                        //Tüm anahtar kelimeler dokümanda aranıyor.
                        if (document.Keyword.Contains(keyword))
                        {
                            Keyword_in_Internship = true;
                            break;
                        }
                    }
                    //Eğer bir dosyada bulursa stajın diğer dosyalarına bakmaz
                    if (Keyword_in_Internship)
                        break;
                }
                //Eğer hiçbir dosyada eşleşme olmazsa bu staj elenir.
                if (Keyword_in_Internship)
                {
                    result.Add(item);
                }
            }
            return result;
        }
    }
}