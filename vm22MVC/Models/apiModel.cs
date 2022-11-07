﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace getAPIstuff.Models
{
    public class apiModel
    {
        public apiModel()
        {

        }
        //making the model
        public apiModel(string response)
        {
            //legger til tekst før api svaret om du vil
            //StringResponse = "String response: " + response;
            StringResponse = response;
        }
        public string Id { get; set; }
        public int ligaId { get; set; }
        public int tournamentId { get; set; }
        public int teamId { get; set; }
        public string date { get; set; }
        public string Url { get; set; }
        public string StringResponse { get; set; }
        public HttpResponseMessage Response { get; set; }

    }
}