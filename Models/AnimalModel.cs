using Microsoft.AspNetCore.Mvc;
using PetimOl.Models;
using System.Diagnostics;

namespace PetimOl.Models
{
    public class AnimalModel
    {
        public int AnimalID { get; set; }
        public int UserID { get; set; }
        public int RaceID { get; set; }
        public int AnnouncementID { get; set; }
        public string? AnimalName { get; set; }
        public int Age { get; set; }
        public string? Color { get; set; }
        public decimal Weight { get; set; }
        public string? Sickness { get; set; }
        public bool VaccineStatus { get; set; }
        public string? Gender { get; set; }
        public string? ImagePath { get; set; }
        public string? Location { get; set; }
        public string? RaceName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AnimalTypeName { get; set; }
    }
}
