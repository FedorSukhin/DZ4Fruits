using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ4Fruits.Model
{
    internal class FruitsVegetable
    {
        //создаем сущность
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public int Calorie {get; set; }

        public override string ToString()
        {
            return $"{Id} - {Name} - {Type} - {Color} - {Calorie}";
        }

    }
}
