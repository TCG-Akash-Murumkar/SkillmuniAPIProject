using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class SULFestModels
    {
    }


    public class tbl_sul_fest_event_registration
    {
        public int id_register { get; set; }
        public int UID { get; set; }        
        public int id_college { get; set; }
        public int id_state { get; set; }
        public int id_city { get; set; }
        public int id_event { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
    }

    public class FestRegistration
    {
        public int UID { get; set; }
        public int id_college { get; set; }
        public int id_state { get; set; }
        public int id_city { get; set; }
        public int id_event { get; set; }
        public int is_new_college { get; set; }
        public string college_name { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string user_name { get; set; }
        public string Email { get; set; }
        public int register_status { get; set; }


    }


    public class tbl_sul_fest_otp
    {
        public int id_otp { get; set; }
        public int UID { get; set; }
        public int id_event { get; set; }
        public string OTP { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }

    }


    public class FestRegResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string OTP_Status { get; set; }



    }
    public class APIPostRes
    {
        public string Status { get; set; }
        public string Message { get; set; }


    }
    public class VerifyOTP
    {
        public int UID { get; set; }
        public int id_event { get; set; }
        public string OTP { get; set; }


    }
    public class VerifyOTPResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }


    }
    public class ResendOTP
    {
        public int UID { get; set; }
        public int id_event { get; set; }
        public string user_name { get; set; }
        public string Email { get; set; }

    }
    public class ResendOTPResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }

    }
    public class EventUserData
    {
        public string Status { get; set; }
        public string Message { get; set; }

    }
    public class SeminarReg
    {
        public int id_seminar { get; set; }
        public int id_user { get; set; }
        public string slot { get; set; }
        public int id_event { get; set; }
        public List<Multislots> slots{ get; set; }




    }
    public class Multislots
    {
        public int slot_id { get; set; }
        public string slot { get; set; }
        public DateTime slot_date { get; set; }





    }
    public class SemFeedback
    {
        public int id_register { get; set; }
        public string ratings { get; set; }
        public string feedback { get; set; }
    }
    public class SemResponse
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public int id_register { get; set; }



    }

    public class HigherEduReg
    {
        public int id_higher_education { get; set; }
        public int id_user { get; set; }
        public string slot { get; set; }
        public int id_event { get; set; }



    }
    public class HigherEduFeedback
    {
        public int id_register { get; set; }
        public string ratings { get; set; }
        public string feedback { get; set; }
    }
    public class HigherEduResponse
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public int id_register { get; set; }



    }
}