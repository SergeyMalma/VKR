using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Models
{
    public class Enrollee
    {
        public int EnrolleeId { get; set; }//Код аббитуриента
        public int MathsPoint { get; set; }//Количество баллов по профильной математике
        public int InformaticsPoint { get; set; } //Количество баллов по информатике
        public int RLPoint { get; set; }// Количество баллов по русскому языку
        public string CodeOfDirection { get; set; } // Код направления
        public DateTime DateOfApplication { get; set; } //Дата подачи заявления
        public int Hostel { get; set; }//Потребность в общежитие
        public int Privileges { get; set; }//Льготное поступление
        public int TotalPoint{ get; set; }



}
}
