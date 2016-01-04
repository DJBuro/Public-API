using System.Collections.Generic;

namespace OrderTracking.Gps.Dao
{
    public interface IGenericDao<T, ID>
    {
        IList<T> FindAll();

        T FindById(ID id);

        T Create(T instance);

        T Save(T instance);

        void Update(T instance);

        void Delete(T instance);

        void DeleteAll(T type);

        void DeleteAll(IList<T> deleteSet);

        void Close();
    }
}
