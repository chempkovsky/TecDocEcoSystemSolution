using System.Data.Entity;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Models
{                
    public class CarShopMsTecDocContext : DbContext
    {

        protected static string DoCreateConnStr(string ConnStringName)
        {
            if (string.IsNullOrEmpty(ConnStringName))
            {
                return "name=CarShopMsTecDocContext";
            }
            else
            {
                return "name=" + ConnStringName;
            }
        }


        public CarShopMsTecDocContext()
            : base("name=CarShopMsTecDocContext")
        {

        }
        public CarShopMsTecDocContext(string ConnStringName)
            : base(DoCreateConnStr(ConnStringName))
        {

        }



        public DbSet<EnterpriseCategoryItemTecDocDescriptionTDES> EnterpriseCategoryItemTecDocDescriptionTDES { get; set; }
        public DbSet<EnterpriseArticleTecDocDescriptionTDES> EnterpriseArticleTecDocDescriptionTDES { get; set; }
        public DbSet<EnterpriseCategoryTecDocTDES> EnterpriseCategoryTecDocTDES { get; set; }
        public DbSet<EnterpriseCategoryItemTecDocTDES> EnterpriseCategoryItemTecDocTDES { get; set; }

        public DbSet<EnterpriseArticleCategoryItemTDES> EnterpriseArticleCategoryItemTDES { get; set; }
        public DbSet<EnterpriseArticleBrandTDES> EnterpriseArticleBrandTDES { get; set; }
        public DbSet<EnterpriseArticleLookUpTDES> EnterpriseArticleLookUpTDES { get; set; }
        public DbSet<EnterpriseArticleApplicTDES> EnterpriseArticleApplicTDES { get; set; }
        public DbSet<EnterpriseArticleTecDocTDES> EnterpriseArticleTecDocTDES { get; set; }

        public DbSet<EnterpriseCarModelFuelTDES> EnterpriseCarModelFuelTDES { get; set; }
        public DbSet<EnterpriseCarBrandTDES> EnterpriseCarBrandTDES { get; set; }
        public DbSet<EnterpriseCarModelTypeTDES> EnterpriseCarModelTypeTDES { get; set; }
        public DbSet<EnterpriseCarModelTDES> EnterpriseCarModelTDES { get; set; }

    }
}