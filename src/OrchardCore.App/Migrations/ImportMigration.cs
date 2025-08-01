using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;

namespace OrchardCoreApp.Migrations;

public class ImportMigration(IRecipeMigrator recipes) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await recipes.ExecuteAsync("import.json", this);
        return 1;
    }
}