using System;

namespace ApplicationService.DTOs
{
    public class RigPartDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public DateTime? IssuedOn { get; set; }


        public string Description { get; set; }


        public float? Warranty { get; set; }

        public virtual bool Validate()
        {
            var nameOk = !string.IsNullOrEmpty(Name) && Name.Length < 50;
            var descriptionOk = true;
            if (Description != null) descriptionOk = Description.Length < 600;
            return nameOk && descriptionOk;
        }

        public override string ToString()
        {
            string output = $"Name: {Name},\nPrice: {Price}";
            if (IssuedOn != null) output += $",\nIssued on: {IssuedOn}";
            if (Description != null) output += $",\nDescription: {Description}";
            if (Warranty != null) output += $",\nWarranty: {Warranty} years";
            return output;
        }
    }
}