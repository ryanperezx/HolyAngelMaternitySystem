using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyAngelMaternitySystem
{
   class PatientRecord
    {

        //patient basic info
        public string patientID
        {
            get;
            set;
        }

        public string fullName
        {
            get;
            set;
        }

        public string birthDate
        {
            get;
            set;
        }
        
        public string address
        {
            get;
            set;
        }

        public int age
        {
            get;
            set;
        }

        public string status
        {
            get;
            set;
        }

        public string contactNo
        {
            get;
            set;
        }

        //patient record

        public double weight
        {
            get;
            set;
        }

        public string date
        {
            get;
            set;
        }

        public string bloodPressure
        {
            get;
            set;
        }

        public string aog
        {
            get;
            set;
        }

        public string eut
        {
            get;
            set;
        }

        //doctor analysis

        public string diagnosis
        {
            get;
            set;
        }

        public string findings
        {
            get;
            set;
        }

        public string treatment
        {
            get;
            set;
        }


    }
}
