
namespace CASh.Core.DBClient
{
    public abstract class AbstractModel
    {
        public static SQLQuery? SQLQuery { get; set; } = null;

        abstract protected void InitModelTables();
    }
}
