using System;
using System.ComponentModel.DataAnnotations;
using server.Data;

namespace server.Models
{
    class ChronicleViewModel
    {
        public ChronicleRecord[] Records { get; set; }
    }
}