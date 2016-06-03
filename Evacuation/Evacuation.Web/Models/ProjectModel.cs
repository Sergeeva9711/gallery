using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Evacuation.Web.Models
{
    public class ProjectModel
    {
        public int ProjectID { get; set; }

        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataCreation { get; set; }

        public int UserID { get; set; }
        private UserModel User { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataStrart { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataEnd { get; set; }

        public byte[] Image { get; set; }

        public bool IsDeleted { get; set; }

    }
}