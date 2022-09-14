// CarShop.Models.LookUpSoatoForEnterpriseAddress
using PagedList;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Models
{

    public class LookUpSoatoForEnterpriseAddress
    {
        public IPagedList<Soato> SoatoList
        {
            get;
            set;
        }

        public string RedirecData
        {
            get;
            set;
        }

        public string RedirectContriller
        {
            get;
            set;
        }

        public string RedirectAction
        {
            get;
            set;
        }
    }
}