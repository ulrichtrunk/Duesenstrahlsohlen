using Shared.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class CalculationViewModel
    {
        public int? Id { get; set; }

        [Required]
        [Display(Name = "Width")]
        [Range(typeof(decimal), "0", "1000")]
        public decimal Width { get; set; }

        [Required]
        [Display(Name = "Height")]
        [Range(typeof(decimal), "0", "1000")]
        public decimal Height { get; set; }

        [Required]
        [Display(Name = "Border X-Axis")]
        [Range(typeof(decimal), "0", "1000")]
        public decimal BorderX { get; set; }

        [Required]
        [Display(Name = "Border Y-Achse")]
        [Range(typeof(decimal), "0", "1000")]
        public decimal BorderY { get; set; }

        [Required]
        [Display(Name = "Drilling Point Distance X-Axis")]
        [Range(typeof(decimal), "0", "1000")]
        public decimal DrillingPointDistanceX { get; set; }

        [Required]
        [Display(Name = "Drilling Point Distance Y-Achse")]
        [Range(typeof(decimal), "0", "1000")]
        public decimal DrillingPointDistanceY { get; set; }

        [Required]
        [Display(Name = "Sealing Slab Diameter")]
        [Range(typeof(decimal), "0", "1000")]
        public decimal SealingSlabDiameter { get; set; }

        [Required]
        [Display(Name = "Depth")]
        [Range(typeof(decimal), "0", "1000")]
        public decimal Depth { get; set; }

        [Required]
        [Display(Name = "Pixels Per Meter")]
        [Range(typeof(int), "1", "30")]
        public int PixelsPerMeter { get; set; }

        [Required]
        [Display(Name = "Standard Derivation Offset")]
        [Range(typeof(decimal), "0", "100")]
        public decimal StandardDerivationOffset { get; set; }

        [Required]
        [Display(Name = "Standard Derivation Radius")]
        [Range(typeof(decimal), "0", "10")]
        public decimal StandardDerivationRadius { get; set; }

        [Required]
        [Display(Name = "Standard Derivation Drilling Point")]
        [Range(typeof(decimal), "0", "10")]
        public decimal StandardDerivationDrillingPoint { get; set; }

        [Display(Name = "Unset Area")]
        public decimal? UnsetAreaResult { get; set; }

        [Required]
        [Display(Name = "Water Level Difference")]
        [Range(typeof(decimal), "0", "1000")]
        public decimal WaterLevelDifference { get; set; }

        [Required]
        [Display(Name = "Sealing Slab Thickness")]
        [Range(typeof(decimal), "0", "1000")]
        public decimal SealingSlabThickness { get; set; }

        [Required]
        [Display(Name = "Permeability Of Sole (Without Unset Area)")]
        [Range(typeof(decimal), "0", "1000")]
        public decimal PermeabilityOfSoleWithoutUnsetArea { get; set; }

        [Required]
        [Display(Name = "Permeability Of Sole (At Unset Area)")]
        [Range(typeof(decimal), "0", "1000")]
        public decimal PermeabilityOfSoleAtUnsetArea { get; set; }

        [Display(Name = "Residual Water Amount")]
        public decimal? ResidualWaterResult { get; set; }

        [Required]
        [Display(Name = "Iterations")]
        [Range(typeof(int), "1", "10000")]
        public int Iterations { get; set; }

        // Results
        public bool HasBasePreview { get; set; }
        public bool HasDerivationPreview { get; set; }
        public string StateName { get; set; }
        public int MaxIteration { get; set; }

        // Properties
        public bool ReadOnly { get { return Id > 0; } }
    }
}