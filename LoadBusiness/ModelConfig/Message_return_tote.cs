using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LoadBusiness.Load
{
    public class Message_return_tote
    {
        
        public bool result { get; set; }
        public string message { get; set; }
        public Guid? log_interface_index { get; set; }
    }
}
