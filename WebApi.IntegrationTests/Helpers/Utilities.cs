using DataModel.Model;
using DataModel.Repository;
using Domain.Model;

namespace WebApi.IntegrationTests.Helpers
{
    public static class Utilities
    {
        public static void InitializeDbForTests(AbsanteeContext db)
        {
            db.Colaborators.AddRange(GetSeedingColaboratorsDataModel());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(AbsanteeContext db)
        {
            db.Colaborators.RemoveRange(db.Colaborators);
            InitializeDbForTests(db);
        }

        public static void ClearDbForTests(AbsanteeContext db)
        {
            db.Colaborators.RemoveRange(db.Colaborators);
            db.SaveChanges();
        }

        public static List<ColaboratorDataModel> GetSeedingColaboratorsDataModel()
        {
            return new List<ColaboratorDataModel>()
            {
                new ColaboratorDataModel(new Colaborator("Catarina Moreira", "catarinamoreira@email.pt", "Street 1", "4000-000")),
                new ColaboratorDataModel(new Colaborator("a", "a@email.pt", "Street 2", "4000-001")),
                new ColaboratorDataModel(new Colaborator("John Doe", "john.doe@email.pt", "Street 3", "4000-002"))
            };
        }
    }
}
