/*
 * Created by: Beybit Abdikaykov
 * Created: 17 августа 2014
 * Copyright © Aquametr
 */

using System;

namespace Bonbon.Client
{
    public class CardDetails
    {
        public string CardNumber { get; set; }
        
        public string CardHolderName { get; set; }
        
        public DateTime LastActivityDate { get; set; }
        
        public DateTime ExpirationDate { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public double Bonus { get; set; }
    }
}