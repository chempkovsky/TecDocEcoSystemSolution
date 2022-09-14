using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TecDocEcoSystemDbClassLibrary
{
    public class DICT_TD : IEquatable<DICT_TD>
    {
        public int DictId { get; set; }
        public string DictTitle { get; set; }

        public bool Equals(DICT_TD other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return this.DictId.Equals(other.DictId);
        }
        public override int GetHashCode()
        {

            //Get hash code for the Name field if it is not null.
            int hashDictTitle = DictTitle == null ? 0 : DictTitle.GetHashCode();

            //Get hash code for the Code field.
            int hashDictId = DictId.GetHashCode();

            //Calculate the hash code for the product.
            return hashDictTitle ^ hashDictId;
        }
    }
}