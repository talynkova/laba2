using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserApp
{
    public class SecurityRisk: ICloneable
    {

        private sbyte сonfidentialityViolation = -1;
        private sbyte integrityViolation = -1;
        private sbyte availabilityViolation = -1;
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SourceOfThreat { get; set; }
        public string ObjectOfImpact { get; set; }
        public string ConfidentialityViolation
        {
            get
            {
                if (сonfidentialityViolation == 1)
                {
                    return "да";
                }
                else if (сonfidentialityViolation == 0) return "нет";
                else return "";
            }
            set
            {
                if (value == "0" || value == "1")
                {
                    сonfidentialityViolation = Convert.ToSByte(value);
                }
                else
                {
                    сonfidentialityViolation = -1;
                }
            }
        }
        public string IntegrityViolation
        {
            get
            {
                if (integrityViolation == 1)
                {
                    return "да";
                }
                else if (integrityViolation == 0) return "нет";
                else return "";
            }
            set
            {
                if (value == "0" || value == "1")
                {
                    integrityViolation = Convert.ToSByte(value);
                }
                else
                {
                    integrityViolation = -1;
                }
            }
        }
        public string AvailabilityViolation
        {
            get
            {
                if (availabilityViolation == 1)
                {
                    return "да";
                }
                else if (availabilityViolation == 0) return "нет";
                else return "";
            }
            set
            {
                if (value == "0" || value == "1")
                {
                    availabilityViolation = Convert.ToSByte(value);
                }
                else
                {
                    availabilityViolation = -1;
                }
            }
        }
        public object Clone()
        {
            return new SecurityRisk()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                SourceOfThreat = SourceOfThreat,
                ObjectOfImpact = ObjectOfImpact,
                сonfidentialityViolation = сonfidentialityViolation,
                integrityViolation = integrityViolation,
                availabilityViolation = availabilityViolation
            };
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            SecurityRisk r = obj as SecurityRisk;
            if (r as SecurityRisk == null)
                return false;
            return r.Id.Equals(Id) && r.Name.Equals(Name) &&r.Description.Equals(Description) && r.SourceOfThreat.Equals(SourceOfThreat) &&r.ObjectOfImpact.Equals(ObjectOfImpact) &&r.сonfidentialityViolation.Equals(сonfidentialityViolation) &&r.integrityViolation.Equals(integrityViolation) &&r.availabilityViolation.Equals(availabilityViolation);
        }
        public static bool operator ==(SecurityRisk risk1, SecurityRisk risk2)
        {
            return Equals(risk1, risk2);
        }

        public static bool operator !=(SecurityRisk risk1, SecurityRisk risk2)
        {
            return !Equals(risk1, risk2);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = сonfidentialityViolation.GetHashCode();
                hashCode = (hashCode * 397) ^ integrityViolation.GetHashCode();
                hashCode = (hashCode * 397) ^ availabilityViolation.GetHashCode();
                hashCode = (hashCode * 397) ^ (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SourceOfThreat != null ? SourceOfThreat.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ObjectOfImpact != null ? ObjectOfImpact.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
    }


