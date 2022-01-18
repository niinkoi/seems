﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEEMS.Models
{
    public class Reservation
    {

        [Key]
        public int Id { get; set; }

        public bool Attend { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime LastUpdateAt { get; set; }

        public decimal DiscountPercent { get; set; }

        public decimal TotalPrice { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }

        public Event Event { get; set; }

        public List<FeedBack> FeedBacks { get; set; }

    }
}