using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evacuation.Dal.Entities
{
    public class Project
    {       

        public int ProjectID { get; set; }
        
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataCreation { get; set; }        
        
        public int UserID { get; set; }     
        private User User { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataStrart { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime DataEnd { get; set; }
        
        [DataType(DataType.ImageUrl)]
        public byte[] Image { get; set; }        

        public bool IsDeleted { get; set; }

    }
}
