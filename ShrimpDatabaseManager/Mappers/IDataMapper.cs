using System.Data;

namespace ShrimpDatabaseManager.Mappers
{
    /// <summary>
    /// (Pattern) Contrato para o Data Mapper.
    /// A responsabilidade desta interface é desacoplar o modelo relacional (IDataReader)
    /// do modelo de objetos (a entidade de domínio).
    /// </summary>
    /// <typeparam name="T">A entidade de domínio a ser mapeada.</typeparam>
    public interface IDataMapper<T> where T : class
    {
        T Map(IDataReader reader);
    }
}