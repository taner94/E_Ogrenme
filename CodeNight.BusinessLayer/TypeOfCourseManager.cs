using EOgrenme.BusinessLayer.Abstract;
using EOgrenme.BusinessLayer.Result;
using EOgrenme.Entities;
using EOgrenme.Entities.Messages;

namespace EOgrenme.BusinessLayer
{
     public class TypeOfCourseManager : ManagerBase<TypeOfCourse>
    {
        public TypeOfCourse GetCourseTypeById(int? id)
        {
            TypeOfCourse type = Find(x => x.Id == id);
            return type;
        }

    }
}
