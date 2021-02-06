using ProgrammersBlog.Entities.Concreate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Entities.DTOs
{
    public class ArticleAddDto
    {

        [DisplayName("Başlık")]
        [Required(ErrorMessage ="{0} alanı boş geçilmemelidir")]
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden fazla girilemez")]
        [MinLength(5, ErrorMessage = "{0} {1} karakterden az olmamalıdır")]
        public string Title { get; set; }


        [DisplayName("İçerik")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir")]
        [MinLength(20, ErrorMessage = "{0} {1} karakterden az olmamalıdır")]
        public string Content { get; set; }

        [DisplayName("Tumbnail")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir")]
        [MaxLength(250, ErrorMessage = "{0} {1} karakterden fazla girilemez")]
        [MinLength(5, ErrorMessage = "{0} {1} karakterden az olmamalıdır")]
        public string Tumbnail { get; set; }

        [DisplayName("Tarih")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir")]
        [DisplayFormat(ApplyFormatInEditMode =true, DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [DisplayName("Seo Yazar")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir")]
        [MaxLength(50, ErrorMessage = "{0} {1} karakterden fazla girilemez")]
        [MinLength(0, ErrorMessage = "{0} {1} karakterden az olmamalıdır")]
        public string SeoAuthuor { get; set; }

        [DisplayName("Seo Açıklama")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir")]
        [MaxLength(150, ErrorMessage = "{0} {1} karakterden fazla girilemez")]
        [MinLength(5, ErrorMessage = "{0} {1} karakterden az olmamalıdır")]
        public string SeoDescription { get; set; }

        [DisplayName("Seo Etiketler")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir")]
        [MaxLength(70, ErrorMessage = "{0} {1} karakterden fazla girilemez")]
        [MinLength(5, ErrorMessage = "{0} {1} karakterden az olmamalıdır")]
        public string SeoTags { get; set; }


        [DisplayName("Kategori")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir")]
        public int CategoryId { get; set; }

        [DisplayName("Seo Etiketler")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir")]
        public Category Category { get; set; }

        [DisplayName("Aktiflik")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")]
        public bool IsActive { get; set; }


    }
}
