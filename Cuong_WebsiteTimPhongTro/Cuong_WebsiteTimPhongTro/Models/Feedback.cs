//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cuong_WebsiteTimPhongTro.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Feedback
    {
        public int FeedbackID { get; set; }
        public Nullable<System.DateTime> FeedbackDate { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FeedbackCustomer { get; set; }
        public string Images { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}