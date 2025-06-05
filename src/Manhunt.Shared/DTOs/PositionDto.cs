using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Datei: Manhunt.Shared/DTOs/PositionDto.cs
namespace Manhunt.Shared.DTOs
{
    public class PositionDto
    {
        public string PlayerId { get; set; }       // Wer sendet die Position
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime TimestampUtc { get; set; }
    }
}

