using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StraxTemplate.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string BannerText { get; set; }
        [MaxLength(250)]
        public string Img { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Text { get; set; }
        
        public DateTime CreatedTime { get; set; }

        [ForeignKey("CustomUsers")]
        public string UserId { get; set; }
        public CustomUser CustomUsers { get; set; }
      
        public string Subscribe { get; set; }


        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey("DesingType")]
        public int DesignTypeId { get; set; }
        public DesingType DesingType { get; set; }



    }
}
