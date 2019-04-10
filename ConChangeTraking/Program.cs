using ImpromptuInterface;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConChangeTraking
{
    class Program
    {
        static void Main(string[] args)
        {
            // crea una entidad
            PruebaDTO entidad = new PruebaDTO
            {
                Edad = 2
            };

            // wrapea la entidad y logea los cambios a las propiedades por consola
            var m = Wrapper<PruebaDTO>.Wrap<IPruebaDTO>(entidad);

            // modifica una propiedad
            m.Nombre = "DAVID";

            // modifica una propiedad
            m.Edad = 2;

            Console.ReadLine();
        }
    }


    public class Wrapper<OB> : DynamicObject
    {
        private readonly OB _wrappedObject;

        public static IN Wrap<IN>(OB obj) where IN : class
        {


            return new Wrapper<OB>(obj).ActLike<IN>();
        }

        private Wrapper(OB obj)
        {
            _wrappedObject = obj;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var prop = _wrappedObject.GetType().GetProperty(binder.Name);
            prop.SetValue(_wrappedObject,value);
            Console.WriteLine("CAMBIO LA PROPIEDAD: '{0}' VALOR: '{1}'", binder.Name, value.ToString());
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            try
            {
                result = _wrappedObject.GetType().GetMethod(binder.Name).Invoke(_wrappedObject, args);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}
