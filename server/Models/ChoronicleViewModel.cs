using System;
using System.ComponentModel.DataAnnotations;
using server.Data;

namespace server.Models
{
    class ChoronicleViewModel
    {
        public ChoronicleRecord[] Records { get; set; }
    }
}